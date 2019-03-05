using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Universal.Core;
using Universal.Entities;
using Universal.Framework.Filters;
using Universal.Framework.Infrastructure;
using Universal.Framework.Menu;
using Universal.Framework.Security.Admin;
using Universal.Services;

namespace Universal.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            // 注入 EF上下文
            //services.AddDbContext<EFDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),b=>b.UseRowNumberForPaging()));
            //dotnet ef migrations add InitialEFDbContext -c EFDbContext -o Data/Migrations/DemoDB
            services.AddDbContextPool<EFDbContext>(options =>
            {
                switch (Configuration["ConnectionStrings:SqlType"])
                {
                    case "SqlServer":
                        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b =>
                        {
                            b.UseRowNumberForPaging();
                            b.MigrationsAssembly(assembly);
                        });
                        break;
                    case "MySql":
                        options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), mySqlOptions =>
                            {
                                mySqlOptions.ServerVersion(new Version(5, 6, 21), ServerType.MySql);
                                mySqlOptions.MigrationsAssembly(assembly);
                            });
                        break;
                    default:
                        Console.WriteLine("数据库配置无效");
                        break;
                }
            });

            // 注入 程序集依赖
            services.AddAssembly("Universal.Services");

            // 注入 泛型仓储
            services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));

            // 注入缓存
            services.AddSingleton<IMemoryCache, MemoryCache>();

            /*
             * ASP.NET Core中提供了一个IHttpContextAccessor接口，HttpContextAccessor 默认实现了它简化了访问HttpContext。
             * 它必须在程序启动时在IServicesCollection中注册，这样在程序中就能获取到HttpContextAccessor，并用来访问HttpContext。
             */
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // 注入 Admin认证
            services.AddScoped<IAdminAuthService, AdminAuthService>();

            // 注入 用户登录后的状态
            services.AddScoped<IWorkContext, WorkContext>();

            // 注入 初始化菜单
            services.AddSingleton<IRegisterMenuService, RegisterMenuService>();

            // 注入 Session
            services.AddSession();

            // 注入 认证信息
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = CookieAdminAuthInfo.AuthenticationScheme;
                o.DefaultChallengeScheme = CookieAdminAuthInfo.AuthenticationScheme;
            }).AddCookie(CookieAdminAuthInfo.AuthenticationScheme, o =>
            {
                o.LoginPath = "/admin/login";
            });

            // 初始化引擎
            EnginContext.Initialize(new UniversalEngine(services.BuildServiceProvider()));

            // 注入 MVC
            services.AddMvc(options =>
            {
                options.Filters.Add<HttpGlobalExceptionFilter>();
            });

            //services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);//这是为了防止中文乱码

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Admin/Main/Error404");
            }

            // 在请求管道中 可以使用跨域
            app.UseCors("AllowAllOrigin");

            // 在请求管道中 使用静态文件
            app.UseStaticFiles();

            // 在请求管道中 使用Session
            app.UseSession();

            // 在请求管道中添加认证
            app.UseAuthentication();

            //初始化数据库
            Policy.Handle<Exception>().Retry(3).Execute(() => app.InitData());

            //初始化菜单，保存进数据库
            EnginContext.Current.Resolve<IRegisterMenuService>().InitMenuRegister();

            // 在请求管道中 使用默认路由
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


            // 在请求管道中 使用区域路由
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Login}/{action=Index}/{id?}"
                );
            });

            Console.WriteLine("MVC程序已成功启动");
        }
    }
}
