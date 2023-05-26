namespace InvestmentAppProd.Commands.UpdateInvestment;

public class UpdateInvestmentHandler : IRequestHandler<UpdateInvestmentCommand, Result<Unit, IError>>
{
    private readonly InvestmentDBContext _context;

    public UpdateInvestmentHandler(InvestmentDBContext context) { _context = context; }

    // Use Result type to return success or failure
    // avoid throwing exceptions or returning null
    public async Task<Result<Unit, IError>> Handle(UpdateInvestmentCommand command, CancellationToken cancellationToken)
    {
        // Validation logic can be moved to a validator (or) a pipeline validation eventually
        if(command.NameOfInvestmentToUpdate != command.NewDetailsOfInvestment.Name)
            return Result.Failure<Unit, IError>(new Error(ErrorType.NameMisMatch));

        if(command.NewDetailsOfInvestment.StartDate > DateTime.Now)
            return Result.Failure<Unit, IError>(new Error(ErrorType.StartDateInFuture));

        var result = await new FetchInvestmentHandler(_context).Handle(
            new FetchInvestmentQuery(command.NameOfInvestmentToUpdate),
            cancellationToken);
        if(result.HasNoValue)
            return Result.Failure<Unit, IError>(new Error(ErrorType.DoesNotExit));
        
        _context.ChangeTracker.Clear();
        _context.Entry(command.NewDetailsOfInvestment).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success<Unit, IError>(Unit.Value);
    }
}