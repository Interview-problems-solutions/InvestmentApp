namespace InvestmentAppProd.Commands.DeleteInvestment;

public class DeleteInvestmentCommand : IRequest<Result>
{
    public DeleteInvestmentCommand(string nameOfInvestment) { NameOfInvestment = nameOfInvestment; }

    public string NameOfInvestment { get; }
}