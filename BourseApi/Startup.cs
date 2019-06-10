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

            app.UseMvc();
        }
    }
}
