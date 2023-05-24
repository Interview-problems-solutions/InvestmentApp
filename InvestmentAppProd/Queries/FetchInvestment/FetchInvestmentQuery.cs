namespace InvestmentAppProd.Queries.FetchInvestment;

public class FetchInvestmentQuery : IRequest<Maybe<Investment>>
{
    public FetchInvestmentQuery(string nameOfInvestment) { NameOfInvestment = nameOfInvestment; }

    public string NameOfInvestment { get; }
}