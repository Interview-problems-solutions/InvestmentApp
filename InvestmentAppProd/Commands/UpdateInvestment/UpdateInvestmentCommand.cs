namespace InvestmentAppProd.Commands.UpdateInvestment;

public class UpdateInvestmentCommand : IRequest<Result<Unit, IError>>
{
    public UpdateInvestmentCommand(string nameOfInvestmentToUpdate, Investment newDetailsOfInvestment)
    {
        NameOfInvestmentToUpdate = nameOfInvestmentToUpdate;
        NewDetailsOfInvestment = newDetailsOfInvestment;
    }

    public string NameOfInvestmentToUpdate { get; }
    public Investment NewDetailsOfInvestment { get; }
}