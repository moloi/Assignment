namespace CourseEnrollment.Shared.Models;

public class StudentCourse
{
    public string StudentId { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public DateTime EnrolledDate { get; set; }
    public Course Course { get; set; } = null!;
}
