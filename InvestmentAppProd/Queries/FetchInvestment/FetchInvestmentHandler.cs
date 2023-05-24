using InvestmentAppProd.Data;
using InvestmentAppProd.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InvestmentAppProd.Queries.FetchInvestment
{
    public class FetchInvestmentHandler : IRequestHandler<FetchInvestmentCommand, IEnumerable<Investment>>
    {
        private readonly InvestmentDBContext _context;

        public FetchInvestmentHandler(InvestmentDBContext context) { _context = context; }

        public async Task<IEnumerable<Investment>> Handle(
            FetchInvestmentCommand command,
            CancellationToken cancellationToken) => await _context.Investments.ToListAsync();
    }
}