using System.Linq;

namespace InvestmentAppProd.Data
{
    public class InvestmentDBContext : DbContext
    {
        public DbSet<Investment> Investments { get; set; }

        public InvestmentDBContext(DbContextOptions<InvestmentDBContext> options) : base(options)
        {
        }

    }
}
