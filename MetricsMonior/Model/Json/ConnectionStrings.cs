using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsMonior.Model.Json
{
    /// <summary>
    /// 获取配置文件绑定信息
    /// </summary>
    public class ConnectionStrings : IOptions<ConnectionStrings>
    {
        public ConnectionStrings Value => this;

        public string influxUrl
        {
            get; set;
        }

        public string influxUser
        {
            get; set;
        }

        public string influxPwd
        {
            get; set;
        }

        public string AppMetricsTest
        {
            get; set;
        }
    }
}
