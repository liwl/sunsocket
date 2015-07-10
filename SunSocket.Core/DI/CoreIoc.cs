using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace SunSocket.Core.DI
{
    public class CoreIoc
    {
        static ContainerBuilder builder = new ContainerBuilder();
        static IContainer container;
        static List<Action<ContainerBuilder>> RegisterFuncList = new List<Action<ContainerBuilder>>();
        static object lockOjb = new object();
        public static void Register(Action<ContainerBuilder> func)
        {
            RegisterFuncList.Add(func);
        }
        /// <summary>
        /// 构建容器
        /// </summary>
        public static void Build()
        {
            lock (lockOjb)
            {
                if (null == container)
                {
                    foreach (var rf in RegisterFuncList)
                    {
                        rf(builder);
                    }
                    container = builder.Build();
                }
            }
        }
        /// <summary>
        /// 重新构建WebIoc
        /// </summary>
        public static void Rebuild()
        {
            lock (lockOjb)
            {
                var rebuilder = new ContainerBuilder();
                foreach (var rf in RegisterFuncList)
                {
                    rf(rebuilder);
                }
                builder = rebuilder;
                container = builder.Build();
            }
        }
        /// <summary>
        /// 获取container
        /// </summary>
        public static IContainer Container
        {
            get
            {
                return container;
            }
        }
    }
}
