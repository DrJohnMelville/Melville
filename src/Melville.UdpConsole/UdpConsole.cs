using System;
using System.Net.Sockets;
using System.Text;

namespace Melville.UdpConsole
{
    public static class UdpConsole
    {
        private static UdpClient? client = null;
        private static UdpClient Client
        {
            get
            {
                client ??= new UdpClient();
                return client ;
            }
        }

        public static string WriteLine(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            Client.Send(bytes, bytes.Length, "127.0.0.1", 15321);
            return str;
        }
    }
}