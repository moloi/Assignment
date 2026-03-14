using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CourseEnrollment.Shared.Models;

namespace CourseEnrollment.API.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Course> Courses { get; set; }
    public DbSet<StudentCourse> StudentCourses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<StudentCourse>()
            .HasKey(sc => new { sc.StudentId, sc.CourseId });

        builder.Entity<StudentCourse>()
            .HasOne(sc => sc.Course)
            .WithMany(c => c.StudentCourses)
            .HasForeignKey(sc => sc.CourseId);

        builder.Entity<Course>().HasData(
            new Course { Id = 1, Name = "Introduction to C#", Description = "Learn C# fundamentals", Code = "CS101", Credits = 3, MaxStudents = 30 },
            new Course { Id = 2, Name = "ASP.NET Core", Description = "Build web applications with ASP.NET Core", Code = "CS201", Credits = 4, MaxStudents = 25 },
            new Course { Id = 3, Name = "Blazor WebAssembly", Description = "Modern web development with Blazor", Code = "CS301", Credits = 4, MaxStudents = 20 },
            new Course { Id = 4, Name = "Entity Framework Core", Description = "Database access with EF Core", Code = "CS202", Credits = 3, MaxStudents = 25 },
            new Course { Id = 5, Name = "Azure Fundamentals", Description = "Cloud computing with Microsoft Azure", Code = "AZ100", Credits = 3, MaxStudents = 35 }
        );
    }
}
