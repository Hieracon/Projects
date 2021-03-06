using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using GASF.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GASF.Services.Interfaces;
using GASF.Services.Repositories;
using GASF.Services.ImplementationServices;
using Microsoft.AspNetCore.Mvc;

namespace GASF
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

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped(typeof(IRepositoryWrapper), typeof(RepositoryWrapper));

            services.AddScoped<CourseAttendanceService>();
            services.AddScoped<CourseGradeService>();
            services.AddScoped<CourseMaterialService>();
            services.AddScoped<CourseService>();
            services.AddScoped<SecretaryService>();
            services.AddScoped<StudentService>();
            services.AddScoped<TeacherService>();

            services.AddDbContext<GASFContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<GASFContext>()
                .AddRoles<IdentityRole>()
                .AddDefaultTokenProviders();

            services.AddControllersWithViews();
            services.AddRazorPages();


            services.ConfigureApplicationCookie(options => {
                
                options.ReturnUrlParameter = "";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                
                options.LoginPath = "/Identity/Account/Login";
                
                options.AccessDeniedPath = "/AccessDenied";
                options.SlidingExpiration = true;

            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
