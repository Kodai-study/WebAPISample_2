using Microsoft.EntityFrameworkCore;

namespace WebAPISample.Modules.Class
{
    public class cs : DbContext
    {
        public cs (DbContextOptions<cs> options)
            : base(options)
        {
        }

        public DbSet<WebAPISample.Models.Class> Class { get; set; } = default!;
    }
}
