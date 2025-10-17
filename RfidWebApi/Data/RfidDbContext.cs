using Microsoft.EntityFrameworkCore;
using RfidWebApi.Models;

namespace RfidWebApi.Data
{
    public class RfidDbContext : DbContext
    {
        public RfidDbContext(DbContextOptions<RfidDbContext> options)
            : base(options)
        {
        }

        public DbSet<CardData> CardDatas { get; set; }
    }
}
