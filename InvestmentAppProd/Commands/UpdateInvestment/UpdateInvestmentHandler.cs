namespace InvestmentAppProd.Commands.UpdateInvestment;

public class UpdateInvestmentHandler : IRequestHandler<UpdateInvestmentCommand, Result<Unit, IError>>
{
    private readonly InvestmentDBContext _context;

    public UpdateInvestmentHandler(InvestmentDBContext context) { _context = context; }

    public async Task<Result<Unit, IError>> Handle(UpdateInvestmentCommand command, CancellationToken cancellationToken)
    {
        if(command.NameOfInvestmentToUpdate != command.NewDetailsOfInvestment.Name)
            return Result.Failure<Unit, IError>(new Error(ErrorType.NameMisMatch));

        if(command.NewDetailsOfInvestment.StartDate > DateTime.Now)
            return Result.Failure<Unit, IError>(new Error(ErrorType.StartDateInFuture));

        var result = await new FetchInvestmentHandler(_context).Handle(
            new FetchInvestmentCommand(command.NameOfInvestmentToUpdate),
            cancellationToken);
        if(result.HasNoValue)
            return Result.Failure<Unit, IError>(new Error(ErrorType.DoesNotExit));

        command.NewDetailsOfInvestment.CurrentValue = command.NewDetailsOfInvestment.CalculateInterest();
        _context.ChangeTracker.Clear();
        _context.Entry(command.NewDetailsOfInvestment).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success<Unit, IError>(Unit.Value);
    }
}