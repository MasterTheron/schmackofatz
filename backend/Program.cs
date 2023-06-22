using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Services;
using backend.Authorization;
using backend.Helpers;
using backend.Config;

var builder = WebApplication.CreateBuilder(args);

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://127.0.0.1:9000").AllowAnyHeader().AllowAnyMethod();
                      });
});

builder.Services.Configure<AzureStorageOptions>(builder.Configuration.GetSection(AzureStorageOptions.AzureStorageConfig));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
// builder.Services.Configure<DatabaseConfig>(builder.Configuration.GetSection(DatabaseConfig.DatabaseConfigName));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var dbOptions = builder.Configuration.GetSection(DatabaseConfig.DatabaseConfigName).Get<DatabaseConfig>();
builder.Services.AddDbContext<RecipeContext>(opt =>
    opt.UseCosmos(dbOptions.AccountEndpoint, dbOptions.AccountEndpoint, dbOptions.DatabaseName));

builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors(MyAllowSpecificOrigins);

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

// app.MapFallbackToFile("index.html");

app.Run();
