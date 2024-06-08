using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransactionApi.Models;

namespace TransactionApi.Controllers;

[ApiController]
[Route("transactions")]
public class TransactionController : ControllerBase
{
    private readonly ILogger<TransactionController> logger;
    private readonly TransactionImport svc;
    private readonly TransactionRepo repo;

    public TransactionController(
        ILogger<TransactionController> logger,
        TransactionImport svc,
        TransactionRepo repo)
    {
        this.logger = logger;
        this.svc = svc;
        this.repo = repo;
    }

    [HttpGet]
    public async Task<List<TransactionModel>> Get(int page = 1, int count = 10)
    {
        return await repo.FetchManyPaginated(page, count, null);
        // return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var parseSuccess = Guid.TryParse(id, out Guid guid);
        if (!parseSuccess)
        {
            return NotFound();
        }
        var result = await repo.SingleById(guid);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<string> Upload()
    {
        await repo.BulkMerge(svc.StreamRows(new StreamReader(Request.Body)));
        return "Success 123!!";
    }
}
