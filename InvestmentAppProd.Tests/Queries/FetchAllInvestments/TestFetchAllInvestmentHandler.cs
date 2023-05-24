using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentAppProd.Tests.Queries.FetchAllInvestments
{
    internal class TestFetchAllInvestmentHandler : TestBase
    {
        [Test]
        public async Task Should_Fetch_All_Investments_Successfully()
        {
            var query = new FetchAllInvestmentsQuery();

            var handler = new FetchAllInvestmentHandler(Context);
            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().HaveCount(3);
        }
    }
}
