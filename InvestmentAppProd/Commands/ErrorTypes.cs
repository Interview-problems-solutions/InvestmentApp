namespace InvestmentAppProd.Commands;

// Use Clearly defined errors instead of exceptions or magical strings
// we can send them to front-end to translate to proper error messages
public interface IError
{
    public ErrorType ErrorType { get; }
}

public class Error : IError
{
    public Error(ErrorType errorType) { ErrorType = errorType; }

    public ErrorType ErrorType { get; set; }
}

public enum ErrorType
{
    StartDateInFuture,
    AlreadyExists,
    DoesNotExit,
    NameMisMatch,
}
