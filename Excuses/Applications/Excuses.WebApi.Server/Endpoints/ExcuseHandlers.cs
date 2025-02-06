using System.ComponentModel.DataAnnotations;
using Excuses.Persistence.Shared.DTO;
using Excuses.Persistence.Shared.Interfaces;

namespace Excuses.WebApi.Server.Endpoints;

// !: TypedResults API
// Source: https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-9.0&tabs=visual-studio#use-the-typedresults-api

public static class ExcuseHandlers
{
    public static async Task<IResult> CreateExcuseAsync(ExcuseInputDto excuse, IExcuseRepository repository)
    {
        if (!Validator.TryValidateObject(excuse, new ValidationContext(excuse), null, true))
            return TypedResults.BadRequest("Invalid excuse data.");

        var result = await repository.CreateExcuseAsync(excuse);
        return result.Match<IResult>(
            onSuccess: createdExcuse => TypedResults.Created($"api/excuses/{createdExcuse.Id}", createdExcuse),
            onFailure: error => TypedResults.BadRequest(error)
        );
    }

    public static async Task<IResult> GetExcusesAsync(string? category, IExcuseRepository repository)
    {
        var result = !string.IsNullOrEmpty(category)
            ? await repository.GetExcusesByCategoryAsync(category)
            : await repository.GetExcusesAsync();
        return result.Match<IResult>(
            onSuccess: excuses => TypedResults.Ok(excuses),
            onFailure: error => TypedResults.Problem(error, statusCode: StatusCodes.Status500InternalServerError)
        );
    }

    public static async Task<IResult> GetExcuseAsync(int id, IExcuseRepository repository)
    {
        var result = await repository.GetExcuseAsync(id);
        return result.Match<IResult>(
            onSuccess: excuse => TypedResults.Ok(excuse),
            onFailure: error => TypedResults.NotFound(error)
        );
    }

    public static async Task<IResult> UpdateExcuseAsync(int id, ExcuseInputDto excuse, IExcuseRepository repository)
    {
        var result = await repository.UpdateExcuseAsync(id, excuse);
        return result.Match<IResult>(
            onSuccess: updatedExcuse => TypedResults.Ok(new
            {
                message = "The excuse has been successfully updated",
                excuse = updatedExcuse
            }),
            onFailure: error => TypedResults.NotFound(error)
        );
    }

    public static async Task<IResult> DeleteExcuseAsync(int id, IExcuseRepository repository)
    {
        var result = await repository.DeleteExcuseAsync(id);
        return result.Match<IResult>(
            onSuccess: deletedExcuse => TypedResults.Ok(new
            {
                message = "The excuse has been successfully deleted",
                excuse = deletedExcuse
            }),
            onFailure: error => TypedResults.NotFound(error)
        );
    }

    public static async Task<IResult> GetRandomExcuseAsync(string? category, IExcuseRepository repository)
    {
        var result = !string.IsNullOrEmpty(category)
            ? await repository.GetRandomExcuseByCategoryAsync(category)
            : await repository.GetRandomExcuseAsync();
        return result.Match<IResult>(
            onSuccess: excuse => TypedResults.Ok(excuse),
            onFailure: error => TypedResults.NotFound(error)
        );
    }

    public static async Task<IResult> GetCategoriesAsync(IExcuseRepository repository)
    {
        var result = await repository.GetCategoriesAsync();
        return result.Match<IResult>(
            onSuccess: categories => TypedResults.Ok(categories),
            onFailure: error => TypedResults.Problem(error, statusCode: StatusCodes.Status500InternalServerError)
        );
    }
}