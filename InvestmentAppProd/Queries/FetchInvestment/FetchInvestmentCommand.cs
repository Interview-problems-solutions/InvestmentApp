using InvestmentAppProd.Models;
using MediatR;
using System.Collections.Generic;

namespace InvestmentAppProd.Queries.FetchInvestment
{
    public class FetchInvestmentCommand : IRequest<IEnumerable<Investment>>
    {
    }
}