using Domains.Entities;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace CoreInfrastructure.DataContext
{
    public class MyDataContext : BaseDbContext, IDataContext
    {
        public MyDataContext(DbContextOptions<MyDataContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>().HasQueryFilter(x => !x.IsDeleted);
            // Configure relationships, indexes, and other model customizations here
        }
    }
}
