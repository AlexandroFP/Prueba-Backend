using Cenace.API.Domain.Entities;  
using Microsoft.EntityFrameworkCore;

namespace Cenace.API.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<CapacityDemand> CapacityDemands { get; set; }
    }
}
