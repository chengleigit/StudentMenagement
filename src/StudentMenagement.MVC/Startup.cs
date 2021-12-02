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
using Microsoft.OpenApi.Models;
using NetCore.AutoRegisterDi;
using StudentMenagement.Application;
using StudentMenagement.Application.Courses;
using StudentMenagement.Application.Departments;
using StudentMenagement.Application.Dtos;
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
using System.IO;
using System.Linq;
using System.Reflection;
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

            //�����ӳټ���UseLazyLoadingProxies()
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

  //          var assembliesToScan = new[]   {
  //Assembly.GetExecutingAssembly(),
  //Assembly.GetAssembly(typeof(PagedResultDto<>)),//��ΪPagedResultDto<>��
  ////MockSchoolManagement.Application����У�����ͨ��PagedResultDto<>��ȡ������Ϣ
  //          };

  //          //�Զ�ע���������ע������
  //          services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)//����
  //                                                                            //ȡ���ĳ�����Ϣע�ᵽ���ǵ�����ע��������
  //          .Where(c => c.Name.EndsWith("Service"))
  //          .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

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
            services.AddScoped<IDepartmentsService, DepartmentsService>();

            #endregion



            // ע��Swagger������������һ������Swagger�ļ�
            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("v1",new OpenApiInfo { Title = "StudentManagement API",Version = "v1" });

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "StudentManagement API",
                    Description = "StudentManagement�����һ���򵥵�ASP.NET Core Web APIʾ������52ABP��Ʒ��",
                    Version = "v1",
                    TermsOfService = new Uri("https://sc.52abp.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "��ͩ��",
                        Email = "ltm@ddxc.org",
                        Url = new Uri("https://github.com/ltm0203/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Apache License 2.0",
                        Url = new Uri("https://github.com/yoyomooc/asp.net-core--for-beginner/blob/master/LICENSE"),
                    }
                });
                if (_env.IsDevelopment())
                {
                    // ����Swagger JSON��UI��ע��·��
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                }
            });



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


            // �����м��Swagger()
            app.UseSwagger();
            //�����м��Swagger()��UI��������Ҫ��Swagger()������һ��
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudentManagement API V1");
            });



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

