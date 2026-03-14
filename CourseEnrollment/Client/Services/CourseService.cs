using System.Net.Http.Json;
using CourseEnrollment.Shared.DTOs;

namespace CourseEnrollment.Client.Services;

public interface ICourseService
{
    Task<List<CourseDto>> GetAllCourses();
    Task<List<CourseDto>> GetAvailableCourses();
    Task<List<EnrollmentDto>> GetMyEnrollments();
    Task<bool> EnrollInCourse(int courseId);
    Task<bool> UnenrollFromCourse(int courseId);
}

public class CourseService : ICourseService
{
    private readonly HttpClient _http;

    public CourseService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<CourseDto>> GetAllCourses()
    {
        return await _http.GetFromJsonAsync<List<CourseDto>>("api/courses") ?? new();
    }

    public async Task<List<CourseDto>> GetAvailableCourses()
    {
        return await _http.GetFromJsonAsync<List<CourseDto>>("api/enrollments/available-courses") ?? new();
    }

    public async Task<List<EnrollmentDto>> GetMyEnrollments()
    {
        return await _http.GetFromJsonAsync<List<EnrollmentDto>>("api/enrollments/my-courses") ?? new();
    }

    public async Task<bool> EnrollInCourse(int courseId)
    {
        var response = await _http.PostAsync($"api/enrollments/enroll/{courseId}", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UnenrollFromCourse(int courseId)
    {
        var response = await _http.DeleteAsync($"api/enrollments/unenroll/{courseId}");
        return response.IsSuccessStatusCode;
    }
}
