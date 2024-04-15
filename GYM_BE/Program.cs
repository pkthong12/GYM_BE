using API;
using GYM_BE.Core.Dto;
using GYM_BE.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

// Add services to the container.

services.Configure<AppSettings>(config.GetSection("AppSettings"));
var _appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            // If the request is for our hub...
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/hubs/signal")))
            {
                // Read the token out of the query string
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});


#region DbContexts
services.AddDbContext<FullDbContext>(options => options.UseSqlServer());
#endregion DbContexts

services.AddRouting(o => o.LowercaseQueryStrings = true);
services.AddCors(options =>
{
    /* Latter, in Production, we need to add specific policy */
    options.AddPolicy("Development",
          builder =>
              builder
                .AllowAnyHeader()
                .WithExposedHeaders("X-Message-Code", "Content-Type")
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed(_ => true)
          );
});
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    //Disable The Default
    options.SuppressModelStateInvalidFilter = true;
})
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new SnackToCamelCaseContractResolver();
    }); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("system", new OpenApiInfo { Title = "SYSTEM API", Version = "v1" });
    c.SwaggerDoc("person", new OpenApiInfo { Title = "PERSON API", Version = "v1" });
    c.SwaggerDoc("locker", new OpenApiInfo { Title = "LOCKER API", Version = "v1" });
    c.SwaggerDoc("goods", new OpenApiInfo { Title = "GOODS API", Version = "v1" });
    c.TagActionsBy(api =>
    {
        if (api.GroupName != null)
        {
            return new[] { api.GroupName };
        }

        var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
        if (controllerActionDescriptor != null)
        {
            return new[] { controllerActionDescriptor.ControllerName };
        }

        throw new InvalidOperationException("Unable to determine tag for endpoint.");
    });


    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
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

    c.DocInclusionPredicate((name, api) =>
    {

        try
        {
            if (api.GroupName == null)
            {
                return false;
            };
            if (api.GroupName.Length > (4 + name.Length))
            {

                if (api.GroupName.ToLower().Substring(4, name.Length) == name.ToLower())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        catch (SwaggerGeneratorException ex)
        {
            return false;
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/system/swagger.json", "SYSTEM API");
        c.SwaggerEndpoint("/swagger/person/swagger.json", "PERSON API");
        c.SwaggerEndpoint("/swagger/locker/swagger.json", "LOCKER API");
        c.SwaggerEndpoint("/swagger/goods/swagger.json", "GOODS API");
    });
}
/* Latter, in Production, we need to use specific policy */
app.UseCors("Development");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
