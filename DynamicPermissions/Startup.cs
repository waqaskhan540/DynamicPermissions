using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicPermissions.Authorization;
using DynamicPermissions.Authorization.PermissionService;
using DynamicPermissions.Config;
using DynamicPermissions.Data;
using DynamicPermissions.Data.Entities;
using DynamicPermissions.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DynamicPermissions
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

            services.AddTransient<IPermissionService, PermissionService>();

            services.AddDbContext<DatabaseContext>(opt => opt.UseInMemoryDatabase("efInMemory"));
            services.AddIdentityServer()
                     .AddDeveloperSigningCredential()
                     .AddInMemoryApiResources(IdentityServerConfig.GetApis())
                     .AddInMemoryClients(IdentityServerConfig.GetClients())
                     .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                     .AddTestUsers(IdentityServerConfig.GetTestUsers())
                     .AddProfileService<CustomProfileService>();

            services.AddAuthentication("Bearer")
                    .AddJwtBearer(options =>
                    {
                        options.Authority = "http://localhost:31987";
                        options.RequireHttpsMetadata = false;
                        options.Audience = "http://localhost:31987/resources";                         
                    });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
            });
            services.AddTransient<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IAuthorizationHandler, DynamicPermissionAuthorizationHandler>();
            services.AddScoped<IPermissionService, PermissionService>();

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy(
                    name: "DynamicPermission",
                    configurePolicy: policy =>
                   {
                       policy.RequireAuthenticatedUser();
                       policy.Requirements.Add(new DynamicPermissionRequirement());

                   });
            });
            services.AddCors(options => options.AddPolicy("cors", p =>
             {
                 p.AllowAnyHeader();
                 p.AllowAnyMethod();
                 p.AllowAnyOrigin();
             }));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("cors");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                SeedPermissionData(context);
            }


            app.UseIdentityServer();
            app.UseAuthentication();

            app.UseMvc();
        }

        private void SeedPermissionData(DatabaseContext context)
        {
            var roles = new List<Role>
            {
                new Role {Id = 1 ,RoleName = "Admin"},
                new Role {Id = 2 ,RoleName = "FreeUser"},
                new Role {Id = 3 ,RoleName = "PaidUser"},
                new Role {Id = 4 ,RoleName = "Visitor"},
            };

            context.Roles.AddRange(roles);
            context.SaveChanges();

            var permissions = new List<Permission>
            {
                new Permission(1,"Protected","FreeUser","FreeUser"),
                new Permission(2,"Protected","PaidUser","PaidUser"),
                new Permission(3,"Protected","AdminUser","AdminUser"),
                new Permission(4,"Protected","Visitor","Visitor"),
            };

            context.Permissions.AddRange(permissions);
            context.SaveChanges();
        }
    }
}
