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

namespace SunSocket.Framework.Session
{
    public class UdpSession : IUdpSession
    {
        public UdpSession(EndPoint remoteEndPoint,IUdpAsyncServer server)
        {
            RemoteEndPoint = remoteEndPoint;
            Server = server;
        }

        public EndPoint RemoteEndPoint
        {
            get;set;
        }

        public IUdpAsyncServer Server
        {
            get;set;
        }

        public void SendAsync(SendCommond cmd)
        {
            Server.SendAsync(RemoteEndPoint, cmd);
        }
    }
}
