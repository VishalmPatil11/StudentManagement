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
        }
    }
}
