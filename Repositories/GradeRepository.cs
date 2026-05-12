using System.Text.Json;
using Siemens.Internship2026.GradeBook.Interfaces;
using Siemens.Internship2026.GradeBook.Models;

namespace Siemens.Internship2026.GradeBook.Repositories;

public class GradeRepository : IGradeRepository
{
    private readonly HttpClient _httpClient;

    private readonly string _dataUrl = "https://gist.githubusercontent.com/ArdeleanTudor/8ea407832cd9794960e0e6bbd1319f6e/raw/145b121103dd1cee3737a681c487f7295ac82e6b/gistfile1.txt";

    public GradeRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Grade>> GetAllAsync()
    {
        // http request
        var response = await _httpClient.GetAsync(_dataUrl);
        response.EnsureSuccessStatusCode();

        // read the json content
        var jsonContent = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        // deserialize the new wrapper
        var apiResponse = JsonSerializer.Deserialize<ApiResponseWrapper>(jsonContent, options);

        // extract of items list from wrapper and filter it
        var grades = apiResponse?.Items ?? Enumerable.Empty<Grade>();

        return grades.Where(g => g.IsActive);
    }

    public async Task<Grade?> GetByIdAsync(int id)
    {
        var allGrades = await GetAllAsync();
        return allGrades.FirstOrDefault(g => g.Id == id);
    }

    
    // class for the wrapper
    private class ApiResponseWrapper
    {
        public IEnumerable<Grade>? Items { get; set; }
    }
}