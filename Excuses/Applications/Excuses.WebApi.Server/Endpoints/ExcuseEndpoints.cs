using Excuses.Persistence.Shared.Models;

namespace Excuses.WebApi.Server.Endpoints;

public static class ExcuseEndpoints
{
    public static void MapExcuseEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/v1/excuses");

        group.MapPost("", ExcuseHandlers.CreateExcuseAsync)
            .WithSummary("Create a new excuse.")
            .WithDescription("Creates a new excuse.")
            .Produces<Excuse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapGet("", ExcuseHandlers.GetExcusesAsync)
            .WithSummary("Get all excuses, optionally filtered by category.")
            .Produces<List<Excuse>>();

        group.MapGet("/{id:int:min(0)}", ExcuseHandlers.GetExcuseAsync)
            .WithSummary("Get an excuse by its ID.")
            .Produces<Excuse>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("random", ExcuseHandlers.GetRandomExcuseAsync)
            .WithSummary("Get a random excuse, optionally filtered by category.")
            .Produces<Excuse>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("categories", ExcuseHandlers.GetCategoriesAsync)
            .WithSummary("Get all excuse categories.")
            .Produces<List<string>>();

        group.MapPut("/{id:int:min(0)}", ExcuseHandlers.UpdateExcuseAsync)
            .WithSummary("Update an excuse by its ID.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:int:min(0)}", ExcuseHandlers.DeleteExcuseAsync)
            .WithSummary("Delete an excuse by its ID.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}