using Microsoft.EntityFrameworkCore;
using Mini_Project_Entitiy_Framework_.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Project_Entitiy_Framework_.Persistence.Data
{
    internal class LibraryDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("server=localhost\\SQLEXPRESS;database=LibraryDbOnline;trusted_connection=true;integrated security=true;trustserversertificate=true;");

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<ReservedItem> ReservedItems => Set<ReservedItem>();

    }
}
