
using System;
using Autofac;

namespace SunSocket.Core.DI
{
    internal class RegisterFunc
    {
        public string AssemblyGuid
        {
            get;
            set;
        }
        public Action<ContainerBuilder> Func
        {
            get;
            set;
        }
        public bool IsRegister
        {
            get;
            set;
        }
    }
}
