namespace InvestmentAppProd.Tests.Commands.DeleteInvestment;

internal class TestDeleteInvestmentHandler : TestBase
{
    [Test]
    public async Task Should_Fail_When_Investment_Does_Not_Exist()
    {
        var command = new DeleteInvestmentCommand("Test Investment");

        var handler = new DeleteInvestmentHandler(Context);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Investment with name Test Investment not found.");
    }

    [Test]
    public async Task Should_Delete_Investment_Successfully()
    {
        var investment = new Investment
        {
            Name = "Test Investment",
            StartDate = DateTime.Parse("2022-03-01"),
            InterestType = InterestType.Simple,
            InterestRate = 3.875,
            PrincipalAmount = 10000
        };

        Context.Investments.Add(investment);
        await Context.SaveChangesAsync(CancellationToken.None);

        var command = new DeleteInvestmentCommand("Test Investment");

        var handler = new DeleteInvestmentHandler(Context);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }
}
