using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using SunSocket.Framework;
using SunSocket.Framework.Config;
using SunSocket.Framework.Session;
using SunSocket.Core;
using SunSocket.Core.Protocol;
using SunSocket.Core.Session;

namespace SunSocket
{
    class Program
    {
        static List<string> li = new List<string>();
        static void Main(string[] args)
        {
            var loger = new Loger();
            var config = new TcpServerConfig();
            config.BufferSize = 1024 * 4;
            config.MaxConnections = 100000;
            Framework.Listener listener = new Framework.Listener(config,new ServerEndPoint() {Name="one",IP="127.0.0.1",Port=9989 }, loger);
            listener.AsyncServer.OnReceived += ReceiveCommond;
            listener.Start();
            Framework.Listener listenerOne = new Framework.Listener(config, new ServerEndPoint() { Name = "one", IP = "127.0.0.1", Port = 9988 }, loger);
            listenerOne.AsyncServer.OnReceived += ReceiveCommond;
            listenerOne.Start();
            Console.WriteLine("服务器已启动");
            Console.ReadLine();
        }
        static byte[] data = Encoding.UTF8.GetBytes("测试数据服务器返回");
        static SendCommond sdata = new SendCommond() { CommondId = 1, Buffer = data, Offset = 0};
        public static void ReceiveCommond(object sender, ReceiveCommond cmd)
        {
            TcpSession session = sender as TcpSession;
            string msg = Encoding.UTF8.GetString(cmd.Data);
            Console.WriteLine("sessionId:{0},cmdId:{1},msg:{2}", session.SessionId, cmd.CommondId, msg);
            session.SendAsync(sdata);
        }
    }
    public class Loger : ILoger
    {
        public void Debug(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message)
        {
            throw new NotImplementedException();
        }

        public void Error(Exception e)
        {
            throw new NotImplementedException();
        }

        public void Error(string message)
        {
            throw new NotImplementedException();
        }

        public void Fatal(Exception e)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message)
        {
            throw new NotImplementedException();
        }

        public void Info(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Info(string message)
        {
            throw new NotImplementedException();
        }

        public void Log(string message)
        {
            throw new NotImplementedException();
        }

        public void Trace(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Trace(string message)
        {
            throw new NotImplementedException();
        }

        public void Warning(Exception e)
        {
            throw new NotImplementedException();
        }

        public void Warning(string message)
        {
            throw new NotImplementedException();
        }
    }
}
