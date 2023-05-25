namespace InvestmentAppProd.Models;

public class Investment
{
    [Required]
    [Key]
    public string Name { get; set; }

    public DateTime StartDate { get; set; }

    public InterestType InterestType { get; set; }

    public double InterestRate { get; set; }

    public double PrincipalAmount { get; set; }
    
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
