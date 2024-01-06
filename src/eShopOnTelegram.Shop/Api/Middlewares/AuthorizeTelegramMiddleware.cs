using System.Collections.Immutable;
using System.Net;
using System.Security.Cryptography;
using System.Text;

using eShopOnTelegram.Shop.Api.Attributes;

namespace eShopOnTelegram.Shop.Api.Middlewares;

public static class AuthorizeTelegramMiddlewareExtensions
{
	public static IApplicationBuilder UseAuthorizeTelegram(
		this IApplicationBuilder builder)
	{
		return builder.UseMiddleware<AuthorizeTelegramMiddleware>();
	}
}

public class AuthorizeTelegramMiddleware
{
	private readonly RequestDelegate _next;

	public AuthorizeTelegramMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
	{
		var endpoint = context.GetEndpoint();
		var enpointMetadata = endpoint?.Metadata;

		var authorizeTelegramAttribute = enpointMetadata?.FirstOrDefault(x => x.GetType() == typeof(AuthorizeTelegramAttribute));

		if (authorizeTelegramAttribute != null)
		{
			var queryString = context.Request.QueryString.Value;
			if (string.IsNullOrWhiteSpace(queryString))
			{
				await ReturnForbiddenResponseAsync(context);
			}

			try
			{
				var queryDictionary = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(queryString);
				var queryParams = queryDictionary.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.FirstOrDefault()).ToImmutableSortedDictionary();

				var dataCheckString = string.Join("\n", "auth_date=" + queryParams["auth_date"], "query_id=" + queryParams["query_id"], "user=" + queryParams["user"]);
				var hashFromTelegram = queryParams["hash"];
				var telegramBotToken = configuration["AppSettings:TelegramBotSettings:Token"];

				var secretKey = HMAC_SHA256(Encoding.UTF8.GetBytes("WebAppData"), Encoding.UTF8.GetBytes(telegramBotToken));
				var calculatedHash = HMAC_SHA256(secretKey, Encoding.UTF8.GetBytes(dataCheckString));
				var calculatedHashHex = BitConverter.ToString(calculatedHash).Replace("-", string.Empty).ToLower();

				if (!string.Equals(hashFromTelegram, calculatedHashHex, StringComparison.OrdinalIgnoreCase))
				{
					await ReturnForbiddenResponseAsync(context);
				}
			}
			catch (KeyNotFoundException)
			{
				await ReturnForbiddenResponseAsync(context);
			}
		}

		await _next(context);
	}

	private static byte[] HMAC_SHA256(byte[] key, byte[] data)
	{
		using HMACSHA256 hmac = new HMACSHA256(key);
		return hmac.ComputeHash(data);
	}

	private async Task ReturnForbiddenResponseAsync(HttpContext context)
	{
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
		await context.Response.StartAsync();
	}

}
