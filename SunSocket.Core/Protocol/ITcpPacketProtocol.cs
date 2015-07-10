using System.Net.Sockets;
using SunSocket.Core.Session;
using SunSocket.Core.Buffer;

namespace SunSocket.Core.Protocol
{
    public interface ITcpPacketProtocol
    {
        /// <summary>
        /// 归属session
        /// </summary>
        ITcpSession Session { get; set; }
        //数据发送缓冲器
        IFixedBuffer SendBuffer { get; set; }
        /// <summary>
        /// 发送指令
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        bool SendAsync(SendCommond cmd);
        void SendComplate(object sender, SocketAsyncEventArgs sendEventArgs);
        void ReceiveComplate(object sender, SocketAsyncEventArgs receiveEventArgs);
        /// <summary>
        /// 清理协议管理器
        /// </summary>
        void Clear();
    }
}
