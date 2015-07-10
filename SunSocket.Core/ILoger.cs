using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunSocket.Core
{
    public interface ILoger
    {

        /// <summary>
        /// 记录日志信息
        /// </summary>
        /// <param name="info">日志信息对象</param>
        /// <returns></returns>
        void Log(string message);
        /// <summary>
        /// 普通信息
        /// </summary>
        /// <param name="message"></param>
        void Trace(string message);
        /// <summary>
        /// 普通异常信息
        /// </summary>
        /// <param name="exception"></param>
        void Trace(Exception exception);
        /// <summary>
        /// deBug日志
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        void Debug(string message);
        /// <summary>
        /// Bug日志
        /// </summary>
        /// <param name="e">异常对象</param>
        /// <returns></returns>
        void Debug(Exception exception);
        /// <summary>
        /// 消息日志
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <returns></returns>
        void Info(string message);
        /// <summary>
        /// 消息日志
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <returns></returns>
        void Info(Exception exception);
        /// <summary>
        /// 提醒日志
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        void Warning(string message);
        /// <summary>
        /// 提醒日志
        /// </summary>
        /// <param name="e">异常对象</param>
        /// <returns></returns>
        void Warning(Exception e);
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        void Error(string message);
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="e">异常对象</param>
        /// <returns></returns>
        void Error(Exception e);
        /// <summary>
        /// 严重错误日志
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        void Fatal(string message);
        /// <summary>
        /// 严重错误日志
        /// </summary>
        /// <param name="e">异常对象</param>
        /// <returns></returns>
        void Fatal(Exception e);
    }
}
