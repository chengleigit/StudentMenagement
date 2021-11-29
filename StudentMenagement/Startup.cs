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
            //注入HttpContextAccessor
            //services.AddHttpContextAccessor();

            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("StudentDBConnection"))
                );


            var builder = services.AddControllersWithViews(config =>
             {
                 //添加全局身份验证
                 var policy = new AuthorizationPolicyBuilder()
                                                .RequireAuthenticatedUser()
                                                .Build();
                 config.Filters.Add(new AuthorizeFilter(policy));

                 var policy1 = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                 config.Filters.Add(new AuthorizeFilter(policy1));

             }).AddXmlSerializerFormatters();

            //Razor视图条件运行时编译
            if (_env.IsDevelopment())
            {
                builder.AddRazorRuntimeCompilation();
            }


            #region 策略授权

            //配置Identity服务
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 1; //最小长度
                options.Password.RequiredUniqueChars = 1; //最大重复字符
                options.Password.RequireNonAlphanumeric = false; //至少有一个非字母数据的字符
                options.Password.RequireLowercase = false;  //必须包含大写字母
                options.Password.RequireUppercase = false;  //必须包含小写字母

                options.SignIn.RequireConfirmedEmail = true; //阻止未验证的用户登录

                //通过自定义的CustomEmailConfirmation名称来覆盖旧有token名称，是//它与AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser//>>("CustomEmailConfirmation")关联在一起
                options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

                //登录失败5次将锁定15分钟
                options.Lockout.MaxFailedAccessAttempts = 5; 
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
              .AddErrorDescriber<CustomIdentityErrorDescriber>() //覆盖掉英文的错误提示
              .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();
                    //.AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser>>("CustomEmailConfirmation");



            //设置令牌的有效期为10S
            //services.Configure<DataProtectionTokenProviderOptions>(
            //  o =>o.TokenLifespan = TimeSpan.FromSeconds(10));


            services.AddAuthorization(options =>
            {
                //策略结合声明授权
                options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role", "true"));

                //策略结合角色授权
                options.AddPolicy("SuperAdminPolicy", policy => policy.RequireRole("admin"));

                //策略结合多个角色进行授权
                // options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("admin","User"));

                options.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Edit Role", "true"));

                /*必须拥有admin角色,包含Edit Role 值为true
                 * Super Admin角色也可以进行编辑
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

            //注入自定义授权处理程序
            //services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            //services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();

            services.ConfigureApplicationCookie(options =>
            {
                //修改拒绝访问的路由地址
                options.AccessDeniedPath = new PathString("/Admin/AccessDenied");
                //修改登录地址的路由
                //   options.LoginPath = new PathString("/Admin/Login");  
                //修改注销地址的路由
                //   options.LogoutPath = new PathString("/Admin/LogOut");
                //统一系统全局的Cookie名称
                options.Cookie.Name = "StudentCookieName";
                // 登录用户Cookie的有效期 
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                //是否对Cookie启用滑动过期时间。
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

            //添加MVC服务
            services.AddMvc(a => a.EnableEndpointRouting = false);

            #region 依赖注入
            //自动注入服务到依赖注入容器
            //services.RegisterAssemblyPublicNonGenericClasses()
            //    .Where(c => c.Name.EndsWith("Service"))
            //    .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);
            //.AsPublicImplementedInterfaces();

            //依赖注入 单例 作用域 瞬间
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


        //授权访问
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

            //数据初始化
            app.UseDataInitializer();

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

