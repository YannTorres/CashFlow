using System.Net;

namespace CashFlow.Exception.ExceptionBase;
public class ErrorOnValidationException : CashFlowException
{
    public ErrorOnValidationException(List<string> errorMessages)
    {
        _errors = errorMessages;
    }

    private readonly List<string> _errors;

    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public override List<string> GetErrors()
    {
        return _errors;
    }
}
