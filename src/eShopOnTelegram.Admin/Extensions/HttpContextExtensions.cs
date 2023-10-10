namespace eShopOnTelegram.Admin.Extensions;

public static class HttpContextExtensions
{
    public static string? IpAddress(this HttpContext httpContext)
    {
        return httpContext.Request.Headers.ContainsKey("X-Forwarded-For")
            ? httpContext.Request.Headers["X-Forwarded-For"]
            : httpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
    }
}
