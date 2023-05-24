namespace InvestmentAppProd.Models;

public class Investment
{
    [Required]
    [Key]
    public string Name { get; set; }

    public DateTime StartDate { get; set; }

    public string InterestType { get; set; }

    public double InterestRate { get; set; }

    public double PrincipalAmount { get; set; }

    public double CurrentValue { get; set; } = 0;

    public Investment()
    {
    }

    public Investment(string name, DateTime startDate, string interestType, double rate, double principal)
    {
        Name = name;
        StartDate = startDate;
        InterestType = interestType;
        InterestRate = rate;
        PrincipalAmount = principal;
    }
}
