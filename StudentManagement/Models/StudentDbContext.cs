using Microsoft.EntityFrameworkCore;

namespace StudentManagement.Models
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext() { }

        public StudentDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer("Data Source=VISHAL;Database=StudentManagement;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_01_studentid");
                entity.ToTable("Student");
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Age);
                entity.Property(e => e.Address);
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Course);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_User_Id");
                entity.ToTable("[User]");
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<LoginLog>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_LoginLog_Id");
                entity.ToTable("LoginLog");
                entity.Property(e => e.Username);
                entity.Property(e => e.Successful);
                entity.Property(e => e.Timestamp).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.IpAddress);
            });
        }
    }
}
