namespace InvestmentAppProd.Commands.AddInvestment;

internal class AddInvestmentHandler : IRequestHandler<AddInvestmentCommand, Result<Investment, IError>>
{
    private readonly InvestmentDBContext _context;

    public AddInvestmentHandler(InvestmentDBContext context)
    {
        _context = context;
    }

    public async Task<Result<Investment, IError>> Handle(AddInvestmentCommand command, CancellationToken cancellationToken)
    {
        if (command.Investment.StartDate > DateTime.Now)
            return Result.Failure<Investment, IError>(new Error(ErrorType.StartDateInFuture));

        var result = await new FetchInvestmentHandler(_context).Handle(new FetchInvestmentCommand(command.Investment.Name), cancellationToken);
            
        if(result.HasValue)
            return Result.Failure<Investment, IError>(new Error(ErrorType.AlreadyExists));

        command.Investment.CurrentValue = command.Investment.CalculateInterest();
        _context.ChangeTracker.Clear();
        _context.Investments.Add(command.Investment);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success<Investment, IError>(command.Investment);  
    }
}