using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using SunSocket.Core.Buffer;
using SunSocket.Core.Protocol;

namespace SunSocket.Core.Session
{
    public interface ITcpSession
    {
        string SessionId { get; set; }
        /// <summary>
        /// 连接时间
        /// </summary>
        DateTime ConnectDateTime { get; set; }
        /// <summary>
        /// 最后活动时间
        /// </summary>
        DateTime ActiveDateTime { get; set; }
        /// <summary>
        /// session数据容器
        /// </summary>
        DataContainer SessionData { get; set; }
        /// <summary>
        /// 连接套接字
        /// </summary>
        Socket ConnectSocket { get; set; }
        /// <summary>
        /// 该session的归属server
        /// </summary>
        IAsyncServer Server { get; set; }
        /// <summary>
        /// 包协议解析器
        /// </summary>
        ITcpPacketProtocol PacketProtocol { get; set; }
        /// <summary>
        /// 接收数据
        /// </summary>
        SocketAsyncEventArgs ReceiveEventArgs { get; set; }
        /// <summary>
        /// 发送数据
        /// </summary>
        SocketAsyncEventArgs SendEventArgs { get; set; }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="cmd"></param>
        void SendAsync(SendCommond cmd);
        /// <summary>
        /// 开始接收数据
        /// </summary>
        void StartReceiveAsync();
        /// <summary>
        /// 清空session
        /// </summary>
        void Clear();
        /// <summary>
        /// soket接受数据完成事件
        /// </summary>
        event EventHandler<SocketAsyncEventArgs> OnSocketReceived;
        /// <summary>
        /// 接受数据处理完成事件(t1:)
        /// </summary>
        event Action<string, byte[], ITcpSession> OnReceiveProcessed;
    }
}
