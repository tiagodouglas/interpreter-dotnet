using System.Text;

namespace interpreter_dotnet.ast;

internal class PrefixExpression: IExpression
{
    public Token Token { get; set; }
    public string Operator { get; set; }
    public IExpression Right { get; set; }

    public PrefixExpression(Token token, string op)
    {
        Token = token;
        Operator = op;
    }

    public void ExpressionNode()
    {
        throw new NotImplementedException();
    }

    public string String()
    {
         var output =  new StringBuilder();

        output.Append("(");
        output.Append(Operator);
        output.Append(Right.String());
        output.Append(")");

        return output.ToString();
    }

    public string TokenLiteral()
    {
        return Token.Literal;
    }
}