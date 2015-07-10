using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using System.Net.Sockets;
using SunSocket.Core.Session;
using SunSocket.Framework.Protocol;
using SunSocket.Core.DI;
using SunSocket.Core;
using SunSocket.Core.Protocol;

namespace SunSocket.Framework.Session
{
    public class TcpSessionPool : ITcpSessionPool
    {
        private static ConcurrentStack<ITcpSession> pool=new ConcurrentStack<ITcpSession>();
        private int count = 0, bufferPoolSize, bufferSize, maxSessions;
        ILoger loger;
        public TcpSessionPool(int bufferPoolSize,int bufferSize,int maxSessions,ILoger loger)
        {
            this.bufferPoolSize = bufferPoolSize;
            this.bufferSize = bufferSize;
            this.maxSessions = maxSessions;
            this.loger = loger;
        } 

        public int Count
        {
            get
            {
                return count;
            }
        }

        public int FreeCount
        {
            get
            {
                return pool.Count;
            }
        }

        public ITcpSession Pop()
        {
            ITcpSession session;
            if (!pool.TryPop(out session))
            {
                if(count <= maxSessions)
                {
                    Interlocked.Increment(ref count);
                    session = new TcpSession(loger);
                    session.PacketProtocol = new TcpPacketProtocol(bufferSize,bufferPoolSize,loger);
                    session.SendEventArgs.Completed += session.PacketProtocol.SendComplate;//数据发送完成事件

                    session.ReceiveEventArgs.SetBuffer(new byte[bufferSize], 0, bufferSize);
                    session.ReceiveEventArgs.Completed += session.PacketProtocol.ReceiveComplate;
                   
                }
            } 
            return session;
        }

        public void Push(ITcpSession item)
        {
            pool.Push(item);
        }
    }
}
