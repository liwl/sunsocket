using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SunSocket.Core;
using SunSocket.Core.Protocol;
using SunSocket.Core.Session;

namespace SunSocket.Framework
{
    public class Listener : IListener
    {
        private Socket listenerSocket;
        IPEndPoint localEndPoint;
        IAsyncServer server;
        ConfigInfo config;
        public Listener(ConfigInfo config)
        {
            this.config = config;
            this.server = new AsyncServer(config);
            localEndPoint = new IPEndPoint(IPAddress.Parse(config.LConfig.IP), config.LConfig.Port);
        }

        public IAsyncServer AsyncServer
        {
            get
            {
                return server;
            }
        }

        public void Start()
        {
            if (this.listenerSocket == null)
            {
                this.listenerSocket = new Socket(this.localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                this.listenerSocket.Bind(this.localEndPoint);
                this.listenerSocket.Listen(this.config.ListenerBackLog);
                server.ListenerSocket =this.listenerSocket;
                server.StartAccept(null);
            }
        } 

        public void Stop()
        {
            if (this.listenerSocket != null)
            {
                this.listenerSocket.Close();
                this.listenerSocket = null;
            }
        }
    }
}
