using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Simpra.Core.Entity;
using Simpra.Core.Role;
using Simpra.Repository;

namespace Simpra.Api.Extensions
{
    public static class MigrateAndSeedUserExtension
    {
        public static void AddMigrateAndUserSeedDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                var applicationDbContext = serviceProvider.GetRequiredService<AppDbContext>();
                //Veritabanına yaptığımız migrationları yansıtacağız. Daha uygulama ayağa kalkerken.update-database komutunu unutabileceğimiz durumlarda oldukça faydalıdır.
                applicationDbContext.Database.Migrate();

                //Eğer kullanıcı yok ise admin kullanıcı oluşturması için aşağıda UserManager nesnesini türettik.
                var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!roleManager.RoleExistsAsync(Role.Admin).Result)
                {
                    var userRole = new IdentityRole(Role.Admin);
                    roleManager.CreateAsync(userRole).Wait();
                }

                if (!userManager.Users.Any())
                {
                    var adminUser = new AppUser
                    {
                        UserName = "admin",
                        FirstName = "admin",
                        LastName = "admin",
                        Email = "admin@gmail.com",
                        PhoneNumber = "053381172764",
                        EmailConfirmed = true,
                        TwoFactorEnabled = true,
                        CreatedBy = "admin",
                        CreatedAt = DateTime.Now
                    };

                    //Database de kullanıcı yok ise default olarak aşağıdaki kullanıcıyı ekliyoruz. Wait ile senkrona çevirdik.
                    userManager.CreateAsync(adminUser, "Password12*").Wait();
                    userManager.AddToRoleAsync(adminUser, Role.Admin).Wait();
                }
            }
        }
    }
}
