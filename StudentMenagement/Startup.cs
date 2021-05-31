using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StudentMenagement.DataRepositories;
using StudentMenagement.Infrastructure;
using StudentMenagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMenagement
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("StudentDBConnection"))
                ); 

            //添加MVC服务
            services.AddMvc(a=>a.EnableEndpointRouting=false);

            //依赖注入 单例 作用域 瞬间
            //services.AddSingleton<IStudentRepository, MockStudentRepository>();
            services.AddScoped<IStudentRepository, SQLStudentRepository>();
            //services.AddTransient<IStudentRepository, MockStudentRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILogger<Startup> logger)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //添加默认文件中间件
            //app.UseDefaultFiles();

            //添加静态文件中间件
            app.UseStaticFiles();

            //添加MVC默认路由
            //app.UseMvcWithDefaultRoute();

            app.UseMvc(routes=> 
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{Id?}");
            });

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello Word!");
            //});

            


        }
    }
}
