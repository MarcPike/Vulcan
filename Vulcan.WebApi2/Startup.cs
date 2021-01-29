using DAL.Marketing.Helpers;
using DAL.Vulcan.Mongo.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO.Compression;
using System.Net;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.ExcelTemplates;
using DAL.Vulcan.Mongo.RequestLogging;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization;
using NPOI.SS.Formula.Functions;
using Vulcan.IMetal.Cache;
using Vulcan.IMetal.Helpers;
using Vulcan.WebApi2.Helpers;
using Vulcan.WebApi2.Hubs;
using Vulcan.WebApi2.Middleware;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;


namespace Vulcan.WebApi2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
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
            services.AddSingleton<IHelperPerson, HelperPerson>();
            services.AddSingleton<IHelperEmail, HelperEmail>();
            services.AddSingleton<IHelperProspect, HelperProspect>();
            services.AddSingleton<ITeamHub, TeamHub>();
            services.AddSingleton<IChatHub, ChatHub>();
            services.AddSingleton<IHelperTeamHub, HelperTeamHub>();
            services.AddSingleton<ILdapHelper, LdapHelper>();
            services.AddSingleton<IHelperReminder, HelperReminder>();
            services.AddSingleton<IHelperQuote, HelperQuote>();
            //services.AddSingleton<IStockItemsCache, StockItemsCache>();
            //services.AddSingleton<IPurchaseOrderItemsCache, PurchaseOrderItemsCache>();
            //services.AddSingleton<IProductMasterCache, ProductMasterCache>();
            services.AddSingleton<IHelperFile, HelperFile>();
            services.AddSingleton<IHelperQuoteQuery, HelperQuoteQuery>();
            services.AddSingleton<IHelperChart, HelperChart>();
            services.AddSingleton<IHelperMarketing, HelperMarketing>();
            services.AddSingleton<IHelperCurrencyForIMetal, HelperCurrencyForIMetal>();
            services.AddSingleton<IHelperCompanyPaymentTerms, HelperCompanyPaymentTerms>();
            services.AddSingleton<IHelperVersionHistory, HelperVersionHistory>();
            services.AddSingleton<IHelperPermissions, HelperPermissions>();
            services.AddSingleton<IHelperChat, HelperChat>();
            services.AddSingleton<IHelperTeamPriceTier, HelperTeamPriceTier>();
            services.AddSingleton<IHelperExternalLogin, HelperExternalLogin>();
            services.AddSingleton<IHelperPortal, HelperPortal>();
            services.AddSingleton<IHelperExcelTemplate, HelperExcelTemplate>();
            services.AddSingleton<IHelperRfq, HelperRfq>();
            services.AddSingleton<IHelperCroz, HelperCroz>();

            services.AddMvc().
                AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            //services.AddSignalR(options =>
            //  {
            //      options.EnableDetailedErrors = true;
            //  });

            services.AddSignalR( hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
                hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1);
            });

            if ((EnvironmentSettings.CurrentEnvironment == Environment.Development) || (EnvironmentSettings.RunningLocal))
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Vulcan Web Api",
                        Description = "Vulcan CRM Service",
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

            }


            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);

            services.AddResponseCompression(options =>
            {
                // options.EnableForHttps = true;
                options.MimeTypes = new string[] { "application/json" };
                options.Providers.Add<GzipCompressionProvider>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            RequestLog.PerformRestart();

            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseCors("CorsPolicy");
            //ConnectionManager = serviceProvider.GetService<IConnectionManager>();

            //app.UseStaticFiles();

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromMinutes(1),
                ReceiveBufferSize = 4 * 1024,
            };
            app.UseWebSockets(webSocketOptions);
            app.UseSignalR(route =>
            {
                //route.MapHub<ChatHub>("/chat");
                route.MapHub<TeamHub>("/team");
            });

            //app.Use(next => async (context) =>
            //{
            //    var hubContext = context.RequestServices
            //        .GetRequiredService<IHubContext<TeamHub>>();

            //});

            if ((EnvironmentSettings.CurrentEnvironment == Environment.Development) ||
                (EnvironmentSettings.RunningLocal))
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vulcan CRM WebAPI"); });
            }


            app.UseResponseCompression();
            app.UseMvcWithDefaultRoute();


        }

    }
}
