namespace InvestmentAppProd.Tests.Extensions;

internal class TestInvestmentExtensions
{
    [TestCase(-2, 3.875, 10000, ExpectedResult = 10804.47)]
    [TestCase(-2.5, 3.875, 10000, ExpectedResult = 11015.51)]
    public double CalculateInterest_For_CompoundInterest_ShouldReturn_CorrectValue(
        double numberOfYears,
        double interestRate,
        double principal)
    {
        var investment = new Investment(
            "Compound Investment",
            DateTime.Now.AddMonths(ToMonths(numberOfYears)),
            "Compound",
            interestRate,
            principal);

        return investment.CalculateInterest();
    }

    [TestCase(-2, 3.875, 10000, ExpectedResult = 10775.00)]
    [TestCase(-2.5, 3.875, 10000, ExpectedResult = 10968.75)]
    public double CalculateInterest_For_Simple_Interest_Should_Return_CorrectValue(
        double numberOfYears,
        double interestRate,
        double principal)
    {
        var investment = new Investment(
            "Simple Investment",
            DateTime.Now.AddMonths(ToMonths(numberOfYears)),
            "Simple",
            interestRate,
            principal);

        return investment.CalculateInterest();
    }

    private static int ToMonths(double numberOfYears) 
        => (int)Math.Round(numberOfYears * 12, MidpointRounding.AwayFromZero);
}
