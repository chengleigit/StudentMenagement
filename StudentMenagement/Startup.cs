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
                //���ȫ�������֤
                var policy = new AuthorizationPolicyBuilder()
                                              .RequireAuthenticatedUser()
                                              .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }
           ).AddXmlSerializerFormatters();

            //����Identity����
            services.AddIdentity<ApplicationUser, IdentityRole>(options=> 
            {
                options.Password.RequiredLength = 1; //��С����
                options.Password.RequiredUniqueChars = 1; //����ظ��ַ�
                options.Password.RequireNonAlphanumeric = false; //������һ������ĸ���ݵ��ַ�
                options.Password.RequireLowercase = false;  //���������д��ĸ
                options.Password.RequireUppercase = false;  //�������Сд��ĸ
            })
               .AddErrorDescriber<CustomIdentityErrorDescriber>() //���ǵ�Ӣ�ĵĴ�����ʾ
               .AddEntityFrameworkStores<AppDbContext>(); ;

            //���MVC����
            services.AddMvc(a=>a.EnableEndpointRouting=false);

            //����ע�� ���� ������ ˲��
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

            //���Ĭ���ļ��м��
            //app.UseDefaultFiles();

            //��Ӿ�̬�ļ��м��
            app.UseStaticFiles();

            //��������֤�м��
            app.UseAuthentication();

            //���MVCĬ��·��
            app.UseMvcWithDefaultRoute();

            //�����Ȩ�м��
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
