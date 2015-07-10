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
    public interface IListener
    {
        /// <summary>
        /// 启动监听
        /// </summary>
        void Start();
        /// <summary>
        /// 停止监听
        /// </summary>
        void Stop();
        /// <summary>
        /// 获取异步服务(启动后可用)
        /// </summary>
        IAsyncServer AsyncServer { get; }
    }
}
