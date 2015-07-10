using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SunSocket.Core;
using SunSocket.Core.Buffer;
using SunSocket.Core.Protocol;
using SunSocket.Core.Session;
using SunSocket.Framework.Buffer;

namespace SunSocket.Framework.Protocol
{
    public class TcpPacketProtocol : ITcpPacketProtocol
    {
        object closeLock = new object();
        bool NetByteOrder = false;
        static int intByteLength = sizeof(int), shortByteLength = sizeof(short);
        static int checkandCmdLength = intByteLength + shortByteLength;
        private object clearLock = new object();
        ILoger loger;
        //缓冲器池
        private static FixedBufferPool BufferPool;
        private int alreadyReceivePacketLength, needReceivePacketLenght;
        private IFixedBuffer InterimPacketBuffer;
        //数据接收缓冲器队列
        private Queue<IFixedBuffer> ReceiveBuffers;
        //数据发送缓冲器
        public IFixedBuffer SendBuffer { get; set; }

        public ITcpSession Session
        {
            get;
            set;
        }

        private SendCommond NoComplateCmd = null;//未完全发送指令
        int isSend = 0;//发送状态
        private Queue<SendCommond> cmdQueue = new Queue<SendCommond>();//指令发送队列
        public TcpPacketProtocol(int bufferSize,int bufferPoolSize,ILoger loger)
        {
            this.loger = loger;
            if(BufferPool==null)
                BufferPool = new FixedBufferPool(bufferPoolSize);
            ReceiveBuffers = new Queue<IFixedBuffer>();
            SendBuffer = new FixedBuffer(bufferPoolSize);
        }
        private bool ProcessReceiveBuffer(byte[] receiveBuffer, int offset, int count)
        {
            while (count > 0)
            {
                if (needReceivePacketLenght > 0 && alreadyReceivePacketLength + count >= needReceivePacketLenght)//说明包已获取完成
                {
                    if (InterimPacketBuffer != null)
                    {
                        ReceiveBuffers.Enqueue(InterimPacketBuffer);
                        InterimPacketBuffer = null;
                    }
                    if (ReceiveBuffers.Count > 0)
                    {
                        int getLenght = 0;//已取出数据

                        var cacheBuffer = ReceiveBuffers.Dequeue();
                        if (cacheBuffer.DataSize > checkandCmdLength)
                        {
                            int cachePacketLength = BitConverter.ToInt32(cacheBuffer.Buffer, 0); //获取包长度
                            short commondId = BitConverter.ToInt16(cacheBuffer.Buffer, intByteLength);
                            var data = new byte[cachePacketLength - shortByteLength];
                            getLenght = cacheBuffer.DataSize - checkandCmdLength;
                            System.Buffer.BlockCopy(cacheBuffer.Buffer, checkandCmdLength, data, 0, getLenght);
                            cacheBuffer.Clear();//清理数据并装入池中
                            BufferPool.Push(cacheBuffer);
                            while (ReceiveBuffers.Count > 0)
                            {
                                var popBuffer = ReceiveBuffers.Dequeue();
                                System.Buffer.BlockCopy(popBuffer.Buffer, 0, data, getLenght, popBuffer.DataSize);
                                getLenght += popBuffer.DataSize;
                                popBuffer.Clear();//清理数据并装入池中
                                BufferPool.Push(popBuffer);
                            }
                            var needLenght = needReceivePacketLenght - getLenght - checkandCmdLength;
                            System.Buffer.BlockCopy(receiveBuffer, offset, data, getLenght, needLenght);
                            offset += needLenght;
                            count -= needLenght;
                            //触发获取指令事件
                            Session.Server.ReceiveCommond(Session, new ReceiveCommond() { CommondId = commondId, Data = data });
                            //清理合包数据
                            needReceivePacketLenght = 0; alreadyReceivePacketLength = 0;
                        }
                        else
                        {
                            InterimPacketBuffer = cacheBuffer;
                        }
                    }
                }
                //按照长度分包
                int packetLength = BitConverter.ToInt32(receiveBuffer, offset); //获取包长度
                if (NetByteOrder)
                    packetLength = IPAddress.NetworkToHostOrder(packetLength); //把网络字节顺序转为本地字节顺序
                if ((count - intByteLength) >= packetLength) //如果数据包达到长度则马上进行解析
                {
                    short commondId = BitConverter.ToInt16(receiveBuffer, offset + intByteLength); //获取指令编号
                    int dataLength = packetLength - shortByteLength;
                    var data = new byte[dataLength];
                    System.Buffer.BlockCopy(receiveBuffer, offset + checkandCmdLength, data, 0, dataLength);
                    //触发获取指令事件
                    Session.Server.ReceiveCommond(Session, new ReceiveCommond() { CommondId = commondId, Data = data });
                    int processLenght = packetLength + intByteLength;
                    offset += processLenght;
                    count -= processLenght;
                }
                else
                {//存入分包缓存池
                    needReceivePacketLenght = packetLength + intByteLength;//记录当前包总共需要多少的数据
                    while (count > 0)//遍历把数据放入缓冲器中
                    {
                        if (InterimPacketBuffer == null)
                        {
                            InterimPacketBuffer = BufferPool.Pop();
                        }
                        var surpos = InterimPacketBuffer.Buffer.Length - InterimPacketBuffer.DataSize;//中间buffer剩余空间
                        if (count > surpos)
                        {
                            InterimPacketBuffer.WriteBuffer(receiveBuffer, offset, surpos);
                            ReceiveBuffers.Enqueue(InterimPacketBuffer);
                            InterimPacketBuffer = null;
                            alreadyReceivePacketLength += surpos;//记录已接收的数据
                            offset += surpos;
                            count -= surpos;
                        }
                        else
                        {
                            InterimPacketBuffer.WriteBuffer(receiveBuffer, offset, count);
                            alreadyReceivePacketLength += count;//记录已接收的数据
                            offset =0;
                            count = 0;
                        }
                    }
                }
            }
           
            return count == 0;
        }

        public bool SendAsync(SendCommond cmd)
        {
            cmdQueue.Enqueue(cmd);
            if (Interlocked.Increment(ref isSend) == 1)
                Task.Run(() =>
                {
                    SendProcess();
                });
            else
            {
                Interlocked.Decrement(ref isSend);
            }
            return true;
        }

        private void SendProcess()
        {
            int surplus = SendBuffer.Buffer.Length;
            while (cmdQueue.Count > 0)
            {
                if (NoComplateCmd != null)
                {
                    int noComplateLength = NoComplateCmd.Buffer.Length - NoComplateCmd.Offset;
                    if (noComplateLength <= SendBuffer.Buffer.Length)
                    {
                        SendBuffer.WriteBuffer(NoComplateCmd.Buffer, NoComplateCmd.Offset, noComplateLength);
                        surplus -= noComplateLength;
                        NoComplateCmd = null;
                    }
                    else
                    {
                        SendBuffer.WriteBuffer(NoComplateCmd.Buffer, NoComplateCmd.Offset, SendBuffer.Buffer.Length);
                        NoComplateCmd.Offset += SendBuffer.Buffer.Length;
                        surplus -= SendBuffer.Buffer.Length;
                        break;//跳出当前循环
                    }
                }
                if (surplus >= intByteLength)
                {
                    var cmd = cmdQueue.Dequeue();
                    var cmdAllLength = cmd.Buffer.Length + checkandCmdLength;
                    if (cmdAllLength <= surplus)
                    {
                        SendBuffer.WriteInt(cmd.Buffer.Length + shortByteLength, false); //写入总大小
                        SendBuffer.WriteShort(cmd.CommondId, false); //写入命令编号
                        SendBuffer.WriteBuffer(cmd.Buffer); //写入命令内容
                        surplus -= cmdAllLength;
                    }
                    else
                    {
                        SendBuffer.WriteInt(cmd.Buffer.Length, false); //写入总大小

                        surplus -= cmd.Buffer.Length;

                        if (surplus >= shortByteLength)
                        {
                            SendBuffer.WriteShort(cmd.CommondId, false); //写入命令编号
                            surplus -= shortByteLength;
                        }
                        if (surplus > 0)
                        {
                            SendBuffer.WriteBuffer(cmd.Buffer, cmd.Offset, surplus); //写入命令内容
                            cmd.Offset = surplus;
                        }
                        NoComplateCmd = cmd;//把未全部发送指令缓存
                    }
                }
                else
                {
                    break;
                }
            }
            if (surplus < SendBuffer.Buffer.Length)
            {
                Session.SendEventArgs.SetBuffer(SendBuffer.Buffer, 0, SendBuffer.DataSize);
                bool willRaiseEvent = Session.ConnectSocket.SendAsync(Session.SendEventArgs);
                if (!willRaiseEvent)
                {
                    SendComplate(null,Session.SendEventArgs);
                }
            }
            else
            {
                Interlocked.Decrement(ref isSend);
            }
        }
        public void SendComplate(object sender, SocketAsyncEventArgs sendEventArgs)
        {
            Session.ActiveDateTime = DateTime.Now;//发送数据视为活跃
            if (sendEventArgs.SocketError == SocketError.Success)
            {
                SendBuffer.Clear(); //清除已发送的包
                SendProcess();//继续发送
            }
            else
            {
                lock(closeLock)
                {
                    if (Session.Server != null)
                        Session.Server.CloseSession(Session);
                }
            }
        }

        public void ReceiveComplate(object sender, SocketAsyncEventArgs receiveEventArgs)
        {
            Session.ActiveDateTime = DateTime.Now;
            if (receiveEventArgs.SocketError == SocketError.Success)
            {
                if (receiveEventArgs.BytesTransferred > 0) //处理接收数据
                {
                    if (!ProcessReceiveBuffer(receiveEventArgs.Buffer, receiveEventArgs.Offset, receiveEventArgs.BytesTransferred))
                    { //如果处理数据返回失败，则断开连接
                        lock (closeLock)
                        {
                            if (Session.Server != null)
                                Session.Server.CloseSession(Session);
                        }
                    }
                }
                Session.StartReceiveAsync();//再次等待接收数据
            }
            else
            {
                lock (closeLock)
                {
                    if (Session.Server != null)
                        Session.Server.CloseSession(Session);//断开连接
                }
            }
        }
        public void Clear()
        {
            SendBuffer.Clear();
            lock (clearLock)
            {
                if (InterimPacketBuffer != null)
                {
                    InterimPacketBuffer.Clear();
                    BufferPool.Push(InterimPacketBuffer);
                    InterimPacketBuffer = null;
                }
                while (ReceiveBuffers.Count > 0)
                {
                    var packetBuffer = ReceiveBuffers.Dequeue();
                    packetBuffer.Clear();
                    BufferPool.Push(packetBuffer);
                }
            }
            NoComplateCmd = null;
            alreadyReceivePacketLength = 0;
            needReceivePacketLenght = 0;
        }
    }
}
