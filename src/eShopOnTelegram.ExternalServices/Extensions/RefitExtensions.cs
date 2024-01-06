using eShopOnTelegram.ExternalServices.Constants;

using Microsoft.Extensions.DependencyInjection;

using Refit;

namespace eShopOnTelegram.ExternalServices.Extensions;

public static class RefitExtensions
{
	public static IHttpClientBuilder AddRefitServiceWithDefaultRetryPolicy<TRefitService>(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureHttpClient)
		where TRefitService : class
	{
		return services
			.AddRefitClient<TRefitService>()
			.ConfigureHttpClient(configureHttpClient)
			.AddPolicyHandlerFromRegistry(HttpPolicyConstants.RetryPolicyKey);
	}
}
