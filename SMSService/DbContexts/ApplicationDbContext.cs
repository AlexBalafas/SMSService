using Microsoft.EntityFrameworkCore;
using SMSService.Entities;

namespace SMSService.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
                   : base(options)
        {

        }
        public DbSet<Sms> Sms { get; set; }
    }
}
