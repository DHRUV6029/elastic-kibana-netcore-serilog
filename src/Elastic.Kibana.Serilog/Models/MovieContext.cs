using Microsoft.EntityFrameworkCore;

namespace Elastic.Kibana.Serilog.Models
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options)
           : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; } = null!;
    }
}
