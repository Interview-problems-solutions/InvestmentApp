using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform;
using NUnit.Framework;
using InvestmentAppProd.Models;
using InvestmentAppProd.Controllers;
using InvestmentAppProd.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Threading;
using InvestmentAppProd.Queries.FetchInvestment;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using InvestmentAppProd.Queries.FetchAllInvestments;

namespace InvestmentAppProd.Tests
{
    [TestFixture]
    public class TestInvestmentController
    {
        InvestmentDBContext _context;
        private IMediator _mediator;

        [OneTimeSetUp]
        public void Setup()
        {
            var services = new ServiceCollection();

            services.AddDbContext<InvestmentDBContext>(options => options.UseInMemoryDatabase("InvestmentsDbTest"));
            services.AddMediatR(
            cfg =>
            {
                    cfg.RegisterServicesFromAssembly(typeof(FetchAllInvestmentsCommand).Assembly);
                });
            var provider = services.BuildServiceProvider();

            _context = provider.GetService<InvestmentDBContext>();
            _context.Database.EnsureCreated();

            SeedDatabase();

            _mediator = provider.GetService<IMediator>();
        }

        [OneTimeTearDown]
        public void CleanUp() { _context.Database.EnsureDeleted(); }

        private void SeedDatabase()
        {
            var newInvestments = new List<Investment>();

            newInvestments.Add(
                new Investment
                {
                    Name = "Investment 1",
                    StartDate = DateTime.Parse("2022-03-01"),
                    InterestType = "Simple",
                    InterestRate = 3.875,
                    PrincipalAmount = 10000
                });
            newInvestments.Add(
                new Investment
                {
                    Name = "Investment 2",
                    StartDate = DateTime.Parse("2022-04-01"),
                    InterestType = "Simple",
                    InterestRate = 4,
                    PrincipalAmount = 15000
                });
            newInvestments.Add(
                new Investment
                {
                    Name = "Investment 3",
                    StartDate = DateTime.Parse("2022-05-01"),
                    InterestType = "Compound",
                    InterestRate = 5,
                    PrincipalAmount = 20000
                });

            _context.Investments.AddRange(newInvestments);
            _context.SaveChanges();
        }

        private void EmptyDatabase()
        {
            _context.Investments.RemoveRange(_context.Investments.ToList<Investment>());
            _context.SaveChanges();
        }

        [Test]
        public async Task GetAllInvestments_WithExistingItems_ShouldReturnAllInvestments()
        {
            // ARRANGE
            var controller = new InvestmentController(_context, _mediator);

            // ACT
            var result = await controller.FetchInvestment();
            var obj = result.Result as ObjectResult;
            var objListResult = (List<Investment>)obj.Value;
            //var objCountResult = ((List<Investment>)obj.Value).Count();

            // ASSERT   : Status code 200 ("Ok") + Count of objects returned is correct + Object returned (first) is of Type Investment.
            Assert.AreEqual(200, (obj.StatusCode));
            Assert.AreEqual(_context.Investments.Count(), objListResult.Count());
            Assert.IsInstanceOf<Investment>(objListResult.First());
        }

        [Test]
        public async Task GetInvestment_WithSingleItem_ShouldReturnSingleInvestment()
        {
            // Arrange
            var controller = new InvestmentController(_context, _mediator);
            var name = "Investment 1";

            // Act
            var result = await controller.FetchInvestment(name);
            var obj = result.Result as ObjectResult;
            var objInvResult = obj.Value as Investment;

            // Assert   : Status code 200 ("Ok") + Object returned is of Type Investment + Object name is same.
            Assert.AreEqual(200, (obj.StatusCode));
            Assert.IsInstanceOf<Investment>(objInvResult);
            Assert.AreEqual(name, objInvResult.Name);
        }

        [Test]
        public void AddInvestment_SingleItem_ShouldAddInvestment()
        {
            // Arrange
            var controller = new InvestmentController(_context, _mediator);
            var newInvestnment = new Investment
            {
                Name = "Investment 4",
                StartDate = DateTime.Parse("2022-05-01"),
                InterestType = "Simple",
                InterestRate = 7.7,
                PrincipalAmount = 25000
            };

            // Act
            var result = controller.AddInvestment(newInvestnment);
            var obj = result.Result as ObjectResult;
            //var objInvResult = obj.Value as Investment;

            // Assert   : Status code 201 ("Created")
            Assert.AreEqual(201, (obj.StatusCode));
        }

        [Test]
        public void UpdateInvestment_SingleItem_ShouldUpdateInvestment()
        {
            // Arrange
            CleanUp();
            Setup();
            var controller = new InvestmentController(_context, _mediator);
            var updateInvestment = "Investment 2";
            var newInvestment = new Investment
            {
                Name = "Investment 2",
                StartDate = DateTime.Parse("2022-06-01"),
                InterestType = "Compound",
                InterestRate = 8,
                PrincipalAmount = 30000
            };

            // Act
            var result = controller.UpdateInvestment(updateInvestment, newInvestment);
            var obj = result as NoContentResult;

            // Assert   : Status code 204 ("No Content")
            Assert.AreEqual(204, obj.StatusCode);
        }

        [Test]
        public async Task DeleteInvestment_SingleItem_ShouldDeleteInvestment()
        {
            // Arrange
            var controller = new InvestmentController(_context, _mediator);
            var deleteInvestment = "Investment 2";

            // Act
            var result = await controller.DeleteInvestment(deleteInvestment);
            var obj = result as NoContentResult;

            // Assert   : Status code 204 ("No Content")
            Assert.AreEqual(204, obj.StatusCode);
        }
    }
}
