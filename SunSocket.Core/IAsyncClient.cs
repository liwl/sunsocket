using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SunSocket.Core.Session;
using SunSocket.Core.Protocol;

namespace SunSocket.Core
{
    public interface IAsyncClient
    {
        /// <summary>
        /// 启用连接
        /// </summary>
        void Connect(EndPoint remoteEndPoint);
        /// <summary>
        /// 关闭会话
        /// </summary>
        /// <param name="sesseion"></param>
        void Disconnect(ITcpClientSession sesseion);
        /// <summary>
        /// 数据包提取完成事件
        /// </summary>
        event EventHandler<ReceiveCommond> OnReceived;
        /// <summary>
        /// 连接服务器成功
        /// </summary>
        event EventHandler<ITcpClientSession> OnConnected;
        /// <summary>
        /// 断开连接时间
        /// </summary>
        event EventHandler<ITcpClientSession> OnDisConnect;
    }
}
