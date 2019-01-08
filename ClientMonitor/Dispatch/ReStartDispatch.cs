using ClientMonitor.Argu;
using ClientMonitor.Common.Enum;
using ClientMonitor.ExceptionMiddleware;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ClientMonitor.Dispatch
{
    public class ReStartDispatch
    {
        private readonly IConfiguration Configuration;

        public ReStartDispatch(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void StartDispatch(ArguReStart arguReStart)
        {
            string fileName = Configuration["RunProcessAdress"];
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                fileName += $"{arguReStart.app}.bat";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                fileName += $"{arguReStart.app}.sh";
            }
            else
            {
                fileName += $"{arguReStart.app}.sh";
            }
            //创建一个ProcessStartInfo对象 使用系统shell 指定命令和参数 设置标准输出
            var psi = new ProcessStartInfo(fileName);
            using (var proc = Process.Start(psi))
            {
                if (proc == null)
                {
                    proc.Kill();
                    throw new ClientException(EnumClientMonitor.进程启动失败, "Process start failed");
                }
            }
        }
    }
}
