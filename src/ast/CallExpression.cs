using System.Text;

namespace interpreter_dotnet.ast;

internal class CallExpression : IExpression
{
    public CallExpression(Token token, IExpression function)
    {
        Token= token;
        Function= function;
    }

    public Token Token { get; set; }
    public IExpression Function { get; set; }
    public List<IExpression> Arguments { get; set; }

    public void ExpressionNode()
    {
    }

    public string String()
    {
        var output = new StringBuilder();

        var args = new string[] {};

        foreach (var arg in Arguments)
        {
            args.Append(arg.String());
        }

        output.Append(Function.String());
        output.Append('(');
        output.Append(string.Join(", ", args));
        output.Append(')');

        return output.ToString();
    }

    public string TokenLiteral()
    {
        return Token.Literal;
    }
}
