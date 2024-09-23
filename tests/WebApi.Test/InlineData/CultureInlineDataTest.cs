using System.Collections;

namespace WebApi.Test.InlineData;
public class CultureInlineDataTest : IEnumerable<object[]> // Uma lista de lista
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "pt-BR" }; // estou devolvendo o valor dessa função para a outra função que está a utilizando. E apos acabar ela continua a execução do código
        yield return new object[] { "en" };
        yield return new object[] { "pt-PT" };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
