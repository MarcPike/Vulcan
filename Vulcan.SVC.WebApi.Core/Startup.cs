using DAL.Marketing.Core.Helpers;
using DAL.Vulcan.Mongo.Base.Core.Context;
using DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates;
using DAL.Vulcan.Mongo.Core.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO.Compression;
using System.Text.Json;
using DAL.iMetal.Core.Helpers;
using Vulcan.SVC.WebApi.Core.Helpers;
using Vulcan.SVC.WebApi.Core.Hubs;
using Vulcan.SVC.WebApi.Core.Middleware;
using HelperUser = DAL.Vulcan.Mongo.Core.Helpers.HelperUser;
using IHelperUser = DAL.Vulcan.Mongo.Core.Helpers.IHelperUser;

namespace Vulcan.Svc.WebApi.Core
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }
        readonly string MyAllowSpecificOrigins = "CorsPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder => builder
                        //.WithOrigins("http.vulcan.howcogroup.com")
                        .WithOrigins(
                            "http://localhost:4200",
                            "http://s-us-web02:83",
                            "http://s-us-web02:82",
                            "http://s-us-web02:88",
                            "http://vulcan.howcogroup.com")
                        //.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            RegisterExcelClassMaps.Execute();

            services.AddSingleton<IHelperUser, HelperUser>();
            services.AddSingleton<IHelperCompany, HelperCompany>();
            services.AddSingleton<IHelperApplication, HelperApplication>();
            services.AddSingleton<IHelperLocation, HelperLocation>();
            services.AddSingleton<IHelperTeam, HelperTeam>();
            services.AddSingleton<IHelperDomain, HelperDomain>();
            services.AddSingleton<IHelperAction, HelperAction>();
            services.AddSingleton<IHelperUserViewConfig, HelperUserViewConfig>();
            services.AddSingleton<IHelperNotifications, HelperNotifications>();
            services.AddSingleton<IHelperGoal, HelperGoal>();
            services.AddSingleton<IHelperContact, HelperContact>();
            services.AddSingleton<DAL.Vulcan.Mongo.Helpers.IHelperPerson, DAL.Vulcan.Mongo.Helpers.HelperPerson>();
            services.AddSingleton<IHelperProspect, HelperProspect>();
            services.AddSingleton<ITeamHub, TeamHub>();
            services.AddSingleton<IChatHub, ChatHub>();
            services.AddSingleton<IHelperTeamHub, HelperTeamHub>();
            services.AddSingleton<ILdapHelper, LdapHelper>();
            services.AddSingleton<IHelperReminder, HelperReminder>();
            services.AddSingleton<IHelperQuote, HelperQuote>();
            services.AddSingleton<IHelperFile, HelperFile>();
            services.AddSingleton<IHelperQuoteQuery, HelperQuoteQuery>();
            services.AddSingleton<IHelperChart, HelperChart>();
            services.AddSingleton<IHelperMarketing, HelperMarketing>();
            services.AddSingleton<IHelperCurrencyForIMetal, HelperCurrencyForIMetal>();
            //services.AddSingleton<IHelperCompanyPaymentTerms, HelperCompanyPaymentTerms>();
            services.AddSingleton<IHelperVersionHistory, HelperVersionHistory>();
            services.AddSingleton<IHelperPermissions, HelperPermissions>();
            services.AddSingleton<IHelperChat, HelperChat>();
            services.AddSingleton<IHelperTeamPriceTier, HelperTeamPriceTier>();
            services.AddSingleton<IHelperExternalLogin, HelperExternalLogin>();
            services.AddSingleton<IHelperPortal, HelperPortal>();
            services.AddSingleton<IHelperExcelTemplate, HelperExcelTemplate>();
            services.AddSingleton<IHelperRfq, HelperRfq>();
            services.AddSingleton<IHelperCroz, HelperCroz>();

            services.AddControllers();

            services.AddMvc().
                AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    o.JsonSerializerOptions.DictionaryKeyPolicy = null;
                    o.JsonSerializerOptions.IgnoreNullValues = false;
                }
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Vulcan CRM WebAPI 3.1",
                    Description = "Howco Vulcan CRM System using Aps.Net Core 3.1",
                    TermsOfService = new Uri("https://howcogroup.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "Marc Pike",
                        Email = "marc.pike@howcogroup.com",
                        Url = new Uri("https://www.howcogroup.com"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
            });

            services.AddSignalR();

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);

            services.AddResponseCompression(options =>
            {
                // options.EnableForHttps = true;
                options.MimeTypes = new string[] { "application/json" };
                options.Providers.Add<GzipCompressionProvider>();
            });

            //AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //RequestLog.PerformRestart();
            //app.UseMiddleware<RequestLoggingMiddleware>();


            app.UseResponseCompression();
            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            if ((EnvironmentSettings.CurrentEnvironment == DAL.Vulcan.Mongo.Base.Core.Context.Environment.Development) ||
                (EnvironmentSettings.RunningLocal))
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vulcan CRM WebAPI 3.1"); });
            }

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromMinutes(1),
                ReceiveBufferSize = 4 * 1024
            };
            app.UseWebSockets(webSocketOptions);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors(MyAllowSpecificOrigins);
                //endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapHub<TeamHub>("/team");
            });

            

        }
    }
}
