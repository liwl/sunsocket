using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunSocket.Core.Protocol
{
    public class ReceiveCommond
    {
        public short CommondId { get; set; }
        public byte[] Data { get; set; }
    }
}
