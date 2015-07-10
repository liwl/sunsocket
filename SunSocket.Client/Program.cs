using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SunSocket.Framework;
using SunSocket.Core;
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
            AsyncClient client = new AsyncClient(1024, 1024 * 4, new Loger());
            client.OnReceived+= ReceiveCommond;
            client.OnConnected += Connected;
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"),9989));
            Console.ReadLine();
            short i =0;
            while (i<5)
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
            Console.WriteLine(message);
        }

        public void Fatal(Exception e)
        {
            Console.WriteLine(e.Message);
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
