using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using SunSocket.Framework;
using SunSocket.Framework.Config;
using SunSocket.Framework.Session;
using SunSocket.Core.Protocol;
using SunSocket.Core.Session;

namespace SunSocket
{
    class Program
    {
        static List<string> li = new List<string>();
        static void Main(string[] args)
        {
            var config = new ConfigInfo();
            config.BufferSize = 1024 * 4;
            config.MaxConnections = 100000;
            config.LConfig = new ListenerConfig();
            config.LConfig.Name = "one";
            config.LConfig.IP = "192.168.1.105";
            config.LConfig.Port = 9989;
            Listener listener = new Listener(config);
            listener.AsyncServer.OnReceived += ReceiveCommond;
            listener.Start();
            var configOne = new ConfigInfo();
            configOne.BufferSize = 1024 * 4;
            configOne.MaxConnections = 100000;
            configOne.LConfig = new ListenerConfig();
            configOne.LConfig.Name = "two";
            configOne.LConfig.IP = "192.168.1.105";
            configOne.LConfig.Port = 9988;
            Listener listenerOne = new Listener(configOne);
            listenerOne.AsyncServer.OnReceived += ReceiveCommond;
            listenerOne.Start();
            Console.WriteLine("服务器已启动");
            //Task.Run(() =>
            //{
            //    while (true)
            //    {
            //        Task.Delay(2000).Wait();
            //        Console.WriteLine(listener.AsyncServer.OnlineList.Count());
            //    }
            //});
            Console.ReadLine();
        }
        static byte[] data = Encoding.UTF8.GetBytes("测试数据服务器返回");
        static SendCommond sdata = new SendCommond() { CommondId = 1, Buffer = data, Offset = 0};
        public static void ReceiveCommond(object sender, ReceiveCommond cmd)
        {
            TcpSession session = sender as TcpSession;
            string msg = Encoding.UTF8.GetString(cmd.Data);
            //li.Add(string.Format("sessionId:{0},cmdId:{1},msg:{2}", session.SessionId, cmd.CommondId, msg));
            Console.WriteLine("sessionId:{0},cmdId:{1},msg:{2}", session.SessionId, cmd.CommondId, msg);
            //for (int i = 0; i < 3; i++)
            //{
           
            session.SendAsync(sdata);
            //}
        }
    }
}
