using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Azure.Identity;

namespace CustomMSGraphProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        static readonly IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        [HttpGet]
        public async Task<string?> GetTaskAsync(string userId)
        {
            var graphClient = GetGraphClient();

            var result = await graphClient.Users[userId].GetAsync((requestConfiguration) =>
            {
                requestConfiguration.QueryParameters.Select =
                    new string[] { "id", "displayName", "employeeId" };
            });
            return result?.EmployeeId?.ToString();
        }

        [HttpPatch]
        public async Task PatchAsync(string userId, string empId)
        {
            var graphClient = GetGraphClient();

            User requestBody = new()
            {
                AdditionalData = new Dictionary<string, object>
                {
                    {"employeeId", empId}
                }
            };
            await graphClient.Users[userId].PatchAsync(requestBody);
        }

        public static GraphServiceClient GetGraphClient()
        {
            var tenantId = configuration["tenantId"];
            var clientId = configuration["clientId"];
            var clientSecretValue = configuration["clientSecretValue"];
            var scopes = new[] { "https://graph.microsoft.com/.default" };

            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            var clientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecretValue, options);

            return new GraphServiceClient(clientSecretCredential, scopes);
        }
    }
}