using Microsoft.EntityFrameworkCore;

namespace WebAPISample.JSONModels
{
    public class cs : DbContext
    {
        public cs(DbContextOptions<cs> options)
            : base(options)
        {
        }

        public DbSet<JSONModels.cs> Class { get; set; } = default!;
    }
}