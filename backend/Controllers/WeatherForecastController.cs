using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public WeatherForecastController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /WeatherForecast
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get()
    {
        return await _context.WeatherForecasts.ToListAsync();
    }

    // GET: /WeatherForecast/5
    [HttpGet("{id}")]
    public async Task<ActionResult<WeatherForecast>> Get(int id)
    {
        var weatherForecast = await _context.WeatherForecasts.FindAsync(id);

        if (weatherForecast == null)
        {
            return NotFound();
        }

        return weatherForecast;
    }
} 