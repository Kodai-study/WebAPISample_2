using Microsoft.EntityFrameworkCore;

namespace WebAPISample.JSONModels
{
    public class SQLDatabaseConText : DbContext
    {
        public SQLDatabaseConText(DbContextOptions<SQLDatabaseConText> options)
            : base(options)
        {
        }

        public DbSet<JSONModels.SQLDatabaseConText> Class { get; set; } = default!;
    }
}