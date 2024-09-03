using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Shipping.Domain.Entities;
using Shipping.Utils.Enums;

namespace Shipping.DataAccess.Extensions;

public static class ExtensionsAuthentications
{
    public static void AddAuthentications(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                };
                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject("401 Unauthorized");
                        return context.Response.WriteAsync(result);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject("403 Forbidden");
                        return context.Response.WriteAsync(result);
                    },
                };

            });

        services.AddAuthorization(options =>
        {
            #region UserManagement

            options.AddPolicy("UserManagementCreate", policyUser =>
            {
                policyUser.RequireRole("Employee", "Owner");
                policyUser.RequireClaim(RoleType.UserManagement.ToString("G"), 
                    PermissionNames.Create.ToString("G")
                ); 
            });  
            options.AddPolicy("UserManagementEdit", policyUser =>
            {
                policyUser.RequireRole("Employee", "Owner");
                policyUser.RequireClaim(RoleType.UserManagement.ToString("G"), 
                    PermissionNames.Edit.ToString("G")
                ); 
            }); 
            options.AddPolicy("UserManagementView", policyUser =>
            {
                policyUser.RequireRole("Employee", "Owner");
                policyUser.RequireClaim(RoleType.UserManagement.ToString("G"), 
                    PermissionNames.View.ToString("G")
                ); 
            }); 
            options.AddPolicy("UserManagementDelete", policyUser =>
            {
                policyUser.RequireRole("Employee", "Owner");
                policyUser.RequireClaim(RoleType.UserManagement.ToString("G"), 
                    PermissionNames.Delete.ToString("G")
                ); 
            });

            #endregion

            #region OrderManagement

            options.AddPolicy("OrderManagementView", policyUser =>
            {
                policyUser.RequireRole("Employee", "Owner");
                policyUser.RequireClaim(
                    RoleType.OrderManagement.ToString("G"),
                    PermissionNames.View.ToString("G")
                );
            }); 
            options.AddPolicy("OrderManagementEdit", policyUser =>
            {
                policyUser.RequireRole("Employee", "Owner");
                policyUser.RequireClaim(
                    RoleType.OrderManagement.ToString("G"),
                    PermissionNames.Edit.ToString("G")
                );
            });  
            options.AddPolicy("OrderManagementDelete", policyUser =>
            {
                policyUser.RequireRole("Employee", "Owner");
                policyUser.RequireClaim(
                    RoleType.OrderManagement.ToString("G"),
                    PermissionNames.Delete.ToString("G")
                );
            });

            #endregion

            #region BranchManagement

            options.AddPolicy("BranchManagementCreate", policyUser =>
            {
                policyUser.RequireRole("Employee", "Owner");
                policyUser.RequireClaim(RoleType.BranchManagement.ToString("G"), 
                    PermissionNames.Create.ToString("G")
                ); 
            });  
            options.AddPolicy("BranchManagementEdit", policyUser =>
            {
                policyUser.RequireRole("Employee", "Owner");
                policyUser.RequireClaim(RoleType.BranchManagement.ToString("G"), 
                    PermissionNames.Edit.ToString("G")
                ); 
            }); 
            options.AddPolicy("BranchManagementView", policyUser =>
            {
                policyUser.RequireRole("Employee", "Owner");
                policyUser.RequireClaim(RoleType.BranchManagement.ToString("G"), 
                    PermissionNames.View.ToString("G")
                ); 
            }); 
            options.AddPolicy("BranchManagementDelete", policyUser =>
            {
                policyUser.RequireRole("Employee", "Owner");
                policyUser.RequireClaim(RoleType.BranchManagement.ToString("G"), 
                    PermissionNames.Delete.ToString("G")
                ); 
            });

            #endregion

            #region CityManagement

            options.AddPolicy("CityManagementView", policyUser =>
            {
                policyUser.RequireRole("Employee", "Owner");
                policyUser.RequireClaim(RoleType.CityManagement.ToString("G"), 
                    PermissionNames.View.ToString("G")
                ); 
            });
            options.AddPolicy("CityManagementCreate", policyUser =>
            {
                policyUser.RequireRole("Employee", "Owner");
                policyUser.RequireClaim(RoleType.CityManagement.ToString("G"), 
                    PermissionNames.Create.ToString("G")
                ); 
            }); 
            options.AddPolicy("CityManagementEdit", policyUser =>
            {
                policyUser.RequireRole("Employee", "Owner");
                policyUser.RequireClaim(RoleType.CityManagement.ToString("G"), 
                    PermissionNames.Edit.ToString("G")
                ); 
            });   
            options.AddPolicy("CityManagementDelete", policyUser =>
            {
                policyUser.RequireRole("Employee", "Owner");
                policyUser.RequireClaim(RoleType.CityManagement.ToString("G"), 
                    PermissionNames.Delete.ToString("G")
                ); 
            });

            #endregion

            #region PermissionManagement

            options.AddPolicy("PermissionManagementView", policyUser =>
            {
                policyUser.RequireRole("Employee", "Owner");
                policyUser.RequireClaim(RoleType.PermissionManagement.ToString("G"), 
                    PermissionNames.View.ToString("G")
                ); 
            }); 
            options.AddPolicy("PermissionManagementDelete", policyUser =>
            {
                policyUser.RequireRole("Employee", "Owner");
                policyUser.RequireClaim(RoleType.PermissionManagement.ToString("G"), 
                    PermissionNames.Delete.ToString("G")
                ); 
            });

            #endregion
            
        });
    }
}