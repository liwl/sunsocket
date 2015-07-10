using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using SunSocket.Core.Protocol;

namespace SunSocket.Core.Session
{
    public interface ITcpSessionPool
    {
        /// <summary>
        /// 压入Session
        /// </summary>
        /// <param name="item"></param>
        void Push(ITcpSession item);
        /// <summary>
        /// 弹出Session
        /// </summary>
        /// <returns></returns>
        ITcpSession Pop();
        /// <summary>
        /// 剩余空闲Session
        /// </summary>
        int FreeCount { get; }
        /// <summary>
        /// pool数量
        /// </summary>
        int Count { get; }
    }
}
