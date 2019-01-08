using InfluxData.Net.InfluxDb;
using MetricsMonior.Argu;
using MetricsMonior.Common.Enum;
using MetricsMonior.ExceptionMiddleware;
using MetricsMonior.Model;
using MetricsMonior.Model.Json;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MetricsMonior.Dispatch
{
    public class MoniorDispatch
    {
        private InfluxDbClient ClientDb;

        private IHttpClientFactory ClientFactory;

        private static string UriAddress = "api/ReStart/ReStartProcess";

        public MoniorDispatch(IOptions<ConnectionStrings> connectionString,IHttpClientFactory clientFactory)
        {
            ClientDb = new InfluxDbClient(connectionString.Value.influxUrl, connectionString.Value.influxUser, connectionString.Value.influxPwd, InfluxData.Net.Common.Enums.InfluxDbVersion.Latest);
            ClientFactory = clientFactory;
        }

        public async Task<string> MonitorProcess(ArguProcess arguProcess,string appHost)
        {
            Uri requestUri = new Uri(appHost + UriAddress);
            var queries = new[]
            {
                $"SELECT last(value) FROM \"application.health__results\" WHERE env = '{arguProcess.env}' and app = '{arguProcess.app}' and health_check_name = '百度 ping' and time > now() - 5s"
             };
            var dbName = "MyMetrics";

            //从指定库中查询数据
            var response = await ClientDb.Client.QueryAsync(queries, dbName);
            //得到Series集合对象
            var series = response.ToList();
            if (series.Count == 0)
            {
                ArguReStart arguReStart = new ArguReStart()
                {
                    app = arguProcess.app,
                    value = "0"
                };
                var responsePost = await PostStringAsync("ReStartProcess", ToJSON(arguReStart), requestUri, null);
                return arguReStart.value;
            }
            var healthCheckValue = (series[0].Values)[0][1];
            var healthCheckToString = Convert.ToString(healthCheckValue);
            if (healthCheckToString == "0")
            {
                //判断条件还不够精确，无法去重启
                //var responsePost = await PostStringAsync("ReStartProcess", null, requestUri, null);
            }
            else if (healthCheckToString == "0.5")
            {
                //程序运行异常
            }
            else
            {
                //程序正常运行
            }
            return healthCheckToString;
        }

        public async Task<HttpResponseMessage> PostStringAsync(string clientName,string data, Uri requestUri, Dictionary<string, string> headers, string contentType = "application/json")
        {
            var client = ClientFactory.CreateClient(clientName);
            var content = new StringContent(data);
            content.Headers.Clear();
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            }
            content.Headers.Add("charset", "utf-8");
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    content.Headers.Add(item.Key, item.Value);
                }
            }
            var requestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = content
            };
            requestMessage.RequestUri = requestUri;
            var responseMessage = await client.SendAsync(requestMessage);
            if (responseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new MetricsException(EnumMetricsException.Http请求错误, $"Post to {requestMessage.RequestUri} error,Status code:{responseMessage.StatusCode}");
            }
            return responseMessage;
        }

        /// <summary>
        /// 把对象转换为JSON字符串
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>JSON字符串</returns>
        public static string ToJSON(object o)
        {
            if (o == null)
            {
                return null;
            }
            return JsonConvert.SerializeObject(o);
        }
        /// <summary>
        /// 把Json文本转为实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T FromJSON<T>(string input)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(input);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
    }
}
