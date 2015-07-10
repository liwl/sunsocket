using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunSocket.Framework.Config
{
    public class ClientConfig
    {
        /// <summary>
        /// 缓冲器数组大小
        /// </summary>
        public int BufferSize
        {
            get;
            set;
        }
        int socketTimeOut = 60 * 1000; //Socket超时设置为60秒
        /// <summary>
        /// 连接超时时间
        /// </summary>
        public int SocketTimeOut { get { return socketTimeOut; } set { socketTimeOut = value; } }
        int maxBufferPoolSize = 4 * 1024;
        /// <summary>
        /// buffer池最大量(一般用于合并分包)
        /// </summary>
        public int MaxBufferPoolSize { get { return maxBufferPoolSize; } set { maxBufferPoolSize = value; } }
    }
}
