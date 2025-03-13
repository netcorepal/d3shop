using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NetCorePal.Extensions.Dto;
using Newtonsoft.Json;

namespace NetCorePal.D3Shop.Web.Extensions;

public static class AuthConfigurationExtensions
{
    internal static IServiceCollection AddAuthenticationSchemes(this IServiceCollection services)
    {
        services.AddAuthentication(authentication =>
            {
                authentication.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddJwtAuthentication()
            .AddCookieAuthentication();

        return services;
    }

    private static AuthenticationBuilder AddJwtAuthentication(this AuthenticationBuilder builder)
    {
        return builder.AddJwtBearer(bearer =>
        {
            bearer.RequireHttpsMetadata = false;
            bearer.SaveToken = true;
            bearer.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero
            };

            bearer.Events = new JwtBearerEvents
            {
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
                }
            };
        });
    }

    private static void AddCookieAuthentication(this AuthenticationBuilder builder)
    {
        builder.AddCookie(options =>
        {
            options.Cookie.HttpOnly = true; // 防止 JavaScript 访问
            options.Cookie.SameSite = SameSiteMode.Lax; // 设置 SameSite 策略
            options.LoginPath = "/admin/login"; // 未认证用户的重定向路径
            options.AccessDeniedPath = "/admin/login"; // 无权限的重定向路径
        });
    }
}