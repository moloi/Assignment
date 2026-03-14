namespace CourseEnrollment.Shared.DTOs;

public class CreateCourseDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int MaxStudents { get; set; }
}
