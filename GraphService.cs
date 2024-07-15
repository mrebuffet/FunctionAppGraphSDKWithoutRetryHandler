using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary.Middleware;
using Microsoft.Kiota.Http.HttpClientLibrary.Middleware.Options;

namespace FunctionAppGraphSDKWithoutRetryHandler
{
	public class GraphService
	{
		GraphServiceClient GraphClient { get; set; }

		// The GraphServiceClient instance
		public GraphService()
		{
			// Create a TokenCredential using the access token
			BaseBearerTokenAuthenticationProvider baseBearerTokenProvider = new BaseBearerTokenAuthenticationProvider(new TokenProvider());

			IList<DelegatingHandler>? handlers = GraphClientFactory.CreateDefaultHandlers();

			// Remove the default retry handler
			// Microsoft.Kiota.Http.HttpClientLibrary.Middleware.RetryHandler
			DelegatingHandler? retryHandler = handlers.FirstOrDefault(h => h is RetryHandler);
			if (retryHandler != null)
			{
				handlers.Remove(retryHandler);
			}

			// Add a new one like the ChaosHandler that simulates random server failures
			// Microsoft.Kiota.Http.HttpClientLibrary.Middleware.ChaosHandler
			// handlers.Add(new ChaosHandler(new ChaosHandlerOption {ChaosPercentLevel = 50}));

			HttpClient? httpClient = GraphClientFactory.Create(handlers);
			this.GraphClient = new GraphServiceClient(httpClient, baseBearerTokenProvider);
		}

		public async Task<User?> GetMeAsync()
		{
			return await this.GraphClient.Me.GetAsync(requestConfiguration => requestConfiguration.QueryParameters.Select = new string[] { "id", "displayName", "mail" });
		}
	}

	public class TokenProvider : IAccessTokenProvider
	{
		public Task<string> GetAuthorizationTokenAsync(Uri uri, Dictionary<string, object> additionalAuthenticationContext = default, CancellationToken cancellationToken = default)
		{
			// Visit https://developer.microsoft.com/en-us/graph/graph-explorer to get a token
			string token = "Add your raw token here";
			return Task.FromResult(token);
		}

		public AllowedHostsValidator AllowedHostsValidator { get; }
	}
}
