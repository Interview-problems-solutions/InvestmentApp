namespace InvestmentAppProd.Commands
{
    public interface IError
    {
        public ErrorType ErrorType { get; }
    }

    public class Error : IError
    {
        public Error(ErrorType errorType)
        {
            ErrorType = errorType;
        }

        public ErrorType ErrorType { get; set; }
    }

    public enum ErrorType
    {
        StartDateInFuture,
        AlreadyExists,
        DoesNotExit,
        NameMisMatch,
    }
}
