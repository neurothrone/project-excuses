using Excuses.Persistence.EFCore.Data;
using Excuses.Persistence.EFCore.Repositories;
using Excuses.Persistence.Shared.Interfaces;
using Excuses.WebApi.Server.Endpoints;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApiDbContext>(options =>
    {
        // !: For deploying on AWS add SqliteConnection to connection strings in the following format:
        // "SqliteConnection": "Data Source=/app/data/excuses.db"

        // !: SQLite is used for local development
        options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection") ?? "Data Source=excuses.db");

        // !: SQL Server is used for production
        // options.UseSqlServer(
        //     builder.Configuration.GetConnectionString("AzureConnection") ?? throw new InvalidOperationException(
        //         $"Connection string 'AzureConnection' not found.")
        // );

#if DEBUG
        options.EnableSensitiveDataLogging();
#endif
    }
);

builder.Services.AddScoped<IExcuseRepository, ExcuseEfCoreRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Excuses API",
            Description = "Making excuses for you since 4000 BC.",
            Version = "v1"
        }
    );

    // Customize ordering for tags
    options.TagActionsBy(api =>
        api.HttpMethod is not null ? new List<string> { api.HttpMethod.ToUpper() } : new List<string> { "Other" });
    options.OrderActionsBy(apiDesc => apiDesc.GroupName switch
        {
            "POST" => "1",
            "GET" => "2",
            "PUT" => "3",
            "DELETE" => "4",
            _ => "5"
        }
    );
});

var app = builder.Build();

await EnsureDatabaseIsMigrated(app.Services);

async Task EnsureDatabaseIsMigrated(IServiceProvider services)
{
    using var scope = services.CreateScope();
    await using var context = scope.ServiceProvider.GetService<ApiDbContext>();
    if (context is not null)
        await context.Database.MigrateAsync();
}

// Use CORS
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// NOTE: This is not recommended for production
app.UseHttpsRedirection();

app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (exception is BadHttpRequestException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { error = "Invalid excuse data." });
            return;
        }

        // Handle other unexpected errors
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
    });
});

app.UseStaticFiles();
app.MapGet("/", async context => context.Response.Redirect("/index.html"));

app.MapExcuseEndpoints();

app.Run();