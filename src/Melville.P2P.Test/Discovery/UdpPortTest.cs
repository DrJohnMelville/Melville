using System.Threading.Tasks;
using Melville.P2P.Raw.NetworkPrimatives;
using Xunit;

namespace Melville.P2P.Test.Discovery
{
    public class UdpPortTest
    {
        int received = 0;
        [Fact]
        public async Task SimpleSend()
        {
            var data = new byte[10];
            using var port = MakePort();
            var retLoop = ReadLoop(port);
            await new UdpBroadcaster(70).Send(data);
            await retLoop;
            Assert.Equal(1, received);
        }

        private async Task ReadLoop(UdpReceiver port)
        {
            await foreach (var packed in port.WaitForReads())
            {
                received++;
                break;
            }

        }

        private UdpReceiver MakePort()
        {
            return new UdpReceiver(70);
        }
    }
}