using System;
using Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence
{
    public class DataContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            //create default admin user
            builder.Entity<Account>().HasData(
             new Account
             {
                 Id = 1,
                 FirstName = "Glory",
                 LastName = "Efionayi",
                 Email = "engreseglory@gmail.com",
                 PhoneNumber = "08034441916",
                 Verified = DateTime.Now,
                 Role = Common.Enums.Role.Admin,
                 PasswordHash = ""
             }
         );
            base.OnModelCreating(builder);
        }


        private readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"));
        }
    }
}