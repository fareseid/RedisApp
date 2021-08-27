using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RedisServer.Logging;
using RedisServer.Middleware;
using RedisServer.Parser;
using RedisServer.RedisOperations;
using RedisServer.Repository;
using RedisServer.Service; 
using System.Collections.Generic;
using System.Text; 

namespace RedisServer
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

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                //First we define the security scheme
                c.AddSecurityDefinition("Bearer", //Name the security scheme
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
                        Scheme = "bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = "Bearer", //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }

                });
            });


            Model.AppModel.AppSettings AppSettings = Helper.AppSettingsHelper.RetrieveAppSettings();
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               options.RequireHttpsMetadata = false;
               options.SaveToken = true;
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = "RedisCloneServer",
                   ValidAudience = "RedisCloneClient",
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.SecuritySettings.JwtSecretKey))

               };
           });

            services.AddSingleton<IRepository, InMemoryRepository>();
            services.AddScoped<IRedisService,RedisService >(); 
            services.AddScoped<AbstractRedisOperation, RedisGetOperation>();
            services.AddScoped<IRedisCommandParser, RedisCommandParser>();
            services.AddScoped<IRedisLogger, RedisLogger>();
            services.AddScoped<IAuthService, JwtAuthService>();

        }
         

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRedisLogger RedisLogger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RedisServer v1"));

            }



            // global error handler
            app.UseMiddleware<ErrorHandlerMiddleware>(RedisLogger);


            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization(); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
