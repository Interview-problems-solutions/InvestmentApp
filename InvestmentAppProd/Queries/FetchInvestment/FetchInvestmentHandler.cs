namespace InvestmentAppProd.Queries.FetchInvestment;

internal class FetchInvestmentHandler : IRequestHandler<FetchInvestmentCommand, Maybe<Investment>>
{
    private readonly InvestmentDBContext _context;

    public FetchInvestmentHandler(InvestmentDBContext context)
    {
        this._context = context;
    }

    public async Task<Maybe<Investment>> Handle(FetchInvestmentCommand command, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(command.NameOfInvestment)) return Maybe<Investment>.None;

        var investment = await _context.Investments.FindAsync(command.NameOfInvestment);
        
        return investment == null ? Maybe<Investment>.None : Maybe<Investment>.From(investment);
    }
}