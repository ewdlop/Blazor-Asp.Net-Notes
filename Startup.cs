using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using BlazorServerApp.Areas.Identity.Data;
using BlazorServerApp.Areas.Identity;
using BlazorServerApp.Data;
using BlazorServerApp.Models;
using BlazorServerApp.Services.Options;
using BlazorServerApp.Services.Providers;
using BlazorServerApp.Services;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;

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

            services.AddDefaultIdentity<ApplicationUser>(
                options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Tokens.ProviderMap.Add("CustomEmailConfirmation",
                        new TokenProviderDescriptor(
                            typeof(CustomEmailConfirmationTokenProvider<ApplicationUser>)));
                    options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddTransient<CustomEmailConfirmationTokenProvider<ApplicationUser>>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.ConfigureApplicationCookie(o =>
            {
                o.ExpireTimeSpan = TimeSpan.FromDays(5);
                o.SlidingExpiration = true;
            });

            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();
            services.AddScoped<IGremlinClient, GremlinClient>((serviceProvider) =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                string EndpointUrl = config.GetSection("CosmosDbGreminlin").GetSection("Endpoint").Value;
                string PrimaryKey = config.GetSection("CosmosDbGreminlin").GetSection("PrimaryKey").Value;
                const int port = 443;
                string database = config.GetSection("CosmosDbGreminlin").GetSection("DatabaseName").Value;
                string container = config.GetSection("CosmosDbGreminlin").GetSection("ContainerName").Value;
                GremlinServer gremlinServer = new GremlinServer(EndpointUrl, port, enableSsl: true,
                                    username: "/dbs/" + database + "/colls/" + container,
                                    password: PrimaryKey);
                return new GremlinClient(gremlinServer, new GraphSON2Reader(), new GraphSON2Writer(), GremlinClient.GraphSON2MimeType);
            });
            services.AddSingleton<ICosmosDbService<MarvelCharactersResult>>(InitializeCosmosClientInstanceAsync<MarvelCharactersResult>("MarvelCharactersResult").GetAwaiter().GetResult());
            services.AddSingleton<ICosmosDbGremlinService, CosmosDbGremlinService>();
            services.AddSingleton<IMarvelCharacterService, MarvelCharacterService>();
            services.AddScoped<AppState>();
            services.AddHttpClient();
            services.AddHttpContextAccessor();

            services.AddAuthorization(options =>
            {
                //options.AddPolicy("Name", policy => policy.RequireClaim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName));
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("JWToken").GetSection("Key").Value)),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });

            services.AddBlazorise(options => { }).AddBootstrapProviders().AddFontAwesomeIcons();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSignalR();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "ewdlop",
                        Email = "ray810815@gmail.com",
                        Url = new Uri("https://example.com/terms"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
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
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //put custom middleware here before endpoints
            
            app.ApplicationServices
              .UseBootstrapProviders()
              .UseFontAwesomeIcons();

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

        private async Task<CosmosDbService<T>> InitializeCosmosClientInstanceAsync<T>(string containerName)
        {
            string databaseName = "LocalCosmosDb";
            string account = Configuration["CosmosDbAccount"];
            string key = Configuration["CosmosDbPrimaryKey"];
            CosmosClientBuilder clientBuilder = new CosmosClientBuilder(account, key);
            CosmosClient client = clientBuilder.WithConnectionModeDirect().Build();
            CosmosDbService<T> cosmosDbService = new CosmosDbService<T>(client, databaseName, containerName);
            DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/api_id");

            return cosmosDbService;
        }
    }

}
