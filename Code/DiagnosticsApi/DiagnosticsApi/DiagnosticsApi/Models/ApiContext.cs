using Microsoft.EntityFrameworkCore;

namespace DiagnosticsApi.Models
{
    public class ApiContext : DbContext
    {
        //// Entity sets.
        //public DbSet<Artist> Artists { get; set; }
        //public DbSet<Track> Tracks { get; set; }


        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

    }
}