using Siemens.Internship2026.GradeBook.Interfaces;
using Siemens.Internship2026.GradeBook.Models;

namespace Siemens.Internship2026.GradeBook.Services;

public class GradeService : IGradeService
{
    // service layer depends on the repository, not directly on the database/http client
    private readonly IGradeRepository _repository;

    public GradeService(IGradeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Grade>> GetAllGradesAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Grade?> GetGradeByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Grade>> GetPassingGradesAsync(int n)
    {
        var allGrades = await _repository.GetAllAsync();

        // filter grades
        return allGrades
            .Where(g => g.IsActive && g.Value >= 5)
            .Take(n);
    }

    public decimal CalculateAverage(IEnumerable<Grade> grades)
    {
        // calculate the average to prevent divide by zero exception if the list is empty
        return grades.Any() ? grades.Average(g => g.Value) : 0;
    }
}