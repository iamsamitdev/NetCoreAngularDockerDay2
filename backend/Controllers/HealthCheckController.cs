using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]
public class HealthCheckController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HealthCheckController> _logger;

    public HealthCheckController(ApplicationDbContext context, ILogger<HealthCheckController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            // ทดสอบการเชื่อมต่อฐานข้อมูล
            await _context.Database.CanConnectAsync();
            
            return Ok(new
            {
                Status = "Healthy",
                Database = "Connected",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database connection failed");
            return StatusCode(500, new
            {
                Status = "Unhealthy",
                Database = "Disconnected",
                Error = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
    }
} 