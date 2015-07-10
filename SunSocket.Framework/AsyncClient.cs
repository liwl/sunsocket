using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SunSocket.Core;
using SunSocket.Core.Protocol;
using SunSocket.Core.Session;
using SunSocket.Framework.Session;
using SunSocket.Framework.Protocol;

namespace SunSocket.Framework
{
    public class AsyncClient : IAsyncClient
    {
        int bufferPoolSize, bufferSize;
        ILoger loger;
        public AsyncClient(int bufferPoolSize, int bufferSize, ILoger loger)
        {
            this.bufferPoolSize = bufferPoolSize;
            this.bufferSize = bufferSize;
            this.loger = loger;
        }
        public event EventHandler<ITcpClientSession> OnConnected;
        public event EventHandler<ReceiveCommond> OnReceived;
        public event EventHandler<ITcpClientSession> OnDisConnect;
        public void Connect(EndPoint remoteEndPoint) 
        {
            TcpClientSession session = new TcpClientSession(remoteEndPoint,bufferPoolSize,bufferSize,loger);
            session.OnReceived += OnReceived;
            session.OnConnected += OnConnected;
            session.OnDisConnect += OnDisConnect;
            session.Connect();
        }

        public void Disconnect(ITcpClientSession sesseion)
        {
            sesseion.DisConnect();
        }
    }
}
