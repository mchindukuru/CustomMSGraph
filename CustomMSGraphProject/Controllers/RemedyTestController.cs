using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CustomMSGraphProject.Model;
using System.Net.Http;
using Azure.Core;
using System.Reflection;
using System.Net.Http.Headers;

namespace CustomMSGraphProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemedyTestController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public RemedyTestController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        private readonly string jsonFilePath = "incidents.json";

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIncident(string id)
        {
            try
            {
                // Read existing data from the JSON file
                var existingData = await ReadJsonFileAsync<List<IncidentModel>>(jsonFilePath) ?? new List<IncidentModel>();

                // Find the incident with the specified ID
                var incident = existingData.Find(i => i.Incident_id == id);

                if (incident == null)
                {
                    return NotFound($"Incident with ID {id} not found.");
                }

                // Return the incident data in the response
                return Ok(incident);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateIncident([FromBody] IncidentModel incident)
        {
            try
            {
                // Read existing data from the JSON file
                var existingData = await ReadJsonFileAsync<List<IncidentModel>>(jsonFilePath) ?? new List<IncidentModel>();

                // Add the new incident to the list
                existingData.Add(incident);

                //using var httpClient = _httpClientFactory.CreateClient();
                //string remedyApiBaseUrl = "https://localhost:7229/api/WeatherForecast";
                //httpClient.DefaultRequestHeaders
                //    .Accept
                //    .Add(new MediaTypeWithQualityHeaderValue("application/string"));

                //// Convert model to JSON
                //var jsonPayload = JsonConvert.SerializeObject(incident);

                // Send POST request to create incident
                //var response = await httpClient.PostAsync($"{remedyApiBaseUrl}", new StringContent(jsonPayload));

                // Ensure the request was successful (status code 2xx)
                //response.EnsureSuccessStatusCode();

                // Write the updated data back to the JSON file
                await WriteJsonFileAsync(existingData, jsonFilePath);

                return Ok("Incident created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private static async Task<T?> ReadJsonFileAsync<T>(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                var jsonData = await System.IO.File.ReadAllTextAsync(filePath);
                return JsonConvert.DeserializeObject<T>(jsonData);
            }

            return default;
        }

        private static async Task WriteJsonFileAsync<T>(T data, string filePath)
        {
            var jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            await System.IO.File.WriteAllTextAsync(filePath, jsonData);
        }
    }
}