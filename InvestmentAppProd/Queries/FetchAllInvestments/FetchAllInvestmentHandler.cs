namespace InvestmentAppProd.Queries.FetchAllInvestments;

public class FetchAllInvestmentHandler : IRequestHandler<FetchAllInvestmentsCommand, IEnumerable<Investment>>
{
    private readonly InvestmentDBContext _context;

    public FetchAllInvestmentHandler(InvestmentDBContext context) { _context = context; }

    public async Task<IEnumerable<Investment>> Handle(
        FetchAllInvestmentsCommand command,
        CancellationToken cancellationToken) => await _context.Investments.ToListAsync();
}