using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IdentityDemoNet3.IRepositories;
using IdentityDemoNet3.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityDemoNet3
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

            services.AddScoped<IMovieRepositorie, MovieRepositorie>();
            services.AddScoped<IGenreRepositorie, GenreRepositorie>();

            var connectionString = @"Server=(LocalDb)\MSSQLLocalDB;database=IdentityDemoNet3User;trusted_connection=yes";
            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<IdentityDemoUserDbContext>(opt => opt.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationAssembly)));
            services.AddIdentityCore<Usuario>(options => { });
            services.AddScoped<IUserStore<Usuario>,
                UserOnlyStore<Usuario, IdentityDemoUserDbContext>>(); //Especifica que user El repositorio o capa de acceso a datos predefinido "UserOnlyStore"

            services.AddAuthentication("cookies")
                        .AddCookie("cookies", options => options.LoginPath = "/Home/IniciarSesion");
            services.AddControllersWithViews();
         
          

        
        }

 
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
