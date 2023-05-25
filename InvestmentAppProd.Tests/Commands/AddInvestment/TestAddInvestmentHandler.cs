namespace InvestmentAppProd.Tests.Commands.AddInvestment;

internal class TestAddInvestmentHandler : TestBase
{
    [Test]
    public async Task Should_Return_Failure_When_Date_Is_In_Future()
    {
        var investment = new Investment
        {
            Name = "Investment 1",
            StartDate = DateTime.Now.AddMinutes(1),
            InterestType = InterestType.Simple,
            InterestRate = 3.875,
            PrincipalAmount = 10000
        };

        var handler = new AddInvestmentHandler(Context);
        var result = await handler.Handle(new AddInvestmentCommand(investment), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeOfType<Error>();
        result.Error.ErrorType.Should().Be(ErrorType.StartDateInFuture);
    }

    [Test]
    public async Task Should_Return_Failure_When_An_Investment_Already_Exists()
    {
        var investment = new Investment
        {
            Name = "Investment 1",
            StartDate = DateTime.Parse("2022-03-01"),
            InterestType = InterestType.Simple,
            InterestRate = 3.875,
            PrincipalAmount = 10000
        };

        var handler = new AddInvestmentHandler(Context);
        var result = await handler.Handle(new AddInvestmentCommand(investment), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeOfType<Error>();
        result.Error.ErrorType.Should().Be(ErrorType.AlreadyExists);
    }

    [Test]
    public async Task Should_Return_An_Investment_In_Success_Case()
    {
        var investment = new Investment
        {
            Name = "Investment 4",
            StartDate = DateTime.Parse("2022-06-01"),
            InterestType = InterestType.Simple,
            InterestRate = 3.875,
            PrincipalAmount = 10000
        };

        var handler = new AddInvestmentHandler(Context);
        var result = await handler.Handle(new AddInvestmentCommand(investment), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<Investment>();
        result.Value.Name.Should().Be("Investment 4");
    }
}
