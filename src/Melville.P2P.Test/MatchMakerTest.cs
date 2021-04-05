using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Threading.Tasks;
using Melville.P2P.Raw.BinaryObjectPipes;
using Melville.P2P.Raw.Matchmaker;
using Xunit;

namespace Melville.P2P.Test
{
    public class MatchMakerTest
    {
        public class Message : ICanWriteToPipe
        {
            public string Text { get; }
            public Message(string text) => Text = text;

            public static Message? ReadMessage(ref SequenceReader<byte> reader) =>
                !reader.TryRead(out string? str) ? null : new Message(str);

            public void WriteToPipe(PipeWriter write)
            {
                using var pw = new SerialPipeWriter(write, 200);
                pw.Write(Text);
            }
        }

        private readonly TaskCompletionSource<String> fakeConsole = new TaskCompletionSource<string>();

        [Fact]
        public async Task SendMessage()
        {
            var dict = new BinaryObjectDictionary();
            dict.Register<Message>(Message.ReadMessage);
            using var server = new MatchmakerServer(71, dict);
            MonitorServer(server);

            using var client = new MatchMakerClient(71, dict);
            var (reader, writer) = await client.Connect();
            await writer.Write(new Message("This is a test"));

            var received = await fakeConsole.Task;
            Assert.Equal("This is a test", received);

        }

        async void MonitorServer(MatchmakerServer server)
        {
            await foreach (var (reader, writer) in server.AcceptConnections())
            {
                MonitorReader(reader);
            }
        }

        async void MonitorReader(BinaryObjectPipeReader reader)
        {
            await foreach (Message msg in reader.Read())
            {
                fakeConsole.SetResult(msg.Text);
                break;
            }
        }

    }
}