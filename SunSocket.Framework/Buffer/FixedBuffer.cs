using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunSocket.Core.Buffer;

namespace SunSocket.Framework.Buffer
{
   public class FixedBuffer:IFixedBuffer
    {
        public FixedBuffer(int bufferSize)
        {
            DataSize = 0;
            Buffer = new byte[bufferSize];
        }
        /// <summary>
        /// 缓存数据数组
        /// </summary>
        public byte[] Buffer
        {
            get;
            set;
        }
        /// <summary>
        /// 写入缓存的数据长度
        /// </summary>
        public int DataSize
        {
            get;
            set;
        }
        private int GetReserveCount() //获得剩余的字节数
        {
            return Buffer.Length - DataSize;
        }
        public void WriteBuffer(byte[] buffer)
        {
            WriteBuffer(buffer, 0, buffer.Length);
        }

        public void WriteBuffer(byte[] buffer, int offset, int count)
        {
            if (GetReserveCount() >= count) //缓冲区空间够，不需要申请
            {
                System.Buffer.BlockCopy(buffer, offset, Buffer, DataSize, count);
                DataSize += count;
            }
            else 
            {
                throw new Exception("缓冲器空间不够");
            }
        }

        public void WriteInt(int value, bool convert)
        {
            if (convert)
            {
                value = System.Net.IPAddress.HostToNetworkOrder(value); //NET是小头结构，网络字节是大头结构，需要客户端和服务器约定好
            }
            byte[] tmpBuffer = BitConverter.GetBytes(value);
            WriteBuffer(tmpBuffer);
        }
        public void WriteShort(short value, bool convert)
        {
            if (convert)
            {
                value = System.Net.IPAddress.HostToNetworkOrder(value); //NET是小头结构，网络字节是大头结构，需要客户端和服务器约定好
            }
            byte[] tmpBuffer = BitConverter.GetBytes(value);
            WriteBuffer(tmpBuffer);
        }
        public void WriteLong(long value, bool convert)
        {
            if (convert)
            {
                value = System.Net.IPAddress.HostToNetworkOrder(value); //NET是小头结构，网络字节是大头结构，需要客户端和服务器约定好
            }
            byte[] tmpBuffer = BitConverter.GetBytes(value);
            WriteBuffer(tmpBuffer);
        }

        public void WriteString(string value)
        {
            byte[] tmpBuffer = Encoding.UTF8.GetBytes(value);
            WriteBuffer(tmpBuffer);
        }
        public void Clear()
        {
            DataSize = 0;
        }

        public void Clear(int count) //清理指定大小的数据
        {
            if (count == 0)
                return;
            if (count >= DataSize) //如果需要清理的数据大于现有数据大小，则全部清理
            {
                DataSize = 0;
            }
            else
            {
                for (int i = 0; i < DataSize - count; i++) //否则后面的数据往前移
                {
                    Buffer[i] = Buffer[count + i];
                }
                DataSize = DataSize - count;
            }
        }
    }
}
