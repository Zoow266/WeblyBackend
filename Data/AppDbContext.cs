using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<User> Users{ get; set; }
    public DbSet<SiteData> SiteDatas{ get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { 
    }
}