using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorServerApp.Areas.Identity;
using BlazorServerApp.Data;
using BlazorServerApp.Services;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Cosmos;
using BlazorServerApp.Models;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using BlazorServerApp.Areas.Identity.Data;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace BlazorServerApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();
            //One client instance per container
            services.AddSingleton<ICosmosDbService<MarvelCharactersResult>>(InitializeCosmosClientInstanceAsync<MarvelCharactersResult>(Configuration.GetSection("CosmosDb"), "MarvelCharactersResult").GetAwaiter().GetResult());
            services.AddHttpClient();
            services.AddScoped<AppState>();
            services.AddScoped<IMarvelCharacterService, MarvelCharacterService>();
            services.AddSignalR();

            services.AddAuthorization(options =>
            {
                //options.AddPolicy("Name", policy => policy.RequireClaim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName));
                //options.AddPolicy("Name", policy => policy.RequireClaim(ClaimTypes.Name));
                options.AddPolicy("Name", policy => policy.RequireClaim(ClaimTypes.Name));
            });

            //services.AddAuthentication().AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            //{
            //    options.SlidingExpiration = true;
            //    //options.LoginPath = $"/Identity/Account/Login";
            //    //options.LogoutPath = $"/Identity/Account/Logout";
            //    //options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            //});

            services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("Token").GetSection("Key").Value)),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //    name: "api", 
                //    pattern: "api/{controller}/{action}/{id?}");
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapRazorPages();
                endpoints.MapHub<RealTimeHub>("/chatHub");
                endpoints.MapFallbackToPage("/_Host");
            });
        }

        private async Task<CosmosDbService<T>> InitializeCosmosClientInstanceAsync<T>(IConfigurationSection configurationSection, string containerName)
        {
            string databaseName = configurationSection.GetSection("DatabaseName").Value;
            string account = configurationSection.GetSection("Account").Value;
            string key = configurationSection.GetSection("Key").Value;
            CosmosClientBuilder clientBuilder = new CosmosClientBuilder(account, key);
            CosmosClient client = clientBuilder
                                .WithConnectionModeDirect()
                                .Build();
            CosmosDbService<T> cosmosDbService = new CosmosDbService<T>(client, databaseName, containerName);
            DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/api_id");

            return cosmosDbService;
        }
    }

}
