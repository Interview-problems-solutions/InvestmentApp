namespace InvestmentAppProd.Commands.DeleteInvestment;

public class DeleteInvestmentHandler : IRequestHandler<DeleteInvestmentCommand, Result>
{
    private readonly InvestmentDBContext _context;

    public DeleteInvestmentHandler(InvestmentDBContext context) { _context = context; }

    // Use Result type to return success or failure
    // it is more readable than throwing exceptions or returning null
    public async Task<Result> Handle(DeleteInvestmentCommand command, CancellationToken cancellationToken)
    {
        var investment = await _context.Investments.FindAsync(command.NameOfInvestment);

        // using Result type avoids checking for null in upper layers
        if(investment == null)
            return Result.Failure($"Investment with name {command.NameOfInvestment} not found.");

        _context.ChangeTracker.Clear();
        _context.Investments.Remove(investment);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}