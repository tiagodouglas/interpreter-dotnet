using System.Text;

namespace interpreter_dotnet.ast;

internal class BlockStatement : IStatement
{
    public BlockStatement(Token token)
    {
        Token = token;
    }

    public Token Token { get; set; }
    public List<IStatement> Statements { get; set; }

    public void StatementNode()
    {
    }

    public string String()
    {
        var output = new StringBuilder();

        foreach (var item in Statements)
        {
            output.Append(item.String());
        }

        return output.ToString();
    }

    public string TokenLiteral()
    {
        return Token.Literal; 
    }
}
