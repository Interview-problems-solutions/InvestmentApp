using CSharpFunctionalExtensions;
using InvestmentAppProd.Queries.FetchInvestment;

namespace InvestmentAppProd.Tests.Queries.FetchInvestment
{
    internal class TestFetchInvestmentHandler : TestBase
    {
        [Test]
        public async Task Should_Return_Expected_Investment()
        {
            var query = new FetchInvestmentQuery("Investment 1");

            var handler = new FetchInvestmentHandler(Context);
            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().BeOfType<Maybe<Investment>>();
            result.HasValue.Should().BeTrue();
            result.Value.Name.Should().BeEquivalentTo("Investment 1");
        }

        [Test]
        public async Task Should_Return_None_When_No_Investment_Is_Found()
        {
            var query = new FetchInvestmentQuery("No Investment");

            var handler = new FetchInvestmentHandler(Context);
            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().BeOfType<Maybe<Investment>>();
            result.HasValue.Should().BeFalse();
        }
    }
}
