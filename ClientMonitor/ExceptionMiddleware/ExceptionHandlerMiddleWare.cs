using ClientMonior.Result;
using ClientMonitor.Common.Enum;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ClientMonitor.ExceptionMiddleware
{
    public class ExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate next;

        /// <summary>
        /// 重构方法
        /// </summary>
        /// <param name="next"></param>
        public ExceptionHandlerMiddleWare(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception == null) return;
            await WriteExceptionAsync(context, exception).ConfigureAwait(false);
        }

        private static async Task WriteExceptionAsync(HttpContext context, Exception exception)
        {
            //返回友好的提示
            var response = context.Response;
            response.ContentType = "application/json";

            if (exception is ClientException)
            {
                await response.WriteAsync(JsonConvert.SerializeObject(new ZeusResultData()
                {
                    Code = ((ClientException)exception).Code,
                    Message = exception.Message,
                })).ConfigureAwait(false);
            }
            else if (exception is Exception)
            {
                await response.WriteAsync(JsonConvert.SerializeObject(new ZeusResultData()
                {
                    Code = (int)ResultStatusCode.UnknowError,
                    Message = exception.Message,
                })).ConfigureAwait(false);
            }
        }
    }
}
