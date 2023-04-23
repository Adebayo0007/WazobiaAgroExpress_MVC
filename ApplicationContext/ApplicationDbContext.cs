using Agro_Express.Models;
using Microsoft.EntityFrameworkCore;

namespace Agro_Express.ApplicationContext
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options)
        {

        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<RequestedProduct> RequestedProducts { get; set; }
        public DbSet<User> Users { get; set; }

           protected override void OnModelCreating (ModelBuilder modelBuilder)
        {

                modelBuilder.Entity<User>().HasData(
                  new User
                {
                       Id = "37846734-732e-4149-8cec-6f43d1eb3f60",
                       Role = "Admin",
                       IsActive = true,
                       Password = BCrypt.Net.BCrypt.HashPassword("Admin0001"),
                       UserName = "Modrator",
                       Name = "Adebayo Addullah",
                       PhoneNumber = "08087054632",
                       Gender =  Enum.Gender.Male,
                       Email = "tijaniadebayoabdllahi@gmail.com",
                       DateCreated = DateTime.Now,
                       IsRegistered = true,
                       Haspaid = true,
                       Due = true
                    
                }
              );

            

                 modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = "37846734-732e-4149-8cec-6f43d1eb3f60",
                    UserId = "37846734-732e-4149-8cec-6f43d1eb3f60",
              
                }
            );
        }

        
    }
}