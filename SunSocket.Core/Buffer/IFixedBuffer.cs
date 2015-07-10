using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunSocket.Core.Buffer
{
    public interface IFixedBuffer
    {

        /// <summary>
        /// 缓存数组
        /// </summary>
        byte[] Buffer { get; set; }
        /// <summary>
        /// 数据大小
        /// </summary>
        int DataSize { get; set; }
        /// <summary>
        /// 在缓冲器中写入数据
        /// </summary>
        /// <param name="buffer"></param>
        void WriteBuffer(byte[] buffer);
        /// <summary>
        /// 在缓冲器中写入数据
        /// </summary>
        /// <param name="buffer">数据</param>
        /// <param name="offset">跳过大小</param>
        /// <param name="count">数据大小</param>
        void WriteBuffer(byte[] buffer, int offset, int count);
        /// <summary>
        /// 在缓冲器中写入short数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="convert">是否转换为网络字节顺序</param>
        void WriteShort(short value, bool convert);
        /// <summary>
        /// 在缓冲器重写入int数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="convert">是否转换为网络字节顺序</param>
        void WriteInt(int value, bool convert);
        /// <summary>
        /// 在缓冲器中写入long数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="convert">是否转换为网络字节顺序</param>
        void WriteLong(long value, bool convert);
        /// <summary>
        /// 在缓冲期中写入字符数据
        /// </summary>
        /// <param name="value">是否转换为网络字节顺序</param>
        void WriteString(string value);
        /// <summary>
        /// 数据全部清理
        /// </summary>
        void Clear();
        /// <summary>
        /// 清理指定大小的数据
        /// </summary>
        /// <param name="count"></param>
        void Clear(int count);
    }
}
