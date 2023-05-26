namespace InvestmentAppProd.Models;

// this needs to be converted to POCO class to be used for only DB
// when a Domain layer is created then we can add mapping and keep logic in domain layer
public class Investment
{
    [Required]
    [Key]
    public string Name { get; set; }

    public DateTime StartDate { get; set; }

    public InterestType InterestType { get; set; }

    public double InterestRate { get; set; }

    public double PrincipalAmount { get; set; }
    
    // Do not persist a value to DB that can be calculated on the fly
    [NotMapped]
    public double CurrentValue => this.CalculateInterest();

    public Investment()
    {
    }

    public Investment(string name, DateTime startDate, InterestType interestType, double rate, double principal)
    {
        Name = name;
        StartDate = startDate;
        InterestType = interestType;
        InterestRate = rate;
        PrincipalAmount = principal;
    }
}
