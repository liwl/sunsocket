using System.Net;
using SunSocket.Core.Protocol;

namespace SunSocket.Core.Session
{
    public interface IUdpSession
    {
        IUdpAsyncServer Server { get; set; }
        EndPoint RemoteEndPoint { get; set; }
        void SendAsync(SendCommond cmd);
    }
}
