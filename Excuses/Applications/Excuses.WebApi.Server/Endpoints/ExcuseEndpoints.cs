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
                var result = await repository.CreateExcuseAsync(excuse);
                return result.Match(
                    onSuccess: createdExcuse => Results.Created($"api/excuses/{createdExcuse.Id}", createdExcuse),
                    onFailure: error => Results.BadRequest(error)
                );
            })
            .WithSummary("Create a new excuse.")
            .Produces<Excuse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapGet("", async (string? category, IExcuseRepository repository) =>
                {
                    var result = !string.IsNullOrEmpty(category)
                        ? await repository.GetExcusesByCategoryAsync(category)
                        : await repository.GetExcusesAsync();
                    return result.Match(
                        onSuccess: excuses => Results.Ok(excuses),
                        onFailure: error =>
                            Results.Problem(detail: error, statusCode: StatusCodes.Status500InternalServerError)
                    );
                }
            )
            .Produces<List<Excuse>>()
            .WithSummary("Get all excuses, optionally filtered by category.");

        group.MapGet("/{id:int:min(0)}", async (int id, IExcuseRepository repository) =>
            {
                var result = await repository.GetExcuseAsync(id);
                return result.Match(
                    onSuccess: excuse => Results.Ok(excuse),
                    onFailure: error => Results.NotFound(error)
                );
            })
            .Produces<Excuse>()
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get an excuse by its ID.");

        group.MapGet("random", async (string? category, IExcuseRepository repository) =>
                {
                    var result = !string.IsNullOrEmpty(category)
                        ? await repository.GetRandomExcuseByCategoryAsync(category)
                        : await repository.GetRandomExcuseAsync();
                    return result.Match(
                        onSuccess: excuse => Results.Ok(excuse),
                        onFailure: error => Results.NotFound(error)
                    );
                }
            )
            .Produces<Excuse>()
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get a random excuse, optionally filtered by category.");

        group.MapGet("/categories", async (IExcuseRepository repository) =>
                {
                    var result = await repository.GetCategoriesAsync();
                    return result.Match(
                        onSuccess: categories => Results.Ok(categories),
                        onFailure: error =>
                            Results.Problem(detail: error, statusCode: StatusCodes.Status500InternalServerError)
                    );
                }
            )
            .Produces<List<string>>()
            .WithSummary("Get all excuse categories.");

        group.MapPut("/{id:int:min(0)}", async (int id, ExcuseInputDto excuse, IExcuseRepository repository) =>
                {
                    var result = await repository.UpdateExcuseAsync(id, excuse);
                    return result.Match(
                        onSuccess: updatedExcuse =>
                            Results.Json(new
                            {
                                message = "The excuse has been successfully updated",
                                excuse = updatedExcuse
                            }),
                        onFailure: error => Results.NotFound(error)
                    );
                }
            )
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Update an excuse by its ID.");

        group.MapDelete("/{id:int:min(0)}", async (int id, IExcuseRepository repository) =>
            {
                var result = await repository.DeleteExcuseAsync(id);
                return result.Match(
                    onSuccess: deletedExcuse =>
                        Results.Json(new
                        {
                            message = "The excuse has been successfully deleted",
                            excuse = deletedExcuse
                        }),
                    onFailure: error => Results.NotFound(error)
                );
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Delete an excuse by its ID.");
    }
}