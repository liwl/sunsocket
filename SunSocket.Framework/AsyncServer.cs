using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SunSocket.Core;
using SunSocket.Core.Session;
using System.Collections.Concurrent;
using SunSocket.Core.Protocol;
using SunSocket.Framework.Protocol;
using SunSocket.Framework.Session;

namespace SunSocket.Framework
{
    public class AsyncServer : IAsyncServer
    {
        private ConfigInfo config;
        private ITcpSessionPool sessionPool;
        public Socket ListenerSocket { get; set; }
        public ConcurrentDictionary<string, ITcpSession> OnlineList { get; set; }

        public string Name
        {
            get;
            set;
        }

        public AsyncServer(ConfigInfo config)
        {
            this.config = config;
            this.sessionPool = new TcpSessionPool(config);
            this.OnlineList = new ConcurrentDictionary<string, ITcpSession>();
        }

        //当接收到命令包时触发
        public event EventHandler<ReceiveCommond> OnReceived;
        //当收到请求时触发
        public event EventHandler<ITcpSession> OnConnected;
        //断开连接事件
        public event EventHandler<ITcpSession> OnDisConnect;

        private void ProcessAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            ITcpSession session = sessionPool.Pop();
            session.Server = this;
            OnlineList.TryAdd(session.SessionId, session);
            session.ConnectSocket = acceptEventArgs.AcceptSocket;
            session.ConnectDateTime = DateTime.Now;
            session.ActiveDateTime = session.ConnectDateTime;
            if (OnConnected != null)
                OnConnected(this, session);//启动连接请求通过事件
            session.StartReceiveAsync();//开始接收数据
            StartAccept(acceptEventArgs); //把当前异步事件释放，等待下次连接
        }
        public void ReceiveCommond(ITcpSession session, ReceiveCommond cmd)
        {
            OnReceived(session, cmd);
        }
        
        public void StartAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            if (acceptEventArgs == null)
            {
                acceptEventArgs = new SocketAsyncEventArgs();
                acceptEventArgs.Completed +=AcceptCompleted;
            }
            else
            {
                acceptEventArgs.AcceptSocket = null; //释放上次绑定的Socket，等待下一个Socket连接
            }
            bool willRaiseEvent = this.ListenerSocket.AcceptAsync(acceptEventArgs);
            if (!willRaiseEvent)
            {
                ProcessAccept(acceptEventArgs);
            }
        }
        private void AcceptCompleted(object sender, SocketAsyncEventArgs acceptEventArgs)
        {
            try
            {
                ProcessAccept(acceptEventArgs);
            }
            catch (Exception e)
            {
                //日志记录错误
            }
        }
        public void CloseSession(ITcpSession sesseion)
        {
            if (OnDisConnect != null)
                OnDisConnect(this, sesseion);
            sesseion.Clear();//自清理
            sessionPool.Push(sesseion);
            OnlineList.TryRemove(sesseion.SessionId, out sesseion);
        }
    }
}
