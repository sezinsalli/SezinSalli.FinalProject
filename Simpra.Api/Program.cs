using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Simpra.Api.Extensions;
using Simpra.Api.Middleware;
using Simpra.Api.Modules;
using Simpra.Core.Logger;
using Simpra.Repository;
using Simpra.Service.FluentValidation;
using Simpra.Service.Mapper;
using System.Reflection;

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

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });
});

// Add RabbitMQ and MassTransit
builder.Services.AddRabbitMQExtension(builder.Configuration);

// Add Redis
builder.Services.AddRedisExtension(builder.Configuration);

//Autofac kütüphanesini yükledikten sonra kullanmak için yazýyoruz.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

//Buradan Autofac kullanarak yazdýðýmýz RepoServiceModule'ü dahil ediyoruz.
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));

// Serilog kullanýyoruz.
builder.Host.UseSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseMiddleware<ErrorHandlerMiddleware>();
Action<RequestProfilerModel> requestResponseHandler = requestProfilerModel =>
{
    Log.Information("-------------Request-Begin------------");
    Log.Information(requestProfilerModel.Request);
    Log.Information(Environment.NewLine);
    Log.Information(requestProfilerModel.Response);
    Log.Information("-------------Request-End------------");
};
app.UseMiddleware<RequestLoggingMiddleware>(requestResponseHandler);

//app.UseCustomException();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
