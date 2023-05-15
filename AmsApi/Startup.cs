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

using AspNetCoreRateLimit;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

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
            services.AddOptions();
            services.AddMemoryCache();
           services.AddCors();

            #region rate limiting config
            //services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            //services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
            //services.Configure<IpRateLimitOptions>(options =>
            //{

            //    options.EnableEndpointRateLimiting = true;
            //    options.StackBlockedRequests = true;
            //    options.HttpStatusCode = 429;
            //    options.RealIpHeader = "X-Real-IP";
            //    options.ClientIdHeader = "X-ClientId";
            //    options.GeneralRules = new List<RateLimitRule>
            //    {
            //            new RateLimitRule
            //            {
            //                   Endpoint = "POST:/api/Registeration/Login",
            //                    Period = "1s",
            //                    Limit = 5,
            //                    //DelayMs= 0,
            //                    //Randomized= true

            //            },  
            //            new RateLimitRule
            //            {
            //                   Endpoint = "POST:/api/Registeration/NewUser",
            //                    Period = "1s",
            //                    Limit = 5,

            //            },
            //            new RateLimitRule
            //            {
            //                   Endpoint = "POST:/api/Request/CreateNew",
            //                    Period = "1s",
            //                    Limit = 5,

            //            }
            //    };

            //});

            //services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            //services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            //services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            //services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            //services.AddInMemoryRateLimiting();

            #endregion

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

            #region for ratelimiting
            //----------------------------------------------------------------------------------------------------------------------------------------
            //services.AddMemoryCache();
            //services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            //services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
            //services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            //services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            //services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            //----------------------------------------------------------------------------------------------------------------------------------------
            // services.AddMemoryCache();
            // services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            //// services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
            // services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            // services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            // services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            #endregion

            #region idneity role settings
            //-----
            //services.AddIdentity<UserAuth, IdentityRole>(options =>
            //{
            //    options.User.RequireUniqueEmail = true;
            //    options.Password.RequireDigit = true;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequiredLength = 8;
            //})
            //      //  .AddEntityFrameworkStores<IdentityDbcontext>()
            //        .AddDefaultTokenProviders();

            #endregion

            services.AddTransient<RegisterationController>();
            services.AddHttpContextAccessor();

            ///----------------Ocelot--------------///
            services.AddOcelot();

            ///----------------Ocelot--------------///

            #region idenity auth
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
            #endregion

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
                    ClockSkew = TimeSpan.Zero
                    


                };
                options.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(jwtBearerTokenSettings.ExpiryTimeInMinutes);
                
                #region validations
                // ValidateLifetime = true,
                //,ValidAudience = Configuration["JWT:ValidAudience"],
                //ValidIssuer = Configuration["JWT:ValidIssuer"],
                //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                //    ValidateIssuer = true,
                //ValidIssuer = jwtBearerTokenSettings.Issuer,
                //ValidateAudience = true,
                //ValidAudience = jwtBearerTokenSettings.Audience,
                //ValidateIssuerSigningKey = true,
                //IssuerSigningKey = new SymmetricSecurityKey(key),
                //ValidateLifetime = true,
                //ClockSkew = TimeSpan.Zero
                #endregion
            });
            services.AddAuthorization(options =>
            {
               // options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));

                // options.AddPolicy("Role", policy => policy.RequireClaim(ClaimTypes.Role,session.role));
            });
            #region don't know why so kept it 
            ////services.AddControllers()
            //    .AddNewtonsoftJson()
            //    .AddControllersAsServices();
            //services.AddControllers().AddControllersAsServices().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            //}); 
            #endregion


            services.AddHttpClient();
            services.AddScoped<LoginRepository>();
            services.AddScoped<AssettypeRepository>();
            services.AddScoped<AssetRepository>();
            services.AddScoped<BranchRepository>();
            services.AddScoped<CompanyRepository>();
            services.AddScoped<DepartmentRepository>();
            services.AddScoped<MenuRepository>();
            services.AddScoped<ProjectRepository>();
            services.AddScoped<VendorRepository>();
            services.AddScoped<StatusRepository>();
            services.AddScoped<RequestRepository>();
            services.AddScoped<ScrapRepository>();
            services.AddScoped<LocationRepository>();
            services.AddScoped<ReportsRepository>();
            services.AddScoped<TransferRepository>();
            services.AddMvc();

            #region//--------Identity connection---------//
            // services.Configure<DbContext>(Configuration.GetSection("Maincon"));
            // services.AddDbContext<IdentityDbcontext>(options => 
            // options.UseSqlServer(Configuration.GetConnectionString("MainCon")));
            #endregion


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
            //app.UseIpRateLimiting();
            app.UseHsts();
            app.UseHttpsRedirection();

            app.UseRouting();
            // global cors policy
            // app.UseCors(x=> x.AllowAnyHeader().
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
           // app.UseOcelot().Wait();
        }
    }


}

