using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EGrower.Infrastructure.Aggregates;
using EGrower.Infrastructure.Aggregates.Interfaces;
using EGrower.Infrastructure.Commands.EmailAccount;
using EGrower.Infrastructure.Commands.SendedEmail;
using EGrower.Infrastructure.Commands.User;
using EGrower.Infrastructure.Data;
using EGrower.Infrastructure.Extension.AutoMapper;
using EGrower.Infrastructure.Extension.EmailConfiguration;
using EGrower.Infrastructure.Extension.Exception;
using EGrower.Infrastructure.Extension.Interfaces;
using EGrower.Infrastructure.Extension.JWT;
using EGrower.Infrastructure.Factories;
using EGrower.Infrastructure.Factories.Interfaces;
using EGrower.Infrastructure.Repositories;
using EGrower.Infrastructure.Repositories.Interfaces;
using EGrower.Infrastructure.Services;
using EGrower.Infrastructure.Services.Interfaces;
using EGrower.Infrastructure.Validators;
using EGrower.Infrastructure.Validators.EmailAccount;
using EGrower.Infrastructure.Validators.SendedEmail;
using EGrower.Infrastructure.Validators.User;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using static EGrower.Infrastructure.Extension.AutoMapper.ActionFilters.ValidationActionFilter;

namespace EGrower.Api {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddMvc (opt => {
                    opt.Filters.Add (typeof (ValidatorActionFilter));
                }).AddFluentValidation ()
                .AddJsonOptions (options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            #region DbContextandSettings
            services.AddCors ();
            services.AddDbContext<EGrowerContext> (options =>
                options.UseSqlServer (Configuration.GetConnectionString ("EGrowerDatabase"),
                    b => b.MigrationsAssembly ("EGrower.Api")));
            var key = Encoding.ASCII.GetBytes (Configuration.GetSection ("JWTSettings:Key").Value);
            services.AddAuthentication (JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer (options => {
                    options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey (key),
                    ValidateIssuer = false,
                    ValidateAudience = false

                    };
                });
            services.AddAuthorization (options => options.AddPolicy ("admin", policy => policy.RequireRole ("admin")));
            services.AddAuthorization (options => options.AddPolicy ("user", policy => policy.RequireRole ("user")));
            services.AddAutoMapper (x => x.AddProfile (new MappingProfiles ()));
            services.AddSingleton<IEmailConfiguration> (Configuration.GetSection ("EmailConfiguration").Get<EmailConfiguration> ());
            services.AddSingleton<IJWTSettings> (Configuration.GetSection ("JWTSettings").Get<JWTSettings> ());
            // services.AddSingleton<IFileProvider> (
            //     new PhysicalFileProvider (
            //         Path.Combine (Directory.GetCurrentDirectory (), "wwwroot")));
            #endregion
            #region Validations
            services.AddTransient<IValidator<SignInUser>, SignInUserValidator> ();
            services.AddTransient<IValidator<CreateUser>, CreateUserValidator> ();
            services.AddTransient<IValidator<UpdateUser>, UpdateUserValidator> ();
            services.AddTransient<IValidator<RestorePassword>, RestorePasswordValidator> ();
            services.AddTransient<IValidator<ChangePasswordByRestoringPassword>, ChangePasswordByRestoringPasswordValidator> ();
            services.AddTransient<IValidator<ChangePassword>, ChangePasswordValidator> ();
            services.AddTransient<IValidator<CreateEmailAccount>, CreateEmailAccountValidator> ();
            services.AddTransient<IValidator<CreateSendedEmail>, CreateSendedEmailValidator> ();
            #endregion
            #region Repositories

            services.AddScoped<IUserRepository, UserRepository> ();
            services.AddScoped<IEmailMessageRepository, EmailMessageRepository> ();
            services.AddScoped<IAtachmentRepository, AtachmentRepository> ();
            services.AddScoped<ISendedEmailMessageRepository, SendedEmailMessageRepository> ();
            services.AddScoped<ISendedAtachmentRepository, SendedAtachmentRepository> ();
            services.AddScoped<IEmailAccountRepository, EmailAccountRepository> ();
            services.AddScoped<IImapRepository, ImapRepository> ();
            services.AddScoped<ISmtpRepository, SmtpRepository> ();

            #endregion
            #region Services

            services.AddScoped<IAuthService, AuthService> ();
            services.AddScoped<IUserService, UserService> ();
            services.AddScoped<IIMapService, IMapService> ();
            services.AddScoped<ISmtpService, SmtpService> ();
            services.AddScoped<IEmailAccountService, EmailAccountService> ();
            services.AddScoped<IEmailAccountIMapService, EmailAccountIMapService> ();
            services.AddScoped<IEmailAccountSmtpService, EmailAccountSmtpService> ();
            services.AddScoped<IEmailMessageService, EmailMessageService> ();
            services.AddScoped<ISendedEmailMessageService, SendedEmailMessageService> ();

            #endregion
            #region Factories

            services.AddScoped<IEmailFactory, EmailFactory> ();
            services.AddScoped<IUserEmailFactory, UserEmailFactory> ();
            services.AddScoped<IEmailClientFactory, EmailClientFactory> ();

            #endregion
            #region Agregates

            services.AddScoped<IEmailClientAggregate, EmailClientAggregate> ();
            services.AddScoped<IUserAggregate, UserAggregate> ();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                app.UseExceptionHandler (builder => {
                    builder.Run (async context => {
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        var error = context.Features.Get<IExceptionHandlerFeature> ();
                        if (error != null) {
                            context.Response.AddApplicationError (error.Error.Message);
                            await context.Response.WriteAsync (error.Error.Message);
                        }
                    });
                });
            }
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory> ().CreateScope ()) {
                if (!serviceScope.ServiceProvider.GetService<EGrowerContext> ().AllMigrationsApplied ()) {
                    serviceScope.ServiceProvider.GetService<EGrowerContext> ().Database.Migrate ();
                }
            }
            app.UseCors (x => x.AllowAnyHeader ().AllowAnyMethod ().AllowAnyOrigin ().AllowCredentials ());
            app.UseAuthentication ();
            app.UseMvc ();
        }
    }
}