using System.Text;

namespace interpreter_dotnet.ast;

internal class ReturnStatement : IStatement
{
    public Token Token { get; set; }
    public IExpression Value { get; set; }

    public ReturnStatement(Token token)
    {
        Token = token;
    }

    public void StatementNode()
    {
        throw new NotImplementedException();
    }

    public string String()
    {
        var output = new StringBuilder();

        output.Append(TokenLiteral() + " ");

        if (Value != null)
        {
            output.Append(Value.String);
        }
        
        output.Append(";");

        return output.ToString();
    }

    public string TokenLiteral()
    {
        return Token.Literal;
    }
}