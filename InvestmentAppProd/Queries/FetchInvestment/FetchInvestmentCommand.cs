namespace InvestmentAppProd.Queries.FetchInvestment;

public class FetchInvestmentCommand : IRequest<Maybe<Investment>>
{
    public FetchInvestmentCommand(string nameOfInvestment)
    {
        NameOfInvestment = nameOfInvestment;
    }

    public string NameOfInvestment { get; }
}