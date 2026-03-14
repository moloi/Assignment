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
public class CoursesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CoursesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
    {
        var courses = await _context.Courses
            .Include(c => c.StudentCourses)
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

        return Ok(courses);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CourseDto>> GetCourse(int id)
    {
        var course = await _context.Courses
            .Include(c => c.StudentCourses)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course == null)
            return NotFound();

        return Ok(new CourseDto
        {
            Id = course.Id,
            Name = course.Name,
            Description = course.Description,
            Code = course.Code,
            Credits = course.Credits,
            MaxStudents = course.MaxStudents,
            EnrolledStudents = course.StudentCourses.Count
        });
    }

    [HttpPost]
    public async Task<ActionResult<CourseDto>> CreateCourse(CreateCourseDto dto)
    {
        var course = new Course
        {
            Name = dto.Name,
            Description = dto.Description,
            Code = dto.Code,
            Credits = dto.Credits,
            MaxStudents = dto.MaxStudents
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, new CourseDto
        {
            Id = course.Id,
            Name = course.Name,
            Description = course.Description,
            Code = course.Code,
            Credits = course.Credits,
            MaxStudents = course.MaxStudents,
            EnrolledStudents = 0
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(int id, CreateCourseDto dto)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
            return NotFound();

        course.Name = dto.Name;
        course.Description = dto.Description;
        course.Code = dto.Code;
        course.Credits = dto.Credits;
        course.MaxStudents = dto.MaxStudents;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
            return NotFound();

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
