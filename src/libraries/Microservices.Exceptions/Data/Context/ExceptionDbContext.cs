using Microservices.Exceptions.Data.Domains;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Microservices.Exceptions.Data.Context
{
    public class ExceptionDbContext : DbContext
    {
        public ExceptionDbContext(DbContextOptions<ExceptionDbContext> options)
              : base(options)
        {

        }

        public DbSet<GlobalExceptionMessage> GlobalExceptionMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
