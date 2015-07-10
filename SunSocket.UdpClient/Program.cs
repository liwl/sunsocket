using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using SunSocket.Core.Session;
using SunSocket.Core.Protocol;
using SunSocket.Framework;

namespace SunSocket.UdpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpAsyncServer server = new UdpAsyncServer(8879, 10, 4 * 1024);
            server.OnReceived += ReceiveCompleted;
            server.Start();
            Console.ReadLine();
            while (true)
            {
                server.SendAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8878), new SendCommond() { CommondId = 11, Buffer = Encoding.UTF8.GetBytes("我爱我的祖国啊啊啊啊,测试测试") });
                Console.ReadLine();
            }
        }
        static void ReceiveCompleted(object sender, ReceiveCommond e)
        {
            IUdpSession session = sender as IUdpSession;
            Console.WriteLine(Encoding.UTF8.GetString(e.Data));
        }
    }
}
