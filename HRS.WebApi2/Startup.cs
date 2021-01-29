using DAL.HRS.Mongo.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO.Compression;
using DAL.Common.Helper;

namespace HRS.WebApi2
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
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });


            services.AddSingleton<IHelperLocation, HelperLocation>();
            services.AddSingleton<IHelperPerson, HelperPerson>();
            services.AddSingleton<IHelperFile, HelperFile>();
            services.AddSingleton<IHelperUser, HelperUser>();
            services.AddSingleton<IHelperSecurity, HelperSecurity>();
            services.AddSingleton<IHelperEmployee, HelperEmployee>();
            services.AddSingleton<IHelperProperties, HelperProperties>();
            services.AddSingleton<IHelperCompensation, HelperCompensation>();
            services.AddSingleton<IHelperDiscipline, HelperDiscipline>();
            services.AddSingleton<IHelperPerformance, HelperPerformance>();
            services.AddSingleton<IHelperBenefits, HelperBenefits>();
            services.AddSingleton<IHelperTraining, HelperTraining>();
            services.AddSingleton<IHelperHse, HelperHse>();
            services.AddSingleton<IHelperEmployeeIncidents, HelperEmployeeIncidents>();
            services.AddSingleton<IHelperRequiredActivity, HelperRequiredActivity>();
            services.AddSingleton<IHelperExternalApi, HelperExternalApi>();
            services.AddSingleton<IHelperMedicalInfo, HelperMedicalInfo>();
            services.AddSingleton<IHelperCommon, HelperCommon>();
            services.AddSingleton<IHelperIntegrations, HelperIntegrations>();
            services.AddSingleton<IHelperVersionHistory, HelperVersionHistory>();
            services.AddSingleton<IHelperLegal, HelperLegal>();
            services.AddSingleton<IHelperDashboard, HelperDashboard>();

            services.AddMvc().
                AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            


            //services.AddSignalR(options =>
            //  {
            //      options.EnableDetailedErrors = true;
            //  });

            //services.AddSignalR( hubOptions =>
            //{
            //    hubOptions.EnableDetailedErrors = true;
            //    hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1);
            //});

            services.AddSwaggerGen(swagger =>
            {
                swagger.DescribeAllEnumsAsStrings();
                swagger.DescribeAllParametersInCamelCase();
                swagger.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Vulcan HRS WebAPI" });
            });

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);

            services.AddResponseCompression(options =>
            {
                // options.EnableForHttps = true;
                options.MimeTypes = new string[] { "application/json" };
                options.Providers.Add<GzipCompressionProvider>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            //ConnectionManager = serviceProvider.GetService<IConnectionManager>();

            //app.UseStaticFiles();

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromMinutes(1),
                ReceiveBufferSize = 4 * 1024
            };
            //app.UseWebSockets(webSocketOptions);
            //app.UseSignalR(route =>
            //{
            //    route.MapHub<ChatHub>("/chat");
            //    route.MapHub<TeamHub>("/team");
            //});

            //app.Use(next => async (context) =>
            //{
            //    var hubContext = context.RequestServices
            //        .GetRequiredService<IHubContext<TeamHub>>();

            //});

            app.UseSwagger();
           
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vulcan HRS WebAPI");
            });

            app.UseResponseCompression();
            app.UseMvcWithDefaultRoute();


        }
    }
}
