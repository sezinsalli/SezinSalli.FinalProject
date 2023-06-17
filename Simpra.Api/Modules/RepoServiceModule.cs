using Autofac;
using Simpra.Core.Repository;
using Simpra.Core.Service;
using Simpra.Core.UnitofWork;
using Simpra.Repository;
using Simpra.Repository.Repository;
using Simpra.Repository.UnitofWork;
using Simpra.Service.Service;
using System.Reflection;
using Module = Autofac.Module;

namespace Simpra.Api.Modules
{
    public class RepoServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(BaseService<>)).As(typeof(IBaseService<>)).InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            var ApiAssembly = Assembly.GetExecutingAssembly();
            var respoAssembly = Assembly.GetAssembly(typeof(AppDbContext));
            var serviceAssembly = Assembly.GetAssembly(typeof(ProductService));

            builder.RegisterAssemblyTypes(ApiAssembly, respoAssembly, serviceAssembly).
                Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ApiAssembly, respoAssembly, serviceAssembly).
                Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
