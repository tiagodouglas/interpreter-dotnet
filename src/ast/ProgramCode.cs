using System.Text;

namespace interpreter_dotnet.ast;

internal class ProgramCode
{
    public List<IStatement?> Statements { get; set; }

    public string TokenLiteral()
    {
        if (Statements?.Count > 0)
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
