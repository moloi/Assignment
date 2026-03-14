using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseEnrollment.API.Data;
using CourseEnrollment.Shared.DTOs;
using CourseEnrollment.Shared.Models;

namespace CourseEnrollment.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EnrollmentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("my-courses")]
    public async Task<ActionResult<IEnumerable<EnrollmentDto>>> GetMyEnrollments()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var enrollments = await _context.StudentCourses
            .Include(sc => sc.Course)
            .Where(sc => sc.StudentId == userId)
            .Select(sc => new EnrollmentDto
            {
                CourseId = sc.CourseId,
                CourseName = sc.Course.Name,
                CourseCode = sc.Course.Code,
                EnrolledDate = sc.EnrolledDate
            })
            .ToListAsync();

        return Ok(enrollments);
    }

    [HttpGet("available-courses")]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetAvailableCourses()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var enrolledCourseIds = await _context.StudentCourses
            .Where(sc => sc.StudentId == userId)
            .Select(sc => sc.CourseId)
            .ToListAsync();

        var availableCourses = await _context.Courses
            .Include(c => c.StudentCourses)
            .Where(c => !enrolledCourseIds.Contains(c.Id))
            .Select(c => new CourseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Code = c.Code,
                Credits = c.Credits,
                MaxStudents = c.MaxStudents,
                EnrolledStudents = c.StudentCourses.Count
            })
            .ToListAsync();

        return Ok(availableCourses);
    }

    [HttpPost("enroll/{courseId}")]
    public async Task<IActionResult> EnrollInCourse(int courseId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var course = await _context.Courses
            .Include(c => c.StudentCourses)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
            return NotFound("Course not found");

        var alreadyEnrolled = await _context.StudentCourses
            .AnyAsync(sc => sc.StudentId == userId && sc.CourseId == courseId);

        if (alreadyEnrolled)
            return BadRequest("Already enrolled in this course");

        if (course.StudentCourses.Count >= course.MaxStudents)
            return BadRequest("Course is full");

        var enrollment = new StudentCourse
        {
            StudentId = userId!,
            CourseId = courseId,
            EnrolledDate = DateTime.UtcNow
        };

        _context.StudentCourses.Add(enrollment);
        await _context.SaveChangesAsync();

        return Ok("Successfully enrolled");
    }

    [HttpDelete("unenroll/{courseId}")]
    public async Task<IActionResult> UnenrollFromCourse(int courseId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var enrollment = await _context.StudentCourses
            .FirstOrDefaultAsync(sc => sc.StudentId == userId && sc.CourseId == courseId);

        if (enrollment == null)
            return NotFound("Enrollment not found");

        _context.StudentCourses.Remove(enrollment);
        await _context.SaveChangesAsync();

        return Ok("Successfully unenrolled");
    }
}
