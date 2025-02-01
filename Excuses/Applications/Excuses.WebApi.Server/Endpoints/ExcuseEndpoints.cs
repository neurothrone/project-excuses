using Excuses.Persistence.Shared.DTO;
using Excuses.Persistence.Shared.Interfaces;
using Excuses.Persistence.Shared.Models;

namespace Excuses.WebApi.Server.Endpoints;

public static class ExcuseEndpoints
{
    public static void MapExcuseEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/v1/excuses");

        group.MapPost("", async (ExcuseInputDto excuse, IExcuseRepository repository) =>
            {
                try
                {
                    var createdExcuse = await repository.CreateExcuseAsync(excuse);
                    return createdExcuse is null
                        ? Results.BadRequest("Failed to create excuse.")
                        : Results.Created($"api/excuses/{createdExcuse.Id}", createdExcuse);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }
            })
            .WithSummary("Create a new excuse.")
            .Produces<Excuse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapGet("", async (string? category, IExcuseRepository repository) =>
                !string.IsNullOrEmpty(category)
                    ? Results.Ok(await repository.GetExcusesByCategoryAsync(category))
                    : Results.Ok(await repository.GetExcusesAsync())
            )
            .WithSummary("Get all excuses, optionally filtered by category.");

        group.MapGet("/{id:int:min(0)}", async (int id, IExcuseRepository repository) =>
            {
                var excuse = await repository.GetExcuseAsync(id);
                return excuse is not null ? Results.Ok(excuse) : Results.NotFound();
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get an excuse by its ID.");

        group.MapGet("random", async (string? category, IExcuseRepository repository) =>
                {
                    var excuse = !string.IsNullOrEmpty(category)
                        ? await repository.GetRandomExcuseByCategoryAsync(category)
                        : await repository.GetRandomExcuseAsync();
                    return excuse is not null ? Results.Ok(excuse) : Results.NotFound();
                }
            )
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get a random excuse, optionally filtered by category.");

        group.MapGet("/categories", async (IExcuseRepository repository) =>
                Results.Json(await repository.GetCategoriesAsync())
            )
            .WithSummary("Get all excuse categories.");

        group.MapPut("/{id:int:min(0)}", async (int id, ExcuseInputDto excuse, IExcuseRepository repository) =>
                {
                    var updated = await repository.UpdateExcuseAsync(id, excuse);
                    return updated ? Results.NoContent() : Results.NotFound();
                }
            )
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Update an excuse by its ID.");

        group.MapDelete("/{id:int:min(0)}", async (int id, IExcuseRepository repository) =>
            {
                var deleted = await repository.DeleteExcuseAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Delete an excuse by its ID.");
    }
}