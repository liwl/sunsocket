using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace SunSocket.Core
{
    public interface IPool<T>
    {
        /// <summary>
        /// 弹出
        /// </summary>
        /// <returns></returns>
        T Pop();
        /// <summary>
        /// 压入
        /// </summary>
        /// <param name="item"></param>
        void Push(T item);
        /// <summary>
        /// 剩余空闲量
        /// </summary>
        int FreeCount { get; }
        /// <summary>
        ///总数量
        /// </summary>
        int Count { get; }
    }
}
