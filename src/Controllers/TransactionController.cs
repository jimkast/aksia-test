using Microsoft.AspNetCore.Mvc;
using TransactionApi.Models;
using TransactionApi.Services;
using TransactionApi.Repo;
using System.Net;

namespace TransactionApi.Controllers;

[ApiController]
[Route("transactions")]
public class TransactionController : ControllerBase
{
    private readonly ILogger<TransactionController> logger;
    private readonly TransactionImport svc;
    private readonly TransactionRepo repo;
    private readonly Func<string, string?> currenciesSymbolIndex;

    public TransactionController(
        ILogger<TransactionController> logger,
        TransactionImport svc,
        TransactionRepo repo,
        ICurrencyRepo currencies)
    {
        this.logger = logger;
        this.svc = svc;
        this.repo = repo;
        this.currenciesSymbolIndex = currencies.AsSymbolIndexFunc();
    }

    [HttpGet]
    public async Task<PaginatedResponse<TransactionModel>> Get(int page = 1, int count = 10)
    {
        var data = await repo.FetchManyPaginated(page, count);
        return new PaginatedResponse<TransactionModel>(data.Item1, (page - 1) * count, data.Item2);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var parseSuccess = Guid.TryParse(id, out Guid guid);
        if (!parseSuccess) return NotFound(null);
        var result = await repo.SingleById(guid);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var parseSuccess = Guid.TryParse(id, out Guid guid);
        if (!parseSuccess) return NotFound(null);
        var result = await repo.DeleteById(guid);
        return result ? Ok() : NotFound(null);
    }

    [HttpPost]
    public async Task<IActionResult> Upsert(TransactionModel payload)
    {
        var validationResult = payload.Validate();
        if (validationResult.Any())
        {
            return ValidationProblem(validationResult.First().ErrorMessage);
        }
        var result = await repo.Upsert(payload);
        return Ok(result);
    }

    [HttpPost]
    [Route("upload")]
    public async Task<IActionResult> Upload()
    {
        // if ((Request.ContentType ?? "").ToLower() != "text/csv")
        // {
        //     Response.StatusCode = (int)HttpStatusCode.UnsupportedMediaType;
        //     return Content("Only text/csv 'Content-Type' is supported for import.");
        // }
        var start = DateTime.UtcNow;
        List<ErrorRowResult> errorAccumulator = [];
        var successRows = await repo.BulkMerge(svc.StreamRows(new StreamReader(Request.Body), currenciesSymbolIndex, errorAccumulator));
        return Ok(new TransactionImportResult
        {
            StartedAt = start,
            DurationMillis = (int)(DateTime.UtcNow - start).TotalMilliseconds,
            SuccessTotalRows = successRows,
            ErrorTotalRows = errorAccumulator.Count,
            Details = errorAccumulator
        });
    }
}
