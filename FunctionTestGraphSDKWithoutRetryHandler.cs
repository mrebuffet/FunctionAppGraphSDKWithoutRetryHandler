using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models;

namespace FunctionAppGraphSDKWithoutRetryHandler
{
	public class FunctionTestGraphSDKWithoutRetryHandler(ILogger<FunctionTestGraphSDKWithoutRetryHandler> logger)
	{
		[Function("FunctionTestGraphSDKWithoutRetryHandler")]
		public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
		{
			logger.LogInformation("C# HTTP trigger function processed a request.");
			GraphService graphService = new GraphService();
			User? myUserDetails = await graphService.GetMeAsync();
			return new OkObjectResult($"Welcome to Azure Functions { myUserDetails?.DisplayName ?? "Unknown user" }!");
		}
	}
}
