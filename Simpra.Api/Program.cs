using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Simpra.Api.Extensions;
using Simpra.Api.Middleware;
using Simpra.Api.Modules;
using Simpra.Repository;
using Simpra.Schema.Mapper;
using Simpra.Service.FluentValidation;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<CategoryUpdateRequestValidator>());

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


// Add Redis
builder.Services.AddRedisExtension(builder.Configuration);


//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
//builder.Services.AddScoped<ICategoryService, CategoryService>();

//builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));



//builder.Services.AddScoped<IProductRepository, ProductRepository>();
//builder.Services.AddScoped<IProductService, ProductService>();
//Autofac kütüphanesini yükledikten sonra kullanmak için yazýyoruz.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

//Buradan Autofac kullanarak yazdýðýmýz RepoServiceModule'ü dahil ediyoruz.
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();
app.UseCustomException();

app.UseAuthorization();

app.MapControllers();


app.Run();
