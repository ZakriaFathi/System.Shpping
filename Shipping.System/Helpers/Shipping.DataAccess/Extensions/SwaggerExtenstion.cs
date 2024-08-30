using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Shipping.DataAccess.Extensions;

public enum AuthorizationMethod
    {
        Bearer,
        ApiKey,
        Both
    }

    public static class SwaggerExtenstion
    {

        /*
    * "swagger": {
    "name": "v1",
    "title": "Api Management",
    "version": "v1",
    "IsActive": true,
    "BaseUrl": "/swagger/v1/swagger.json",
    },
    */

        public static IServiceCollection AddSwaggerView(this IServiceCollection services,
            AuthorizationMethod authorizationMethod = AuthorizationMethod.Bearer, string apiKeyName = "")
        {
            var swaggerOptions = new SwaggerOptions();
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                services.Configure<Swashbuckle.AspNetCore.Swagger.SwaggerOptions>(configuration.GetSection("swagger"));

                configuration.GetSection("swagger").Bind(swaggerOptions);
            }

            if (!swaggerOptions.IsActive) return services;

#if (DEBUG || ONLINEDEBUG)

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(swaggerOptions.Version, new OpenApiInfo
                {
                    Title = swaggerOptions.Name,
                    Version = swaggerOptions.Version,
                    Description = swaggerOptions.Title,
                    Contact = new OpenApiContact
                    {
                        Name = "Backenders"
                    },
                });

                var authorizationMethodType = authorizationMethod switch
                {
                    AuthorizationMethod.ApiKey => AddApiKey(apiKeyName, options),
                    AuthorizationMethod.Bearer => AddBearerToken(options),
                    AuthorizationMethod.Both => AddBearerTokenAndApiKey(apiKeyName, options),
                    _ => false
                };

                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                options.CustomSchemaIds(type => $"{type.Name}_{Guid.NewGuid()}");
                //options.IncludeXmlComments(xmlPath);
                options.EnableAnnotations();
                // options.SchemaFilter<EnumSchemaFilter>();
                options.OperationFilter<AcceptLanguageHeaderFilter>();

            });
            services.AddSwaggerGenNewtonsoftSupport();

#endif
            return services;
        }

        private static bool AddBearerTokenAndApiKey(string apiKeyName, SwaggerGenOptions options)
            => AddApiKey(apiKeyName, options) && AddBearerToken(options);

        private static bool AddApiKey(string apiKeyName, SwaggerGenOptions options)
        {
            options.AddSecurityDefinition(apiKeyName, new OpenApiSecurityScheme
            {
                Description = "Api key needed to access the endpoints. X-Api-Key: My_API_Key",
                In = ParameterLocation.Header,
                Name = apiKeyName,
                Type = SecuritySchemeType.ApiKey
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Name = apiKeyName,
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = apiKeyName
                    },
                },
                new string[] { }
            }
        });
            return true;
        }

        private static bool AddBearerToken(SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
            return true;
        }

        public static IApplicationBuilder UseSwaggerView(this IApplicationBuilder app)
        {
            var swaggerOptions = new SwaggerOptions();

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var configuration = serviceScope.ServiceProvider.GetService<IConfiguration>();
                configuration.GetSection("swagger").Bind(swaggerOptions);
            }

            if (!swaggerOptions.IsActive) return app;

#if (DEBUG || ONLINEDEBUG)
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DocumentTitle = swaggerOptions.Title;
                options.DocExpansion(DocExpansion.None);
                options.SwaggerEndpoint(
                    string.IsNullOrEmpty(swaggerOptions.BaseUrl) ? "/swagger/v1/swagger.json" : swaggerOptions.BaseUrl,
                    swaggerOptions.Name);
                options.InjectStylesheet("/swagger-custom.css");
                options.DefaultModelExpandDepth(6);
                options.DefaultModelRendering(ModelRendering.Example);
                options.DefaultModelsExpandDepth(2);
                options.DisplayOperationId();
                options.DisplayRequestDuration();
                options.EnableDeepLinking();
                options.EnableFilter();
                options.ShowExtensions();
                options.ShowCommonExtensions();
                options.EnableValidator();

                options.EnableTryItOutByDefault();
                options.UseRequestInterceptor("(request) => { return request; }");
                options.UseResponseInterceptor("(response) => { return response; }");
            });

            app.UseReDoc(options =>
            {
                options.DocumentTitle = swaggerOptions.Title;
                options.SpecUrl = "/swagger/v1/swagger.json";
            });

#endif
            return app;
        }

        public class EnumSchemaFilter : ISchemaFilter
        {
            public void Apply(OpenApiSchema model, SchemaFilterContext context)
            {
                if (context.Type.IsEnum)
                {
                    model.Enum.Clear();
                    Enum.GetNames(context.Type)
                        .ToList()
                        .ForEach(name => model.Enum.Add(new OpenApiString(Enum.Parse(context.Type, name).ToString())));
                }
            }
        }

        public class AcceptLanguageHeaderFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    In = ParameterLocation.Query,
                    Name = "culture",
                    Description = "pass the locale here: examples like => (ar-LY , en-US)",
                    Schema = new OpenApiSchema
                    {
                        Type = "String",
                        Default = new OpenApiString("ar-LY"),
                        Example = new OpenApiString("ar-LY")
                    },
                });
            }
        }
    }



    public class SwaggerOptions
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }
        public string BaseUrl { get; set; }
        public bool IsActive { get; set; }
    }
