using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunSocket.Core.Session;
using SunSocket.Core.Protocol;
using SunSocket.Framework;

namespace SunSocket.Udp
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpAsyncServer server = new UdpAsyncServer(8878, 10, 4 * 1024);
            server.OnReceived += ReceiveCompleted;
            server.Start();
            Console.ReadLine();
        }
        static void ReceiveCompleted(object sender, ReceiveCommond e)
        {
            IUdpSession session = sender as IUdpSession;
            Console.WriteLine(Encoding.UTF8.GetString(e.Data));
            session.SendAsync(new SendCommond() { CommondId=e.CommondId,Buffer=e.Data});
        }
    }
}
