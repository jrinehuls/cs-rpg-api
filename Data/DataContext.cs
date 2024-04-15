
namespace RPG_API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        // Name of table is property name
        // public DbSet<Character> Character { get; set; }
        // Getter returns set of characters, no setter, I think.
        public DbSet<Character> Character => Set<Character>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Weapon> Weapon => Set<Weapon>();
        public DbSet<Skill> Skill => Set<Skill>();
    }
}
