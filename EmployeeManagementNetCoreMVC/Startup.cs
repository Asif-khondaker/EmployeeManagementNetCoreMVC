using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementNetCoreMVC.Models;
using EmployeeManagementNetCoreMVC.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmployeeManagementNetCoreMVC
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
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("EmployeeDBConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 3;
            }).AddEntityFrameworkStores<AppDbContext>();
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

            services.ConfigureApplicationCookie(option =>
           {
               option.AccessDeniedPath = new PathString("/Administration/AccessDenied");
           });

            //Google Auth
            services.AddAuthentication()
                .AddGoogle(option =>
                {
                    option.ClientId = "895936145722-mtsd52afd0sggqvpu2btlv0hnf9bh0sn.apps.googleusercontent.com";
                    option.ClientSecret = "_72acRvRMQFmVZt1Hf53cHZN";
                });

            //Claims Policy
            services.AddAuthorization(option =>
           {
               option.AddPolicy("DeleteRolePolicy",
                   policy => policy.RequireClaim("Delete Role")
                   );

               //option.AddPolicy("EditRolePolicy",
               //    policy => policy.RequireAssertion(context => GetAthorizeAccess(context))
               //    );

               option.AddPolicy("EditRolePolicy",
                   policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirment()));

               option.AddPolicy("AdminRolePolicy",
                    policy => policy.RequireRole("Admin")
                    );
               
           });

            //Claims Policy
            services.AddAuthorization(option =>
            {

            });
            //services.AddControllersWithViews().AddXmlSerializerFormatters();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
        }

        //Acess Method by Assertion
        private bool GetAthorizeAccess(AuthorizationHandlerContext context)
        {
            return context.User.IsInRole("Admin") &&
                context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
                context.User.IsInRole("Super Admin");
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
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");

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
            });
        }
    }
}
