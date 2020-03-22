using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //km:this allows to access the settings
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //km:dependency injection controller
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(x=>x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            services.AddCors();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                                        .AddJwtBearer(options=>
                                        {
                                            options.TokenValidationParameters = new TokenValidationParameters
                                                {
                                                    ValidateIssuerSigningKey = true,
                                                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII
                                                        .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                                                        ValidateIssuer = false,
                                                        ValidateAudience = false
                                                };
                                        });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // km:middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //km:are we in development mode
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // the UseExceptionHandler will pipe all the exception to the builder which is middleware 
                app.UseExceptionHandler(builder => {
                    // Run is http request and response
                    builder.Run(async context => {
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError; 
                        // the error will store the particular error
                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if(error != null) {

                            context.Response.AddApplicationError(error.Error.Message);
                            // add own our extention method
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }

            //km :not listening on https
            //app.UseHttpsRedirection();

            //km:routing
            app.UseRouting();

            //km: usercors must be above auth and endpoints
            app.UseCors(x=>x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader() );

            app.UseAuthentication();

            //km:not configured
            app.UseAuthorization();
            //km:map our controller endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
        }
    }
}
