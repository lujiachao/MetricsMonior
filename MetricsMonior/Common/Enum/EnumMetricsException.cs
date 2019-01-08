using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsMonior.Common.Enum
{
    public enum EnumMetricsException
    {
       请求参数不能为空 = 101,
       Http请求错误 = 102,
       验证IP地址白名单失败 = 103
    }
}
