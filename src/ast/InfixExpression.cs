using System.Text;

namespace interpreter_dotnet.ast;

internal class InfixExpression : IExpression
{
    public Token Token { get; set; }
    public IExpression Left { get; set; }
    public IExpression Right { get; set; }
    public string Operator { get; set; }

    public InfixExpression(Token token, string op,  IExpression left)
    {
        Token = token;
        Operator = op;
        Left = left;
    }

    public void ExpressionNode()
    {
        throw new NotImplementedException();
    }

    public string String()
    {
        return Token.Literal;
    }

    public string TokenLiteral()
    {
        var output = new StringBuilder();

        output.Append("(");
        output.Append(Left.String());
        output.Append(" " + Operator + " ");
        output.Append(Right.String());
        output.Append(")");

        return output.ToString();
    }
}