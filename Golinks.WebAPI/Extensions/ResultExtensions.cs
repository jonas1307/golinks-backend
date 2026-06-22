using Golinks.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace Golinks.WebAPI.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(
        this Result<T> result,
        ControllerBase controller,
        Func<T, IActionResult> onSuccess)
        => result.Match(
            onSuccess,
            error => error.Type switch
            {
                ErrorType.NotFound => controller.Problem(detail: error.Description, statusCode: 404),
                ErrorType.Conflict => controller.Problem(detail: error.Description, statusCode: 409),
                ErrorType.Validation => controller.Problem(detail: error.Description, statusCode: 400),
                _ => controller.Problem(detail: error.Description, statusCode: 500)
            });

    public static IActionResult ToActionResult(
        this Result result,
        ControllerBase controller,
        Func<IActionResult> onSuccess)
        => result.Match(
            onSuccess,
            error => error.Type switch
            {
                ErrorType.NotFound => controller.Problem(detail: error.Description, statusCode: 404),
                ErrorType.Conflict => controller.Problem(detail: error.Description, statusCode: 409),
                ErrorType.Validation => controller.Problem(detail: error.Description, statusCode: 400),
                _ => controller.Problem(detail: error.Description, statusCode: 500)
            });
}
