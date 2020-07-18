using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.Cursos;
using Dominio;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistencia;
using Seguridad.TokenSeguridad;
using WebAPI.Middleware;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using AutoMapper;
using Persistencia.DapperConexion;
using Persistencia.DapperConexion.Instructor;
using Microsoft.OpenApi.Models;
using Persistencia.Paginacion;
using Microsoft.AspNetCore.Routing.Matching;

namespace WebAPI
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
            services.AddCors(opt =>
            {
                opt.AddPolicy("corsApp", builder =>
                 {
                     builder.AllowAnyOrigin();
                     builder.AllowAnyMethod();
                     builder.AllowAnyHeader();
                     
                 });

            });

            //Agregar esto para que levante el contexto(BD)
            services.AddDbContext<CursosOnlineContext>(opt => {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddOptions();

            //agregar para dapper
            services.Configure<ConexionConfiguracion>(Configuration.GetSection("ConnectionStrings"));

            services.AddMediatR(typeof(Consulta.Manejador).Assembly);


            services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
           
            })
            .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Nuevo>());


            var builder = services.AddIdentityCore<Usuario>();

            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);

            //Para instanciar el servicio de rol  manager
            identityBuilder.AddRoles<IdentityRole>();
            identityBuilder.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Usuario,IdentityRole>>();

            identityBuilder.AddEntityFrameworkStores<CursosOnlineContext>();
            identityBuilder.AddSignInManager<SignInManager<Usuario>>();
            services.TryAddSingleton<ISystemClock, SystemClock>();


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };
                });

            services.AddScoped<IJwtGenerator,JwtGenerator>();
            services.AddScoped<IUsuarioSession, UsuarioSession>();
            services.AddAutoMapper(typeof(Consulta.Manejador));

            services.AddTransient<IFactoryConnection, FactoryConnection>();
            services.AddScoped<IInstructor, InstructorRepositorio>();
            services.AddScoped<IPaginacion, PaginacionRepositorio>();
           

            //Para swagger
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Servicios para mantenimiento de cursos",
                    Version = "v1"
                });

                //para que agarre la ruta completa Namespace, clase + metodo
                opt.CustomSchemaIds(opt => opt.FullName);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ManejadorErrorMiddleware>();
            //if (env.IsDevelopment())
            //{
            //   // app.UseDeveloperExceptionPage();          
            //}

            ////app.UseHttpsRedirection();
            app.UseCors("corsApp");

            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            //para configurar los archivos staticos que vienen del la build de react
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                 );

                endpoints.MapFallbackToController("Index", "Home");
            });

            //Para Swagger
            app.UseSwagger();
            app.UseSwaggerUI(opt=> {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Cursos Online Version 1");
            });
        }
    }
}
