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
using StudentMenagement.Security;
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
            //ע��HttpContextAccessor
            services.AddHttpContextAccessor();

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
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 1; //��С����
                options.Password.RequiredUniqueChars = 1; //����ظ��ַ�
                options.Password.RequireNonAlphanumeric = false; //������һ������ĸ���ݵ��ַ�
                options.Password.RequireLowercase = false;  //���������д��ĸ
                options.Password.RequireUppercase = false;  //�������Сд��ĸ
            })
               .AddErrorDescriber<CustomIdentityErrorDescriber>() //���ǵ�Ӣ�ĵĴ�����ʾ
               .AddEntityFrameworkStores<AppDbContext>(); ;


            services.AddAuthorization(options =>
            {
                //���Խ��������Ȩ
                options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role","true"));

                //���Խ�Ͻ�ɫ��Ȩ
                options.AddPolicy("SuperAdminPolicy", policy => policy.RequireRole("admin"));

                //���Խ�϶����ɫ������Ȩ
                // options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("admin","User"));

                options.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Edit Role", "true"));

                /*����ӵ��admin��ɫ,����Edit Role ֵΪtrue
                 * Super Admin��ɫҲ���Խ��б༭
                 */
                //options.AddPolicy("EditRolePolicy", policy =>
                //     policy.RequireAssertion(context => AuthorizeAccess(context)));

                /*
                 * 
                 */
                //options.AddPolicy("EditRolePolicy", policy =>
                //     policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

                //options.InvokeHandlersAfterFailure = false;
            });

            //���MVC����
            services.AddMvc(a => a.EnableEndpointRouting = false);

            //����ע�� ���� ������ ˲��
            //services.AddSingleton<IStudentRepository, MockStudentRepository>();
            services.AddScoped<IStudentRepository, SQLStudentRepository>();
            //services.AddTransient<IStudentRepository, MockStudentRepository>();

            //ע���Զ�����Ȩ�������
            services.AddSingleton<IAuthorizationHandler,CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler,SuperAdminHandler>();

            services.ConfigureApplicationCookie(options =>
            {
                //�޸ľܾ����ʵ�·�ɵ�ַ
                options.AccessDeniedPath = new PathString("/Admin/AccessDenied");
                //�޸ĵ�¼��ַ��·��
                //   options.LoginPath = new PathString("/Admin/Login");  
                //�޸�ע����ַ��·��
                //   options.LogoutPath = new PathString("/Admin/LogOut");
                //ͳһϵͳȫ�ֵ�Cookie����
                options.Cookie.Name = "StudentCookieName";
                // ��¼�û�Cookie����Ч�� 
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                //�Ƿ��Cookie���û�������ʱ�䡣
                options.SlidingExpiration = true;
            });


        }


        //��Ȩ����
        private bool AuthorizeAccess(AuthorizationHandlerContext context)
        {
            return  context.User.IsInRole("admin") &&
                    context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
                    context.User.IsInRole("Super Admin");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsStaging() || env.IsProduction() || env.IsEnvironment("UAT"))
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

            app.UseMvc(routes =>
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
