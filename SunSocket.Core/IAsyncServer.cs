using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Concurrent;
using SunSocket.Core.Session;
using SunSocket.Core.Protocol;

namespace SunSocket.Core
{
    public interface IAsyncServer
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 监听socket
        /// </summary>
        Socket ListenerSocket { get; set; }
        /// <summary>
        /// 在线列表
        /// </summary>
        ConcurrentDictionary<string, ITcpSession> OnlineList { get; set; }
        /// <summary>
        /// 开始接受请求
        /// </summary>
        /// <param name="acceptEventArgs">异步套接字操作</param>
        void StartAccept(SocketAsyncEventArgs acceptEventArgs);
        /// <summary>
        /// 接收命令
        /// </summary>
        /// <param name="session"></param>
        /// <param name="cmd"></param>
        void ReceiveCommond(ITcpSession session, ReceiveCommond cmd);
        /// <summary>
        /// 关闭会话
        /// </summary>
        /// <param name="sesseion"></param>
        void CloseSession(ITcpSession sesseion);
        /// <summary>
        /// 数据包提取完成事件
        /// </summary>
        event EventHandler<ReceiveCommond> OnReceived;
        /// <summary>
        /// 当连接请求通过后
        /// </summary>
        event EventHandler<ITcpSession> OnConnected;
        /// <summary>
        /// 断开连接通知
        /// </summary>
        event EventHandler<ITcpSession> OnDisConnect;
    }
}
