using eShopOnTelegram.ExternalServices.Constants;

using Polly;
using Polly.Extensions.Http;
using Polly.Registry;

namespace eShopOnTelegram.ExternalServices.Extensions;

public static class PolicyRegistryExtensions
{
    public static IPolicyRegistry<string> AddHttpRetryPolicy(this IPolicyRegistry<string> policyRegistry)
    {
        policyRegistry.Add(
            HttpPolicyConstants.RetryPolicyKey,
            HttpPolicyExtensions.HandleTransientHttpError().RetryAsync(3)
        );

        return policyRegistry;
    }
}
