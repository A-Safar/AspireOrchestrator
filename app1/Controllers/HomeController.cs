using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Diagnostics.Metrics;

namespace BasicOpenTelemetry.Controllers;


public static class DiagnosticsConfig
{
//Resource name for Aspire Dashboard
public const string ServiceName = "BasicOpenTelemetry.API";

public static Meter Meter = new(ServiceName);

//Metric to track the number of students
public static Counter<int> StudentCounter = Meter.CreateCounter<int>("students.count");

}
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}

public sealed record StudentCreateDto
{
    public required string Name { get; init; }
    public required int Age { get; init; }
}


public sealed record StudentUpdateDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public int Age { get; init; }
}
public class StudentDbContext : DbContext
{
    public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
}
[ApiController]
[Route("[controller]")]
public class StudentController(StudentDbContext context, ILogger<StudentController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        logger.LogInformation("Calling external API on http://localhost:5001");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:5001");

        try
        {
            var response = await httpClient.GetAsync("/Home"); // Replace "/api/endpoint" with the actual API path
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            logger.LogInformation("OK");
            return Ok(responseData);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error occurred while calling the external API");
            return Ok(new {name="abdo"});
        }
    }

    [HttpGet("{id}")]
    public Student Get(int id)
    {
        logger.LogInformation("Getting student by id");
        return context.Students.Find(id);
    }

    [HttpPost]
    public Student Post(StudentCreateDto studentCreateDto)
    {
        logger.LogInformation("Creating a new student");
        var student = new Student
        {
            Name = studentCreateDto.Name,
            Age = studentCreateDto.Age
        };
        context.Students.Add(student);
        context.SaveChanges();
        //Code to increment the following metric value by one each time a student is added
        DiagnosticsConfig.StudentCounter.Add(1, new KeyValuePair<string, object>("student.id", student.Id));
        logger.LogInformation("Student created");
        return student;
    }

    [HttpPut]
    public Student Put(StudentUpdateDto studentUpdateDto)
    {
        logger.LogInformation("Updating student");
        var student = context.Students.Find(studentUpdateDto.Id);
        student.Name = studentUpdateDto.Name;
        student.Age = studentUpdateDto.Age;
        context.SaveChanges();
        logger.LogInformation("Student updated");
        return student;
    }

    [HttpDelete("{id}")]
    public Student Delete(int id)
    {
        logger.LogInformation("Deleting student");
        var student = context.Students.Find(id);
        context.Students.Remove(student);
        context.SaveChanges();
        logger.LogInformation("Student deleted");
        return student;
    }


}
