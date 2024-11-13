using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using NetCorePal.D3Shop.Admin.Shared.PermissionConfig;
using Newtonsoft.Json;

namespace NetCorePal.D3Shop.Web.Extensions
{
    public static class AuthConfigurationExtensions
    {
        internal static IServiceCollection AddPermissionAuthorizationServices(this IServiceCollection services)
        {
            services
                .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
                .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            return services;
        }

        internal static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
            AppConfiguration config)
        {
            var key = Encoding.ASCII.GetBytes(config.Secret);
            services
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(bearer =>
                {
                    bearer.RequireHttpsMetadata = false;
                    bearer.SaveToken = true;
                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RoleClaimType = ClaimTypes.Role,
                        ClockSkew = TimeSpan.Zero
                    };

                    bearer.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            // 从 Cookie 中获取 token
                            var token = context.HttpContext.Request
                                .Cookies["authToken"];

                            if (!string.IsNullOrEmpty(token))
                            {
                                context.Token = token;
                            }

                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = c =>
                        {
                            if (c.Exception is SecurityTokenExpiredException)
                            {
                                c.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                c.Response.ContentType = "application/json";
                                var result =
                                    JsonConvert.SerializeObject(new ResponseData(false, "The Token is expired."));
                                return c.Response.WriteAsync(result);
                            }
                            else
                            {
                                c.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                c.Response.ContentType = "application/json";
                                var result =
                                    JsonConvert.SerializeObject(new ResponseData(false,
                                        "An unhandled error has occurred."));
                                return c.Response.WriteAsync(result);
                            }
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            if (context.Response.HasStarted) return Task.CompletedTask;

                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject("You are not Authorized.");
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new ResponseData(false,
                                "You are not authorized to access this resource."));
                            return context.Response.WriteAsync(result);
                        },
                    };
                });

            services.AddAuthorizationBuilder();

            return services;
        }
    }
}