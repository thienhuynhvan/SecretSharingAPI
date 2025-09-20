using System.Security.Claims;

public class ApiKeyMiddleware : IMiddleware
{
    private const string HeaderName = "X-API-KEY";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // Chỉ check với endpoint có AuthorizeApiKeyAttribute
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<AuthorizeApiKeyAttribute>() != null)
        {
            if (!context.Request.Headers.TryGetValue(HeaderName, out var extractedKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key is missing");
                return;
            }

            if (!ApiKeyStore.ValidateKey(extractedKey!))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Invalid API Key");
                return;
            }

            // attach user identity
            var email = ApiKeyStore.GetUserByKey(extractedKey!);
            var claims = new[] { new Claim(ClaimTypes.Name, email) };
            context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "ApiKey"));
        }

        await next(context);
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeApiKeyAttribute : Attribute { }