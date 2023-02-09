using Microsoft.EntityFrameworkCore;

namespace WebAPISample.Modules
{
    public class cs : DbContext
    {
        public cs(DbContextOptions<cs> options)
            : base(options)
        {
        }

        public DbSet<Modules.cs> Class { get; set; } = default!;
    }
}