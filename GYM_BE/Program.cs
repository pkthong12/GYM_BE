using API;
using GYM_BE.Core.Dto;
using GYM_BE.Entities;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

services.Configure<AppSettings>(config.GetSection("AppSettings"));
var _appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();

// Add services to the container.


#region DbContexts
services.AddDbContext<FullDbContext>();
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
    });
}
/* Latter, in Production, we need to use specific policy */
app.UseCors("Development");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
