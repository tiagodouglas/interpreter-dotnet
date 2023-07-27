using System.Text;

namespace interpreter_dotnet.ast;

internal class IfExpression : IExpression
{
    public IfExpression(Token token)
    {
        Token = token;
    }

    public Token Token { get; set; }
    public IExpression Condition { get; set; }
    public IStatement Consequence { get; set; }
    public IStatement Alternative { get; set; }

    public void ExpressionNode()
    {
    }

    public string String()
    {
        var output = new StringBuilder();

        output.Append("if");
        output.Append(Condition.String());
        output.Append(" ");
        output.Append(Consequence.String());

        if (Alternative != null)
        {
            output.Append("else ");
            output.Append(Alternative.String());
        }

        return output.ToString();
    }

    public string TokenLiteral()
    {
        return Token.Literal;
    }
}
