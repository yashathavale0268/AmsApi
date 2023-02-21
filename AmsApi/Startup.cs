using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmsApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

using AmsApi.Repository;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection.AutoRegistration;
using AmsApi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AmsApi.Controllers;
using AmsApi.Configuration;
using System.Security.Claims;

namespace AmsApi
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
            
            services.AddControllers()
                .AddNewtonsoftJson()
                .AddControllersAsServices()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = null;
                });
            //----------------Role policies
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Adminonly", policy => policy.RequireRole("Admin"));
            //});
            //--------------Role policies
            services.AddSession(m => m.IdleTimeout = TimeSpan.FromMinutes(30));
            services.AddIdentity<UserAuth, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
            })
                    .AddEntityFrameworkStores<IdentityDbcontext>()
                    .AddDefaultTokenProviders();

           
            
            services.AddTransient<RegisterationController>();
            services.AddHttpContextAccessor();

            //services.AddIdentity<UserModel, IdentityRole>()
            //    .AddEntityFrameworkStores<IdentityDbcontext>()
            //    .AddDefaultTokenProviders();
            //        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = Configuration["Jwt:Issuer"],
            //        ValidAudience = Configuration["Jwt:Audience"],
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            //    };
            //});

            // configure strongly typed settings objects
            var jwtSection = Configuration.GetSection("JwtBearerTokenSettings");
                services.Configure<JwtBearerTokenSettings>(jwtSection);
                var jwtBearerTokenSettings = jwtSection.Get<JwtBearerTokenSettings>();
                var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtBearerTokenSettings.ValidIssuer,
                    ValidateAudience = true,
                    ValidAudience = jwtBearerTokenSettings.ValidAudience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                   // ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                    //,ValidAudience = Configuration["JWT:ValidAudience"],
                    //ValidIssuer = Configuration["JWT:ValidIssuer"],
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                    
                };
            //    ValidateIssuer = true,
            //ValidIssuer = jwtBearerTokenSettings.Issuer,
            //ValidateAudience = true,
            //ValidAudience = jwtBearerTokenSettings.Audience,
            //ValidateIssuerSigningKey = true,
            //IssuerSigningKey = new SymmetricSecurityKey(key),
            //ValidateLifetime = true,
            //ClockSkew = TimeSpan.Zero
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));

                // options.AddPolicy("Role", policy => policy.RequireClaim(ClaimTypes.Role,session.role));
            });
            ////services.AddControllers()
            //    .AddNewtonsoftJson()
            //    .AddControllersAsServices();
            //services.AddControllers().AddControllersAsServices().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            //}); 
            services.AddHttpClient();
            services.AddScoped<LoginRepository>();
            services.AddScoped<AssettypeRepository>();
            services.AddScoped<AssetRepository>();
            services.AddScoped<BranchRepository>();
            services.AddScoped<CompanyRepository>();
            services.AddScoped<DepartmentRepository>();
            services.AddScoped<UserDetailsRepository>();
            services.AddScoped<VendorRepository>();
            services.AddScoped<StatusRepository>();
            services.AddScoped<RequestRepository>();
            services.AddScoped<ScrapRepository>();
          
            services.AddMvc();
         //   services.Configure<DbContext>(Configuration.GetSection("Maincon"));
            services.AddDbContext<IdentityDbcontext>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("MainCon")));
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AmsApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AmsApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors( m=> m.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }


}

