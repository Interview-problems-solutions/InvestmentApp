namespace InvestmentAppProd.Queries.FetchAllInvestments;

public class FetchAllInvestmentHandler : IRequestHandler<FetchAllInvestmentsQuery, IEnumerable<Investment>>
{
    private readonly InvestmentDBContext _context;

    public FetchAllInvestmentHandler(InvestmentDBContext context) { _context = context; }

    public async Task<IEnumerable<Investment>> Handle(
        FetchAllInvestmentsQuery command,
        CancellationToken cancellationToken) => await _context.Investments.ToListAsync(cancellationToken);
}