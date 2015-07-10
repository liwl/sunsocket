using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SunSocket.Framework;
using SunSocket.Core.Protocol;
using SunSocket.Framework.Session;
using SunSocket.Core.Session;

namespace SunSocket.Client
{
    class Program
    {
        static ITcpClientSession Session;
        static void Main(string[] args)
        {
            var config = new ConfigInfo();
            config.BufferSize = 1024 * 4;
            config.MaxConnections = 60000;
            AsyncClient client = new AsyncClient(config);
            client.OnReceived+= ReceiveCommond;
            client.OnConnected += Connected;
            client.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.105"),9989));
            Console.ReadLine();
            short i =0;
            while (i<500)
            {
                Task.Delay(1).Wait();
                i++;
                var data = Encoding.UTF8.GetBytes("测试数据"+i);
                Session.SendAsync(new SendCommond() { CommondId = i, Buffer = data });
            }
            Console.ReadLine();
        }
        public static void Connected(object sender, ITcpClientSession session)
        {
            Console.WriteLine("连接成功，开始接受数据");
            Session = session;
        }
        public static void ReceiveCommond(object sender, ReceiveCommond cmd)
        {
            TcpClientSession session = sender as TcpClientSession;
            string msg = Encoding.UTF8.GetString(cmd.Data);
            //li.Add(string.Format("sessionId:{0},cmdId:{1},msg:{2}", session.SessionId, cmd.CommondId, msg));
            Console.WriteLine("sessionId:{0},cmdId:{1},msg:{2}", session.SessionId, cmd.CommondId, msg);
            session.StartReceiveAsync();
        }
    }
}
