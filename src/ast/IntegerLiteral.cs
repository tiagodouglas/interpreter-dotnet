namespace interpreter_dotnet.ast
{
    internal class IntegerLiteral: IExpression
    {
        public Token Token { get; set; }
        public int Value { get; set; }

        public IntegerLiteral(Token token)
        {
            Token = token;
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
            return  Token.Literal;
        }
    }
}