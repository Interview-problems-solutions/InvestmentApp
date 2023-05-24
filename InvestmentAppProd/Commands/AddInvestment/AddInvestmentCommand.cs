namespace InvestmentAppProd.Commands.AddInvestment;

public class AddInvestmentCommand : IRequest<Result<Investment, IError>>
{
    public AddInvestmentCommand(Investment investment) { Investment = investment; }

    public Investment Investment { get; }
}