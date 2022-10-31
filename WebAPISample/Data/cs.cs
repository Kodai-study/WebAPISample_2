using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPISample.Models;

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
