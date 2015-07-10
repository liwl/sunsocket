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
using Autofac;
using SunSocket.Core.Protocol;

namespace SunSocket.Framework.Session
{
    public class TcpSessionPool : ITcpSessionPool
    {
        private static ConcurrentStack<ITcpSession> pool=new ConcurrentStack<ITcpSession>();
        private ConfigInfo config;
        private int count = 0;
        public TcpSessionPool(ConfigInfo config)
        {
            this.config = config;
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
                if(count <= config.MaxConnections)
                {
                    Interlocked.Increment(ref count);
                    session = new TcpSession();
                    session.PacketProtocol = new TcpPacketProtocol(config.BufferSize,config.MaxBufferPoolSize);
                    session.SendEventArgs.Completed += session.PacketProtocol.SendComplate;//数据发送完成事件

                    session.ReceiveEventArgs.SetBuffer(new byte[config.BufferSize], 0, config.BufferSize);
                    session.ReceiveEventArgs.Completed += session.PacketProtocol.ReceiveComplate;
                   
                }
            } 
            return session;
        }

        public void Push(ITcpSession item)
        {
            if (item == null)
                throw new Exception("item cannot be null");
            else
                pool.Push(item);
        }
    }
}
