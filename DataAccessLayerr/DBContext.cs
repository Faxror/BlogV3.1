using EntityLayerr.Concrate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete
{
    public class DBContext : IdentityDbContext<AppUser, AppRole, int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BlogV3;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

        public DbSet<Blogs> Blogss { get; set; }
        public DbSet<Author> Authors { get; set; }
    }
}
