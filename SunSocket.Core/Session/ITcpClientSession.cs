using System;
using System.Net.Sockets;
using SunSocket.Core.Protocol;


namespace SunSocket.Core.Session
{
    public interface ITcpClientSession 
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
        /// 连接套接字
        /// </summary>
        Socket ConnectSocket { get; set; }
        /// <summary>
        /// 包协议解析器
        /// </summary>
        ITcpClientPacketProtocol PacketProtocol { get; set; }
        /// <summary>
        /// 接收数据
        /// </summary>
        SocketAsyncEventArgs ReceiveEventArgs { get; set; }
        /// <summary>
        /// 发送数据
        /// </summary>
        SocketAsyncEventArgs SendEventArgs { get; set; }
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="session">session对象</param>
        /// <param name="cmd"></param>
        void ReceiveCommond(ITcpClientSession session, ReceiveCommond cmd);
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
        /// 连接
        /// </summary>
        void Connect();
        /// <summary>
        /// 清空session
        /// </summary>
        void DisConnect();
        /// <summary>
        /// 连接成功事件
        /// </summary>
        event EventHandler<ITcpClientSession> OnConnected;
        /// <summary>
        /// 断开连接事件
        /// </summary>
        event EventHandler<ITcpClientSession> OnDisConnect;
        /// <summary>
        /// 收到指令事件
        /// </summary>
        event EventHandler<ReceiveCommond> OnReceived;
    }
}
