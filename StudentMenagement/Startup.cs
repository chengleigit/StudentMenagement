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
using NetCore.AutoRegisterDi;
using StudentMenagement.Application;
using StudentMenagement.Application.Courses;
using StudentMenagement.Application.Students;
using StudentMenagement.Application.Teachers;
using StudentMenagement.CustomerMiddlewares;
using StudentMenagement.DataRepositories;
using StudentMenagement.Infrastructure;
using StudentMenagement.Infrastructure.Data;
using StudentMenagement.Infrastructure.Repositories;
using StudentMenagement.Models;
using StudentMenagement.Security;
using StudentMenagement.Security.CustomTokenProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMenagement
{
    public class Startup
    {
        private IWebHostEnvironment _env;
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //ע��HttpContextAccessor
            //services.AddHttpContextAccessor();

            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("StudentDBConnection"))
                );


            var builder = services.AddControllersWithViews(config =>
             {
                 //���ȫ�������֤
                 var policy = new AuthorizationPolicyBuilder()
                                                .RequireAuthenticatedUser()
                                                .Build();
                 config.Filters.Add(new AuthorizeFilter(policy));

                 var policy1 = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                 config.Filters.Add(new AuthorizeFilter(policy1));

             }).AddXmlSerializerFormatters();

            //Razor��ͼ��������ʱ����
            if (_env.IsDevelopment())
            {
                builder.AddRazorRuntimeCompilation();
            }


            #region ������Ȩ

            //����Identity����
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 1; //��С����
                options.Password.RequiredUniqueChars = 1; //����ظ��ַ�
                options.Password.RequireNonAlphanumeric = false; //������һ������ĸ���ݵ��ַ�
                options.Password.RequireLowercase = false;  //���������д��ĸ
                options.Password.RequireUppercase = false;  //�������Сд��ĸ

                options.SignIn.RequireConfirmedEmail = true; //��ֹδ��֤���û���¼

                //ͨ���Զ����CustomEmailConfirmation���������Ǿ���token���ƣ���//����AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser//>>("CustomEmailConfirmation")������һ��
                options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

                //��¼ʧ��5�ν�����15����
                options.Lockout.MaxFailedAccessAttempts = 5; 
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
              .AddErrorDescriber<CustomIdentityErrorDescriber>() //���ǵ�Ӣ�ĵĴ�����ʾ
              .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();
                    //.AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser>>("CustomEmailConfirmation");



            //�������Ƶ���Ч��Ϊ10S
            //services.Configure<DataProtectionTokenProviderOptions>(
            //  o =>o.TokenLifespan = TimeSpan.FromSeconds(10));


            services.AddAuthorization(options =>
            {
                //���Խ��������Ȩ
                options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role", "true"));

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

            //ע���Զ�����Ȩ�������
            //services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            //services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();

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

            services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = _configuration["Authentication:Microsoft:ClientId"];
                microsoftOptions.ClientSecret = _configuration["Authentication:Microsoft:ClientSecret"];
            }).AddGitHub(options =>
            {
                options.ClientId = _configuration["Authentication:GitHub:ClientId"];
                options.ClientSecret = _configuration["Authentication:GitHub:ClientSecret"];
            });

            #endregion

            //���MVC����
            services.AddMvc(a => a.EnableEndpointRouting = false);

            #region ����ע��
            //�Զ�ע���������ע������
            //services.RegisterAssemblyPublicNonGenericClasses()
            //    .Where(c => c.Name.EndsWith("Service"))
            //    .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);
            //.AsPublicImplementedInterfaces();

            //����ע�� ���� ������ ˲��
            //services.AddSingleton<IStudentRepository, MockStudentRepository>();

            //services.AddTransient<IStudentRepository, MockStudentRepository>();
            //services.AddScoped<ICourseRepository, SQLCourseRepository>();
            services.AddTransient(typeof(IRepository<,>), typeof(RepositoryBase<,>));
            services.AddSingleton<DataProtectionPurposeStrings>();
            services.AddScoped<IStudentRepository, SQLStudentRepository>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ITeacherService, TeacherService>();

            #endregion






        }


        //��Ȩ����
        private bool AuthorizeAccess(AuthorizationHandlerContext context)
        {
            return context.User.IsInRole("admin") &&
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

            //���ݳ�ʼ��
            app.UseDataInitializer();

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

