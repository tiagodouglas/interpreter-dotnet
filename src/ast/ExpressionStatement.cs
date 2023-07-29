namespace interpreter_dotnet.ast;

internal class ExpressionStatement : IStatement
{
    public Token Token { get; set; }
    public IExpression Expression { get; set; }

    public ExpressionStatement(Token token)
    {
        Token = token;
    }

    public void StatementNode()
    {
        throw new NotImplementedException();
    }

    public string String()
    {
        if (Expression != null)
        {
            return Expression.String();
        }

        return string.Empty;
    }

    public string TokenLiteral()
    {
        return Token.Literal;
    }
}