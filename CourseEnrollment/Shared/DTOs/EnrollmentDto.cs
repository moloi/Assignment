namespace CourseEnrollment.Shared.DTOs;

public class EnrollmentDto
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public DateTime EnrolledDate { get; set; }
}
