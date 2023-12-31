﻿using System.Text;

namespace interpreter_dotnet.ast;

internal class LetStatement: IStatement
{
    public Token Token { get; set; }
    public IExpression Value { get; set; }
    public Identifier Name { get; set; }

    public LetStatement(Token token)
    {
        Token = token;
    }

    public void StatementNode()
    {
    }

    public string String()
    {
        var output =  new StringBuilder();

        output.Append(TokenLiteral() + " ");
        output.Append(Name.String());
        output.Append(" = ");

        if (Value != null)
        {
            output.Append(Value.String());
        }

        output.Append(";");
      
        return output.ToString();
    }

    public string TokenLiteral()
    {
        return Token.Literal;
    }
}
