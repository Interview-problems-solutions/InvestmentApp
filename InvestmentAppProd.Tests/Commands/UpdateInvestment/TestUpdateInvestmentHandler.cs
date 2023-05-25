namespace InvestmentAppProd.Tests.Commands.UpdateInvestment;

internal class TestUpdateInvestmentHandler : TestBase
{
    [Test]
    public async Task Should_Result_In_Name_MisMatch_Error()
    {
        var command = new UpdateInvestmentCommand(
            "Test Investment",
            new Investment
            {
                Name = "Test Investment 2",
                StartDate = DateTime.Parse("2022-03-01"),
                InterestType = InterestType.Simple,
                InterestRate = 3.875,
                PrincipalAmount = 10000
            });

        var handler = new UpdateInvestmentHandler(Context);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeOfType<Error>();
        result.Error.ErrorType.Should().Be(ErrorType.NameMisMatch);
    }

    [Test]
    public async Task Should_Return_Failure_When_Date_Is_In_Future()
    {
        var command = new UpdateInvestmentCommand(
            "Test Investment",
            new Investment
            {
                Name = "Test Investment",
                StartDate = DateTime.Now.AddMinutes(1),
                InterestType = InterestType.Simple,
                InterestRate = 3.875,
                PrincipalAmount = 10000
            });

        var handler = new UpdateInvestmentHandler(Context);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeOfType<Error>();
        result.Error.ErrorType.Should().Be(ErrorType.StartDateInFuture);
    }

    [Test]
    public async Task Should_Return_Does_Not_Exist_If_An_Investment_Is_Not_Present()
    {
        var command = new UpdateInvestmentCommand(
            "Test No Investment",
            new Investment
            {
                Name = "Test No Investment",
                StartDate = DateTime.Parse("2022-03-01"),
                InterestType = InterestType.Simple,
                InterestRate = 3.875,
                PrincipalAmount = 10000
            });

        var handler = new UpdateInvestmentHandler(Context);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeOfType<Error>();
        result.Error.ErrorType.Should().Be(ErrorType.DoesNotExit);
    }

    [Test]
    public async Task Should_Return_Success_In_Good_Case()
    {
        var command = new UpdateInvestmentCommand(
            "Investment 1",
            new Investment
            {
                Name = "Investment 1",
                StartDate = DateTime.Parse("2022-03-01"),
                InterestType = InterestType.Simple,
                InterestRate = 3.875,
                PrincipalAmount = 100000
            });

        var handler = new UpdateInvestmentHandler(Context);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }
}
