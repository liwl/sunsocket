using System;
using SunSocket.Core.Session;
using System.Net;
using System.Net.Sockets;
using SunSocket.Core.Protocol;
using SunSocket.Core;
using SunSocket.Framework.Protocol;

namespace SunSocket.Framework.Session
{
    public class TcpClientSession : ITcpClientSession
    {
        EndPoint remoteEndPoint;
        byte[] receiveBuffer;
        public TcpClientSession(EndPoint remoteEndPoint,ConfigInfo config)
        {
            this.remoteEndPoint = remoteEndPoint;
            SessionId = ObjectId.GenerateNewId().ToString();//生成唯一sesionId
            ReceiveEventArgs = new SocketAsyncEventArgs();
            ReceiveEventArgs.RemoteEndPoint = remoteEndPoint;
            SendEventArgs = new SocketAsyncEventArgs();
            SendEventArgs.RemoteEndPoint = remoteEndPoint;
            PacketProtocol = new TcpClientPacketProtocol(config.BufferSize, config.MaxBufferPoolSize);
            PacketProtocol.Session = this;
            SendEventArgs.Completed += PacketProtocol.SendComplate;//数据发送完成事件
            receiveBuffer = new byte[config.BufferSize];
        }
        public DateTime ActiveDateTime
        {
            get;set;
        }

        public DateTime ConnectDateTime
        {
            get;set;
        }
        Socket connectSocket;
        public Socket ConnectSocket
        {
            get { return connectSocket; }
            set
            {
                connectSocket = value;
                if (connectSocket == null) //清理缓存
                {
                    PacketProtocol.Clear();
                }
                ReceiveEventArgs.AcceptSocket = connectSocket;
                SendEventArgs.AcceptSocket = connectSocket;
            }
        }

        public ITcpClientPacketProtocol PacketProtocol
        {
            get;set;
        }

        public SocketAsyncEventArgs ReceiveEventArgs
        {
            get;set;
        }

        public SocketAsyncEventArgs SendEventArgs
        {
            get;set;
        }

        public string SessionId
        {
            get;set;
        }

        public event EventHandler<ITcpClientSession> OnDisConnect;
        //当接收到命令包时触发
        public event EventHandler<ReceiveCommond> OnReceived;
        public event EventHandler<ITcpClientSession> OnConnected;

        private Socket localSocket;
        public void Connect()
        {
            if (localSocket == null)
                localSocket = new Socket(remoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            ReceiveEventArgs.Completed += ConnectComplate;
            localSocket.ConnectAsync(ReceiveEventArgs);
        }
        private void ConnectComplate(object sender, SocketAsyncEventArgs asyncEventArgs)
        {
            ReceiveEventArgs.Completed -= ConnectComplate;
            if (asyncEventArgs.SocketError == SocketError.Success)
            {
                ReceiveEventArgs.Completed += PacketProtocol.ReceiveComplate;
                ReceiveEventArgs.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
                ConnectSocket = asyncEventArgs.ConnectSocket;
                StartReceiveAsync();
                if (OnConnected != null)
                    OnConnected(asyncEventArgs, this);//响应连接成功事件
            }
            else
            {
                throw new Exception("连接失败");
                //记录连接失败
            }
        }
        public void DisConnect()
        {
            OnDisConnect(null, this);
            //释放引用，并清理缓存，包括释放协议对象等资源
            PacketProtocol.Clear();
            if (ConnectSocket == null)
                return;
            try
            {
                ConnectSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception e)
            {
                //日志记录
                // Program.Logger.ErrorFormat("CloseClientSocket Disconnect client {0} error, message: {1}", socketInfo, E.Message);
            }
            ConnectSocket.Close();
            ConnectSocket = null;
        }
        public void ReceiveCommond(ITcpClientSession session, ReceiveCommond cmd)
        {
            OnReceived(this, cmd);
        }
        public void SendAsync(SendCommond cmd)
        {
            PacketProtocol.SendAsync(cmd);
        }

        public void StartReceiveAsync()
        {
            try
            {
                bool willRaiseEvent = ConnectSocket.ReceiveAsync(ReceiveEventArgs); //投递接收请求
                if (!willRaiseEvent)
                {
                    PacketProtocol.ReceiveComplate(null, ReceiveEventArgs);
                }
            }
            catch (Exception e)
            {
                //日志记录
            }
        }
    }
}
