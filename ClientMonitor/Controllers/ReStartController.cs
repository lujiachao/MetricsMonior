using ClientMonitor.Argu;
using ClientMonitor.Common.Enum;
using ClientMonitor.Dispatch;
using ClientMonitor.ExceptionMiddleware;
using ClientMonitor.Result;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ClientMonitor.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("AllowCors")]
    [ApiController]
    public class ReStartController : ControllerBase
    {
        private readonly IConfiguration Configuration;

        public ReStartController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpPost]
        public ActionResult<ResultStatus> ReStartProcess([FromBody]ArguReStart arguReStart)
        {
            if (arguReStart == null)
            {
                throw new ClientException(EnumClientMonitor.请求参数不能为空, "Arguments is null");
            }
            if (arguReStart.value != "0")
            {
                throw new ClientException(EnumClientMonitor.程序运行状态正常, "Process start success");
            }
            ReStartDispatch reStartDispatch = new ReStartDispatch(Configuration);
            reStartDispatch.StartDispatch(arguReStart);
            return new ResultStatus((int)ResultStatusCode.Success,"正常启动程序");
        }
    }
}