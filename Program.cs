using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger config: thêm ô nhập API Key (X-API-KEY)
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key needed to access the endpoints. Example: \"X-API-KEY: {your_key}\"",
        Name = "X-API-KEY", // header name
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKey"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                Scheme = "ApiKey",
                Name = "X-API-KEY",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// Register middleware
builder.Services.AddSingleton<ApiKeyMiddleware>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Custom middleware
app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();

app.Run();