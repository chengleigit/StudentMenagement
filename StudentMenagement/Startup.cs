using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StudentMenagement.CustomerMiddlewares;
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
     
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("StudentDBConnection"))
                );

            services.AddControllersWithViews(config =>
            {
                //添加全局身份验证
                var policy = new AuthorizationPolicyBuilder()
                                              .RequireAuthenticatedUser()
                                              .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }
           ).AddXmlSerializerFormatters();

            //配置Identity服务
            services.AddIdentity<ApplicationUser, IdentityRole>(options=> 
            {
                options.Password.RequiredLength = 1; //最小长度
                options.Password.RequiredUniqueChars = 1; //最大重复字符
                options.Password.RequireNonAlphanumeric = false; //至少有一个非字母数据的字符
                options.Password.RequireLowercase = false;  //必须包含大写字母
                options.Password.RequireUppercase = false;  //必须包含小写字母
            })
               .AddErrorDescriber<CustomIdentityErrorDescriber>() //覆盖掉英文的错误提示
               .AddEntityFrameworkStores<AppDbContext>(); ;

            //添加MVC服务
            services.AddMvc(a=>a.EnableEndpointRouting=false);

            //依赖注入 单例 作用域 瞬间
            //services.AddSingleton<IStudentRepository, MockStudentRepository>();
            services.AddScoped<IStudentRepository, SQLStudentRepository>();
            //services.AddTransient<IStudentRepository, MockStudentRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILogger<Startup> logger)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if(env.IsStaging()||env.IsProduction()||env.IsEnvironment("UAT"))
            {
                //app.UseStatusCodePages();
                //app.UseStatusCodePagesWithRedirects("/Error/{0}"); //302
                //app.UseStatusCodePagesWithReExecute("/Error/{0}");   //404

                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            //添加默认文件中间件
            //app.UseDefaultFiles();

            //添加静态文件中间件
            app.UseStaticFiles();

            //添加身份验证中间件
            app.UseAuthentication();

            //添加MVC默认路由
            app.UseMvcWithDefaultRoute();

            //添加授权中间件
            app.UseAuthorization();

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
