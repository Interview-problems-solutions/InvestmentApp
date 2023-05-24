namespace InvestmentAppProd.Queries.FetchInvestment;

public class FetchInvestmentHandler : IRequestHandler<FetchInvestmentQuery, Maybe<Investment>>
{
    private readonly InvestmentDBContext _context;

    public FetchInvestmentHandler(InvestmentDBContext context) { this._context = context; }

    public async Task<Maybe<Investment>> Handle(FetchInvestmentQuery command, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(command.NameOfInvestment))
            return Maybe<Investment>.None;

        var investment = await _context.Investments.FindAsync(command.NameOfInvestment);

        return investment == null ? Maybe<Investment>.None : Maybe<Investment>.From(investment);
    }
}