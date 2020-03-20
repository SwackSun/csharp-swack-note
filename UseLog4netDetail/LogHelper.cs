using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UseLog4netDetail
{
    class LogHelper
    {
        //创建日志记录组件实例
        static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void WriteFatal(Exception ex)
        {
            log.Fatal("Fatal", ex);
        }

        /// <summary>
        /// 输出异常信息
        /// </summary>
        /// <param name="t"></param>
        /// <param name="e"></param>
        public static void WriteLog(Type t, Exception ex)
        {
            using (log4net.NDC.Push(t.ToString()))
            {
                log.Error("Application Error", ex);
            }
        }

        /// <summary>
        /// 输出普通错误信息
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteLog(Exception ex)
        {
            log.Error("Error", ex);
        }

        /// <summary>
        /// 输出DEBUG信息
        /// </summary>
        /// <param name="text"></param>
        public static void WriteDebug(object text)
        {
            log.Debug(text);
        }

        /// <summary>
        /// 输出程序运行信息
        /// </summary>
        /// <param name="text"></param>
        public static void WriteInfo(string text)
        {
            log.Info(text);
        }
    }
}
