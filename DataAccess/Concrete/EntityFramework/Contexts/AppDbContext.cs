using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Concrete.EntityFramework.Contexts
{
    /// <summary>
    /// Because this context is followed by migration for more than one provider
    /// works on PostGreSql db by default. If you want to pass sql
    /// When adding AddDbContext, use MsDbContext derived from it.
    /// </summary>
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        /// <summary>
        /// in constructor we get IConfiguration, parallel to more than one db
        /// we can create migration.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="configuration"></param>
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        public AppDbContext()
        {

        }

        /// <summary>
        /// Let's also implement the general version.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="configuration"></param>
        protected AppDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //todo: config ile oku
                optionsBuilder.UseSqlServer("data source=.;initial catalog=Northwind;Trusted_Connection=true;");
            }
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }

        //public DbSet<UserClaim> UserClaims { get; set; }
        //public DbSet<Group> Groups { get; set; }
        //public DbSet<UserGroup> UserGroups { get; set; }
        //public DbSet<User> Users { get; set; }
        //public DbSet<GroupClaim> GroupClaims { get; set; }
        //public DbSet<Log> Logs { get; set; }
        //public DbSet<MobileLogin> MobileLogins { get; set; }
        //public DbSet<Language> Languages { get; set; }
        //public DbSet<Translate> Translates { get; set; }
    }
}