using MetricsMonior.Argu;
using MetricsMonior.Common.Enum;
using MetricsMonior.Dispatch;
using MetricsMonior.ExceptionMiddleware;
using MetricsMonior.Model.Json;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace MetricsMonior.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("AllowCors")]
    [ApiController]
    public class MonitorController : ControllerBase
    {
        private IOptions<ConnectionStrings> ConnectionStrings;

        private IConfiguration Configuration;

        private IHttpClientFactory ClientFactory;
        public MonitorController(IOptions<ConnectionStrings> connectionStrings, IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            ConnectionStrings = connectionStrings;
            Configuration = configuration;
            ClientFactory = clientFactory;
        }
        /// <summary>
        /// 监控程序是否正常运行
        /// </summary>
        /// <param name="arguProcess"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> MonitorProcess([FromBody]ArguProcess arguProcess)
        {
            if (arguProcess == null)
            {
                throw new MetricsException(EnumMetricsException.请求参数不能为空,"Arguments is null");
            }
            string appHost = Configuration[$"{arguProcess.app}"];
            MoniorDispatch moniorDispatch = new MoniorDispatch(ConnectionStrings, ClientFactory);
            return await moniorDispatch.MonitorProcess(arguProcess,appHost);
        }
    }
}
