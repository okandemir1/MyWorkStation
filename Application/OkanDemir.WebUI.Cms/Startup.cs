using OkanDemir.Business;
using OkanDemir.Business.Services;
using OkanDemir.Data;
using OkanDemir.Data.Repository;
using OkanDemir.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using OkanDemir.WebUI.Cms.Helpers;
using System.Globalization;
using Hangfire;
using Hangfire.MemoryStorage;

namespace OkanDemir.Application.Cms
{
    public partial class Startup
    {
        public IConfiguration Configuration { get; }
        public IConfigurationRoot ConfigurationRoot { get; set; }
        public Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment { get; set; }

        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;

            ConfigurationRoot = new ConfigurationBuilder()
                            .SetBasePath(env.ContentRootPath)
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                            .AddEnvironmentVariables()
                            .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(config =>
                   config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                       .UseSimpleAssemblyNameTypeSerializer()
                       .UseDefaultTypeSerializer()
                       .UseMemoryStorage());
            services.AddHangfireServer();
            services.AddTransient<BackgroundServices>();

            services.AddDbContext<OkanDemirDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ArchiveCategoryBusiness>();
            services.AddScoped<ArchiveBusiness>();
            services.AddScoped<UserBusiness>();
            services.AddScoped<IncomeBusiness>();
            services.AddScoped<IncomeTypeBusiness>();
            services.AddScoped<InvoiceTypeBusiness>();
            services.AddScoped<InvoiceBusiness>();
            services.AddScoped<ExpenseBusiness>();
            services.AddScoped<CodeCategoryBusiness>();
            services.AddScoped<CodeNoteBusiness>();
            services.AddScoped<SubscriptionBusiness>();
            services.AddScoped<SubscriptionTypeBusiness>();
            services.AddScoped<NoteBusiness>();
            services.AddScoped<TodoBusiness>();

            //Services
            services.AddScoped<SmsService>();
            services.AddScoped<EmailService>();
            services.AddScoped<FileService>();

            // Infra - Data
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<OkanDemirDbContext>();

            //Cache
            services.AddMemoryCache();
            services.AddTransient<ICache, Infrastructure.Caching.MemoryCache.Cache>();
            services.AddTransient<CacheHelper>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<BreadCrumbHelperExtensions>();
            services.AddTransient<PageHeaderHelperExtensions>();
            services.AddTransient<FormHelperExtensions>();

            if (Environment.IsDevelopment())
            {
                services.AddControllersWithViews().AddRazorRuntimeCompilation();
            }

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o => { o.LoginPath = new PathString("/home/login/"); });

            services.AddControllersWithViews();

            services.AddMvc(x =>
            {
                x.EnableEndpointRouting = false;
            });
        }

        public void Configure(IApplicationBuilder app
            , IWebHostEnvironment env
            , IRecurringJobManager recurringJobManager)
        {
            var cultureInfo = new CultureInfo("tr-TR");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }
            else { app.UseHttpsRedirection(); }

            app.UseAuthentication();
            app.UseRouting();

            var contentTypeProvider = new FileExtensionContentTypeProvider();
            contentTypeProvider.Mappings.Add(".avif", "image/avif");

            app.UseStaticFiles(new StaticFileOptions
            {
                HttpsCompression = Microsoft.AspNetCore.Http.Features.HttpsCompressionMode.Compress,
                OnPrepareResponse = (context) =>
                {
                    var headers = context.Context.Response.GetTypedHeaders();
                    headers.CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromDays(365)
                    };
                },
                ContentTypeProvider = contentTypeProvider
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            recurringJobManager.AddOrUpdate<BackgroundServices>("Arkaplan Servisi", x => x.Execute(), "*/3 * * * *");
        }
    }
}