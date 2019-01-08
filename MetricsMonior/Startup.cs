using MetricsMonior.ExceptionMiddleware;
using MetricsMonior.Model.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;

namespace MetricsMonior
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.AddHttpClient("ReStartProcess", x => { });
            #region Swagger
            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v0.1.0",
                    Title = "MetricsMonitor API",
                    Description = "dotnet core 监控系统框架说明文档",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "MetricsMonitor.Core", Email = "632849093@qq.com", Url = "https://www.baidu.com" }                    
                })
                ;
                //就是这里
                var xmlPath = Path.Combine(basePath, "MetricsMonitor.xml");  //这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlPath, true); //默认第二个参数是false,这个是controller的注释，记得修改
            });
            #endregion
            services.AddCors(_options => _options.AddPolicy("AllowCors", _builder => _builder.AllowAnyOrigin().AllowAnyMethod()));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                #region Swagger
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                });
                #endregion
                app.UseMiddleware(typeof(ExceptionHandlerMiddleWare));
            }
            else
            {
                app.UseHsts();
                #region Swagger
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                });
                #endregion
                app.UseMiddleware(typeof(ExceptionHandlerMiddleWare));
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
