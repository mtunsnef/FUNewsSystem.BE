using FluentValidation;
using FluentValidation.AspNetCore;
using FUNewsSystem.API.Middlewares;
using FUNewsSystem.Domain.Configs.OData;
using FUNewsSystem.Domain.Models;
using FUNewsSystem.Infrastructure.Repositories.Categories;
using FUNewsSystem.Infrastructure.Repositories.InvalidatedTokens;
using FUNewsSystem.Infrastructure.Repositories.NewsArticles;
using FUNewsSystem.Infrastructure.Repositories.SystemAccounts;
using FUNewsSystem.Infrastructure.Repositories.Tags;
using FUNewsSystem.Services.AutoMapper;
using FUNewsSystem.Services.Services.Auth;
using FUNewsSystem.Services.Services.Categories;
using FUNewsSystem.Services.Services.Configs;
using FUNewsSystem.Services.Services.HttpContexts;
using FUNewsSystem.Services.Services.NewsArticles;
using FUNewsSystem.Services.Services.SystemAccounts;
using FUNewsSystem.Services.Services.Tags;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

namespace FUNewsSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddOData(options =>
                    options.Select()
                           .Filter()
                           .Count()
                           .OrderBy()
                           .Expand()
                           .SetMaxTop(100)
                           .AddRouteComponents("odata", ODataConfiguration.GetEdmModel())
                )
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "News Swagger API",
                    Description = "An ASP.NET Core Web API for FU News System App",
                    TermsOfService = new Uri("https://example.com/terms"),
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = new Uri("https://example.com/license")
                    }
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
                // using System.Reflection;
                //var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            //Add cors
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            // Add Database configuration
            builder.Services.AddDbContext<FunewsSystemApiDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("FUNewsConnection"));
            });

            // Add Authenticaion and Authorization
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
                        ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:SecretKey"))
                        )
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            var jti = context.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                            var blacklistService = context.HttpContext.RequestServices.GetRequiredService<IBlacklistTokenService>();
                            if (!string.IsNullOrEmpty(jti))
                            {
                                var isBlacklisted = await blacklistService.IsBlacklistedAsync(jti);
                                if (isBlacklisted)
                                {
                                    context.Fail("Token is blacklisted.");
                                }
                            }
                        }
                    };
                });

            builder.Services.AddAuthorization();
            builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();

            // Add Http Context Accessor
            builder.Services.AddHttpContextAccessor();

            // Add Fluent Validation
            builder.Services.AddFluentValidationAutoValidation();

            //Repository
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<ISystemAccountRepository, SystemAccountRepository>();
            builder.Services.AddScoped<IBlacklistTokenRepository, BlacklistTokenRepository>();

            //Service
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<ISystemAccountService, SystemAccountService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IConfigService, ConfigService>();
            builder.Services.AddScoped<IHttpContextService, HttpContextService>();
            builder.Services.AddScoped<IBlacklistTokenService, BlacklistTokenService>();

            //Config automapper
            builder.Services.AddAutoMapper(typeof(CategoryProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(Program));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseGlobalExceptionHandler();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors();


            app.MapControllers();

            app.Run();
        }
    }
}
