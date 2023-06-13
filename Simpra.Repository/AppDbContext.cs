using Simpra.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Simpra.Repository
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<User> Users { get; set; }


        //CreatedDate ve UpdatedDate bilgilerini client'dan bağımsız otomatik olarak yapabilmek için SaveChange metodunu burada eziyoruz. Ef Core Entity Database'e Save edilene kadar Tracking yani takip ediyor. Biz burada Tracking edilen entity'i SaveChange ile database'e kaydetmeden önce araya girerek CreatedDate ya da UpdatedDate bilgilerini gireceğiz. (SaveChangeAsync ve SaveChange için yaptık)
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            entityReference.CreatedAt = DateTime.Now;
                            break;
                        case EntityState.Modified:
                            //Önemli! UpdatedDate'i güncellerken CreatedDate'i default bir değer atıyor bunu yapmaması için aşağıdaki gibi bir tanımalama yaparak şunu diyoruz CreatedDate'e dokunma UpdatedDate'i oluştur.
                            Entry(entityReference).Property(x => x.CreatedAt).IsModified = false;
                            entityReference.UpdatedAt = DateTime.Now;
                            break;
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            entityReference.CreatedAt = DateTime.Now;
                            break;
                        case EntityState.Modified:
                            Entry(entityReference).Property(x => x.CreatedAt).IsModified = false;
                            entityReference.UpdatedAt = DateTime.Now;
                            break;
                    }
                }
            }

            return base.SaveChanges();
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
