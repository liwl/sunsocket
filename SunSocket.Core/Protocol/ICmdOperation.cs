using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunSocket.Core.Protocol
{
    public interface ICmdOperation<T>
    {
        T Parse(byte[] buffer, int offset, int count);
    }
}
