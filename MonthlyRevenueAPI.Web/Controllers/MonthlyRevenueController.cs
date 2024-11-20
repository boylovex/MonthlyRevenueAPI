using Microsoft.AspNetCore.Mvc;
using MonthlyRevenueAPI.Core.Models;
using MonthlyRevenueAPI.Core.Services.Interfaces;

namespace MonthlyRevenueAPI.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MonthlyRevenueController : ControllerBase
{
    private readonly IMonthlyRevenueService _service;
    private readonly ILogger<MonthlyRevenueController> _logger;

    public MonthlyRevenueController(
        IMonthlyRevenueService service,
        ILogger<MonthlyRevenueController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MonthlyRevenue>>> GetMonthlyRevenue(
        [FromQuery] MonthlyRevenueQuery query)
    {
        try
        {
            var result = await _service.GetMonthlyRevenueAsync(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting monthly revenue data");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> InsertMonthlyRevenue(
        [FromBody] MonthlyRevenue revenue)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _service.InsertMonthlyRevenueAsync(revenue);
            return result ? Ok() : BadRequest("Failed to insert data");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inserting monthly revenue data");
            return StatusCode(500, "Internal server error");
        }
    }
}
