namespace interpreter_dotnet.ast;

internal class Boolean: IExpression
{
    public Boolean(Token token, bool value)
    {
        Token = token;
        Value = value;
    }

    public Token Token { get; set; }
    public bool Value { get; set; }

    public void ExpressionNode()
    {
    }

    public string String()
    {
        return Token.Literal;
    }

    public string TokenLiteral()
    {
        return Token.Literal;
    }
}
