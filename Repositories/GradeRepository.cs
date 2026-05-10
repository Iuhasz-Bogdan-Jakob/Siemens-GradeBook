using System.Text.Json;
using Siemens.Internship2026.GradeBook.Interfaces;
using Siemens.Internship2026.GradeBook.Models;

namespace Siemens.Internship2026.GradeBook.Repositories;

public class GradeRepository : IGradeRepository
{
    private readonly HttpClient _httpClient;

    // external endpoint containing the grades data in JSON format
    private readonly string _dataUrl = "https://gist.githubusercontent.com/ArdeleanTudor/8ea407832cd9794960e0e6bbd1319f6e/raw/145";

    //  httpClient inject to handle the external API request
    public GradeRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Grade>> GetAllAsync()
    {
        // GET request to the external source 
        var response = await _httpClient.GetAsync(_dataUrl);
        // exception for http response (if not succesful)
        response.EnsureSuccessStatusCode(); 

        var jsonContent = await response.Content.ReadAsStringAsync();

        // deserialize JSON string into a C# collection
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var grades = JsonSerializer.Deserialize<IEnumerable<Grade>>(jsonContent, options);

        // return only active grades or empty list if fails
        return grades?.Where(g => g.IsActive) ?? Enumerable.Empty<Grade>();
    }

    public async Task<Grade?> GetByIdAsync(int id)
    {
        // fetch all grades and find the first one matching the provided ID
        var allGrades = await GetAllAsync();
        return allGrades.FirstOrDefault(g => g.Id == id);
    }
}