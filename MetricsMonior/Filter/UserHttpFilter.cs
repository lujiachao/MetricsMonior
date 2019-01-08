using MetricsMonior.Common.Enum;
using MetricsMonior.ExceptionMiddleware;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsMonior.Filter
{
    public class UserHttpFilter : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
            if (ipAddress == "::1")
            {
                ipAddress = "127.0.0.1";
            }

            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                throw new MetricsException(EnumMetricsException.验证IP地址白名单失败,"REQUEST IPADRESS NOT ALLOWED");
            }

            await next();
        }
    }
}
