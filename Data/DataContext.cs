
using static System.Net.Mime.MediaTypeNames;

namespace RPG_API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData(
                new Skill() { Id = 1, Name = "Fireball", Damage = 30 },
                new Skill() { Id = 2, Name = "Frenzy", Damage = 20 },
                new Skill() { Id = 3, Name = "Blizzard", Damage = 50 }
            );
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
