using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using SunSocket.Core;
using System.Net.Sockets;

namespace SunSocket.Framework
{
    public class EventArgsPool : IPool<SocketAsyncEventArgs>
    {
        private ConcurrentStack<SocketAsyncEventArgs> pool = new ConcurrentStack<SocketAsyncEventArgs>();
        int maxCount, bufferSize,allCount=0;
        EventHandler<SocketAsyncEventArgs> completedCallBack;
        public EventArgsPool(int maxCount,int bufferSize,EventHandler<SocketAsyncEventArgs> completedCallBack)
        {
            this.maxCount = maxCount;
            this.bufferSize = bufferSize;
            this.completedCallBack = completedCallBack;
        }
        public int Count
        {
            get
            {
                return allCount;
            }
        }

        public int FreeCount
        {
            get
            {
                return pool.Count();
            }
        }

        public SocketAsyncEventArgs Pop()
        {
            SocketAsyncEventArgs result;
            if (!pool.TryPop(out result))
            {
                if (Interlocked.Increment(ref allCount) <= maxCount)
                {
                    result = new SocketAsyncEventArgs();
                    result.SetBuffer(new byte[bufferSize], 0, bufferSize);
                    result.Completed += completedCallBack;
                }
            }
            return result;
        }

        public void Push(SocketAsyncEventArgs item)
        {
            pool.Push(item);
        }
    }
}
