using System.Net;
using Microsoft.AspNetCore.Diagnostics;

namespace TransactionApi.Controllers;
public class GlobalUnexpectedErrorHandler(ILogger<TransactionController> logger) : IExceptionHandler
{
    private readonly ILogger<TransactionController> logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext ctx, Exception ex, CancellationToken cancellation)
    {
        Guid errorId = Guid.NewGuid();
        logger.LogError(ex, ex.Message, ex.StackTrace);
        ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var err = new { errorId, message = "An unexpected error occured." };
        await ctx.Response.WriteAsJsonAsync(err, cancellation);
        return true;
    }
}
