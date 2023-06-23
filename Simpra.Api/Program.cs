using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Serilog;
using Simpra.Api.Extensions;
using Simpra.Api.Middleware;
using Simpra.Api.Modules;
using Simpra.Core.Logger;
using Simpra.Service.FluentValidation.Category;
using Simpra.Service.Mapper;


var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
Log.Information("Application is starting...");

// Fluent Validation
builder.Services.AddControllers()
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<CategoryUpdateRequestValidator>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MapperProfile));

// DbContext and IdentityUser
builder.Services.AddDbContextExtension(builder.Configuration);

// RabbitMQ and MassTransit
builder.Services.AddRabbitMQExtension(builder.Configuration);

// Redis
builder.Services.AddRedisExtension(builder.Configuration);

// JWT
builder.Services.AddJwtExtension(builder.Configuration);

//Autofac k�t�phanesini y�kledikten sonra kullanmak i�in yaz�yoruz.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

//Buradan Autofac kullanarak yazd���m�z RepoServiceModule'� dahil ediyoruz.
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));

// Serilog kullan�yoruz.
builder.Host.UseSerilog();

var app = builder.Build();

// Otomatik update-database ve admin user seed
app.AddMigrateAndUserSeedDatabase();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Add Auth
app.UseAuthentication();

// Logging and erroring middleware
Action<RequestProfilerModel> requestResponseHandler = requestProfilerModel =>
{
    Log.Information("-------------Request-Begin------------");
    Log.Information(requestProfilerModel.Request);
    Log.Information(Environment.NewLine);
    Log.Information(requestProfilerModel.Response);
    Log.Information("-------------Request-End------------");
};
app.UseMiddleware<RequestLoggingAndErrorHandlerMiddleware>(requestResponseHandler);

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
