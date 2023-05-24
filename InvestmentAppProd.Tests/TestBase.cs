namespace InvestmentAppProd.Tests;

internal class TestBase
{
    private static readonly DbContextOptions<InvestmentDBContext> DbContextOptions
        = new DbContextOptionsBuilder<InvestmentDBContext>()
            .UseInMemoryDatabase(databaseName: "InvestmentsDbTest")
        .Options;

    protected InvestmentDBContext Context;

    [OneTimeSetUp]
    public void Setup()
    {
        Context = new InvestmentDBContext(DbContextOptions);
        Context.Database.EnsureCreated();
        SeedDatabase();
    }

    [OneTimeTearDown]
    public void CleanUp() { Context.Database.EnsureDeleted(); }

    private void SeedDatabase()
    {
        var newInvestments = new List<Investment>
        {
            new Investment
            {
                Name = "Investment 1",
                StartDate = DateTime.Parse("2022-03-01"),
                InterestType = "Simple",
                InterestRate = 3.875,
                PrincipalAmount = 10000
            },
            new Investment
            {
                Name = "Investment 2",
                StartDate = DateTime.Parse("2022-04-01"),
                InterestType = "Simple",
                InterestRate = 4,
                PrincipalAmount = 15000
            },
            new Investment
            {
                Name = "Investment 3",
                StartDate = DateTime.Parse("2022-05-01"),
                InterestType = "Compound",
                InterestRate = 5,
                PrincipalAmount = 20000
            }
        };
        Context.Investments.AddRange(newInvestments);
        Context.SaveChanges();
    }
}
