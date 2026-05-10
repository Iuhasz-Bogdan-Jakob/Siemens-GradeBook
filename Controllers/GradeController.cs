using Microsoft.AspNetCore.Mvc;
using Siemens.Internship2026.GradeBook.Interfaces;

namespace Siemens.Internship2026.GradeBook.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradesController : ControllerBase
{
    // dependecies injected through constructor
    private readonly IGradeService _gradeService;
    private readonly ILogger<GradesController> _logger;

    public GradesController(IGradeService gradeService, ILogger<GradesController> logger)
    {
        _gradeService = gradeService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GET api/grades called");

        // fetch data using business logic layer
        var grades = await _gradeService.GetAllGradesAsync();
        var gradeList = grades.ToList();

        // calculate statistics
        var totalCount = gradeList.Count;
        var averageValue = _gradeService.CalculateAverage(gradeList);

        // return a customized JSON structure containing data and stats
        return Ok(new
        {
            Data = gradeList,
            Statistics = new
            {
                TotalCount = totalCount,
                AverageValue = averageValue,
                RetrievedAt = DateTime.UtcNow
            }
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GET api/grades/{Id} called", id);

        if (id <= 0) return BadRequest("Id must be a positive integer.");

        var grade = await _gradeService.GetGradeByIdAsync(id);
        
        // return 404 status code if the resoursce doesn't exist 
        if (grade == null) return NotFound($"Grade with Id {id} was not found.");

        return Ok(grade);
    }

    [HttpGet("passing/{n}")]
    public async Task<IActionResult> GetPassingGrades(int n)
    {
        _logger.LogInformation("GET api/grades/passing/{N} called", n);

        if (n <= 0) return BadRequest("N must be greater than 0.");

        var passingGrades = await _gradeService.GetPassingGradesAsync(n);

        return Ok(passingGrades);
    }
}