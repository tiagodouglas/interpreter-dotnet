using System.Linq.Expressions;
namespace interpreter_dotnet.ast
{
    internal class ExpressionStatement : IStatement
    {
        public Token Token { get; set; }
        public IExpression Value { get; set; }

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
            if (Value != null)
            {
                return Value.String();
            }

            return string.Empty;
        }

        public string TokenLiteral()
        {
            return Token.Literal;
        }
    }
}