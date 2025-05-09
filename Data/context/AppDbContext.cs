using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

public class AppDbContext : DbContext
{
    public DbSet<MonitoredFlightsModel> MonitoredFlights { get; set; }
    public DbSet<FlyghtLinkedInUserModel> FlyLinked {  get; set; }
    public DbSet<UserModel> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}
