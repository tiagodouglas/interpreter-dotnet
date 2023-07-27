using System.Text;

namespace interpreter_dotnet.ast;

internal class ProgramCode
{
    public IStatement[]? Statements { get; set; }

    public string TokenLiteral()
    {
        if (Statements?.Length > 0)
        {
            return Statements[0].TokenLiteral();
        }
        else
        {
            return string.Empty;
        }
    }

    public string String()
    {
        var output = new StringBuilder();

        if (Statements != null)
            foreach (var s in Statements)
            {
                output.Append(s.String());
            }

        return output.ToString();
    }
}
