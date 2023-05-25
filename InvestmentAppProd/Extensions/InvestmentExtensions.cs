namespace InvestmentAppProd.Extensions;

public static class InvestmentExtensions
{
    public static double CalculateCompoundInterest(this Investment investment)
    {
        // Interest rate is divided by 100.
        var r = investment.InterestRate / 100;

        // Time t is calculated to the nearest month.
        double monthsDiff = 12 *
            (investment.StartDate.Year - DateTime.Now.Year) +
            investment.StartDate.Month -
            DateTime.Now.Month;
        monthsDiff = Math.Abs(monthsDiff);
        var t = monthsDiff / 12;

        // COMPOUND INTEREST.
        // Compounding period is set to monthly (i.e. n = 12).
        double n = 12;
        return investment.PrincipalAmount * Math.Pow((1 + (r / n)), (n * t));
    }

    public static double CalculateSimpleInterest(this Investment investment)
    {
        // Interest rate is divided by 100.
        var r = investment.InterestRate / 100;

        // Time t is calculated to the nearest month.
        double monthsDiff = 12 *
            (investment.StartDate.Year - DateTime.Now.Year) +
            investment.StartDate.Month -
            DateTime.Now.Month;
        monthsDiff = Math.Abs(monthsDiff);
        var t = monthsDiff / 12;

        // SIMPLE INTEREST.
        return investment.PrincipalAmount * (1 + (r * t));
    }

    public static double CalculateInterest(this Investment investment)
    {
        var value = investment.InterestType switch
        {
            InterestType.None => 0,
            InterestType.Simple => investment.CalculateSimpleInterest(),
            InterestType.Compound => investment.CalculateCompoundInterest(),
            _ => 0
        };

        return Math.Round(value, 2);
    }
}
