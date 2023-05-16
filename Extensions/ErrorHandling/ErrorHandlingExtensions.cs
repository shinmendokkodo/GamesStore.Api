using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GamesStore.Api.Extensions.ErrorHandling;

public static class ErrorHandlingExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.Run(async context =>
        {
            var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("ErrorHandling");
            var exceptionDetails = context.Features.Get<IExceptionHandlerFeature>();

            var exception = exceptionDetails?.Error;

            logger.LogError(exception, "Unable to process request on {Machine}. TraceId: {TraceId}", Environment.MachineName, Activity.Current?.TraceId);

            var problem = new ProblemDetails
            {
                Title = "An error occurred while processing your request",
                Detail = "Please contact the system administrator.",
                Status = StatusCodes.Status500InternalServerError,
                Extensions =
                {
                    { "traceId", Activity.Current?.TraceId.ToString()}
                }

            };

            var environment = context.RequestServices.GetRequiredService<IHostEnvironment>();

            if (environment.IsDevelopment())
            {
                problem.Detail = exception?.ToString();
            }

            await Results.Problem(problem).ExecuteAsync(context);
        });
    }
}