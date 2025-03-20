using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SiteDataController : ControllerBase
{
    private readonly AppDbContext _context;

    public SiteDataController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Post([FromBody] SiteData data)
    {
        _context.Add(data);
        _context.SaveChanges();

        return Ok("Данные сохранены");
    }

    [HttpGet]
    public IActionResult Get(int id)
    {
        var data = _context.SiteDatas.FirstOrDefault(x => x.Id == id);

        if(data == null)
        {
            return NotFound();
        }
        return Ok(data);
    }
}