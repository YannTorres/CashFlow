using System.Net;

namespace CashFlow.Exception.ExceptionBase;
public class NotFoundException : CashFlowException
{
    public NotFoundException(string errorMessage) : base(errorMessage) { }
    public override int StatusCode => (int)HttpStatusCode.NotFound;
    public override List<string> GetErrors()
    {
        return [Message];
    }
}
