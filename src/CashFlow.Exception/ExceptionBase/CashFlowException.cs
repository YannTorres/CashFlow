namespace CashFlow.Exception.ExceptionBase;
public abstract class CashFlowException : System.Exception
{
    protected CashFlowException() { }
    protected CashFlowException(string message) : base(message) { }
    public abstract int StatusCode { get; }
    public abstract List<string> GetErrors();
}
