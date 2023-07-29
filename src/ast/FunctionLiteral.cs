using System.Text;

namespace interpreter_dotnet.ast;

internal class FunctionLiteral : IExpression
{
    public FunctionLiteral(Token token)
    {
        Token = token;
    }

    public Token Token { get; set; }
    public List<IExpression?> Parameters { get; set; }
    public IStatement Body { get; set; }


    public void ExpressionNode()
    {
    }

    public string String()
    {
        var output = new StringBuilder();

        var paramList = new string[] { };

        foreach (var param in Parameters)
        {
            paramList.Append(param.String());
        }

        output.Append(Token.Literal);
        output.Append('(');
        output.Append(string.Join(", ", paramList));
        output.Append(")");
        output.Append(Body.String());

        return output.ToString();
    }

    public string TokenLiteral()
    {
        return Token.Literal;
    }
}
