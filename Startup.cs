using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RestApiDating.Data;
using RestApiDating.Helpers;

namespace RestApiDating
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(x =>
            {
                // UseLazyLoadingProxies() carga las entidades relacionadas automaticamente
                // con esto ya no es necesario utilizar el metodo include()
                // pero es necesario agregar 'virtual' a las propiedades relacionales
                x.UseLazyLoadingProxies();
                x.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });

            ConfigureServices(services);
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(x =>
            {
                // UseLazyLoadingProxies() carga las entidades relacionadas automaticamente
                // con esto ya no es necesario utilizar el metodo include()
                // pero es necesario agregar 'virtual' a las propiedades relacionales
                x.UseLazyLoadingProxies();
                x.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
            });

            ConfigureServices(services);
        }

        // Aquí se inyectan los servicios.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                // para evitar error de bucle de autoreferencia
                .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            // CORS
            services.AddCors();

            // Add Jwt support
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(op =>
            {
                op.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("Jwt:Key").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            //-------- Filtro Jwt para los request --------
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

                config.Filters.Add(new AuthorizeFilter(policy));
            });

            // inyectamos los repositorios
            // AddScoped crea una nueva instancia por cada request
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IDatingRepository, DatingRepository>();
            
            // Automapper
            ServiceCollectionExtensions.UseStaticRegistration = false;
            services.AddAutoMapper(typeof(Startup));

            // Cloudinary
            services.Configure<CloudinarySettings>(Configuration.GetSection("Cloudinary"));

            // Filter Action
            services.AddScoped<LogUserActivity>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                this.ExceptionHandler(app);
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseAuthentication();
            app.UseDefaultFiles(); // para servir los archivos del frontend
            app.UseStaticFiles(); // para servir los archivos del wwwroot (frontend)
            // para que la API pueda resolver las rutas del frontend
            app.UseMvc(routes =>
            {
                routes.MapSpaFallbackRoute(
                  name: "spa-fallback",
                  defaults: new { controller = "Home", action = "Index" }
                );
            });

            string basePath = Configuration.GetSection("ProductionBasePath:path").Value;

            app.Map(basePath, appBuilder =>
            {
                appBuilder.UseSpa(spa =>
                {
                    spa.Options.DefaultPage = basePath + "/index.html";
                    spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions { RequestPath = basePath };
                });
            });
        }

        // manejador global de excepciones
        private void ExceptionHandler(IApplicationBuilder app)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        context.Response.AddApplicationError(error.Error.Message);
                        await context.Response.WriteAsync(error.Error.Message);
                    }
                });
            });
        }
    }
}
