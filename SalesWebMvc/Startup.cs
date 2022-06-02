using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Services;

namespace SalesWebMvc
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
            services.Configure<CookiePolicyOptions>(options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //basicamente chamamos o mysql, com o nome da nossa classe de content dentro de Data.
            //e depois add um delegate e entre a segunda aspas é o nome do nosso projeto
            services.AddDbContext<SalesWebMvcContext>(options =>
                    options.UseMySql(Configuration.GetConnectionString("SalesWebMvcContext"), builder => builder.MigrationsAssembly("SalesWebMvc")));

            //sempre que criar um novo service add aqui
            services.AddScoped<SeedingService>();
            services.AddScoped<SellerService>();
            services.AddScoped<DepartmentService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //esse metodo permite que coloque outros parametros nele
        //ao add o metodo seedingService no Configure automaticamente ja é instanciado ele mesmo
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SeedingService seedingService)
        {
            //se estivermos no perfil de desenvolvimento iremos rodar o SeedingService
            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
                seedingService.Seed();
                //entao populamos a base de dados caso ela nao esteja populada ainda
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    //o ? ao lado de id significa que o id eh opcional, entao nao 
                    //declaramos valor a ele
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}


