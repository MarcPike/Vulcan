using BI.DAL.Mongo.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Configuration;
using System.IO.Compression;
using System.Reflection;
using BI.DAL.DataWarehouse.Core.Helpers;
using BI.WebApi.Tests;

namespace BI.WebApi
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }


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
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        //.WithOrigins("http.vulcan.howcogroup.com")
                        .WithOrigins("http://localhost:4200", "http://s-us-web02:100")
                        //.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddSingleton<IHelperPerson, HelperPerson>();
            services.AddSingleton<IHelperUser, HelperUser>();
            services.AddSingleton<IHelperSecurity, HelperSecurity>();
            services.AddSingleton<IHelperQueries, HelperQueries>();
            services.AddSingleton<IHelperOds, HelperOds>();


            services.AddControllers();

            services.AddMvc().
                AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.PropertyNamingPolicy = null;
                    o.JsonSerializerOptions.DictionaryKeyPolicy = null;
                }
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Howco BI API",
                    Description = "Business Intelligence for Howco",
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

            //ConnectionManager = serviceProvider.GetService<IConnectionManager>();

            //app.UseStaticFiles();

            //var webSocketOptions = new WebSocketOptions()
            //{
            //    KeepAliveInterval = TimeSpan.FromMinutes(1),
            //    ReceiveBufferSize = 4 * 1024
            //};
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vulcan BI WebAPI");
            });



            //app.UseRouting(o=>o.);

            app.UseResponseCompression();
            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //StaticTests.GetBiUserModel();
        }
    }
}
