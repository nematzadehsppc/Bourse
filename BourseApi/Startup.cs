using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BourseApi.Contract;
using BourseApi.Repositories;
using Back.DAL.Context;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BourseApi
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
            //برای فراخوانی متدهای محافظت شده از فناوری
            //JWT (JSon Web Tokens)
            //استفاده می‌کنیم
            //خطوط بعدی برای پیکربندی مراجعه
            //Authorize
            //ها به این فناوری گذاشته شده است

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "bearer";
            }).AddJwtBearer("bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidAudience = "Everyone",
                    ValidateIssuer = true,
                    ValidIssuer = "SPPC",

                    ValidateIssuerSigningKey = true,
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TadbirSecurityParameters.Secret)),

                    ValidateLifetime = true, //validate the expiration and not before values in the token

                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };

            });

            services.AddDbContext<Back.DAL.Context.UAppContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("UAppContext")));

            services.AddTransient<DbContext, Back.DAL.Context.UAppContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISymbolRepository, SymbolRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IParamValueRepository, ParamValueRepository>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //استفاده از JWT
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
