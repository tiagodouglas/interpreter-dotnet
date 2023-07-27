namespace interpreter_dotnet.ast
{
    internal class Identifier: IExpression
    {
        public Identifier(Token token, string value)
        {
            Token = token;
            Value = value;
        }

        public Token Token { get; set; }
        public string Value { get; set; }

        public void ExpressionNode()
        {
            throw new NotImplementedException();
        }

        public string String()
        {
            return Value;
        }

        public string TokenLiteral()
        {
            throw new NotImplementedException();
        }
    }
}
