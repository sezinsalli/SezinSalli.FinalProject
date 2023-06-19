using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Serilog;
using Simpra.Api.Extensions;
using Simpra.Api.Middleware;
using Simpra.Api.Modules;
using Simpra.Service.FluentValidation;
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

// Add DbContext and IdentityUser
builder.Services.AddDbContextExtension(builder.Configuration);

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

app.UseCustomException();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
