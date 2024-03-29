﻿namespace InvestmentAppProd.Commands.AddInvestment;

public class AddInvestmentHandler : IRequestHandler<AddInvestmentCommand, Result<Investment, IError>>
{
    private readonly InvestmentDBContext _context;

    public AddInvestmentHandler(InvestmentDBContext context) { _context = context; }

    public async Task<Result<Investment, IError>> Handle(
        AddInvestmentCommand command,
        CancellationToken cancellationToken)
    {
        // eventually when validations grow this can be moved to a validator (or) a pipeline validation
        // this way clear error codes can be sent to front-end to translate to proper error messages
        if(command.Investment.StartDate > DateTime.Now)
            return Result.Failure<Investment, IError>(new Error(ErrorType.StartDateInFuture));

        var result = await new FetchInvestmentHandler(_context).Handle(
            new FetchInvestmentQuery(command.Investment.Name),
            cancellationToken);

        if(result.HasValue)
            return Result.Failure<Investment, IError>(new Error(ErrorType.AlreadyExists));
        
        _context.ChangeTracker.Clear();
        _context.Investments.Add(command.Investment);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success<Investment, IError>(command.Investment);
    }
}