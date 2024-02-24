using API;
using GYM_BE.Core.Dto;
using GYM_BE.ENTITIES;

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
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
