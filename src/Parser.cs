using interpreter_dotnet.ast;

namespace interpreter_dotnet;

internal class Parser
{
    private enum PrecedencesEnum
    {
        _int,
        LOWEST,
        EQUALS,
        LESSGREATER,
        SUM,
        PRODUCT,
        PREFIX,
        CALL
    };

    private Dictionary<string, int> Precedences = new Dictionary<string, int>
    {
         {Constants.EQ, (int)PrecedencesEnum.EQUALS},
         {Constants.NOT_EQ,(int) PrecedencesEnum.EQUALS},
         {Constants.LT,(int) PrecedencesEnum.LESSGREATER},
         {Constants.GT, (int)PrecedencesEnum.LESSGREATER},
         {Constants.PLUS,(int) PrecedencesEnum.SUM},
         {Constants.MINUS,(int) PrecedencesEnum.SUM},
         {Constants.SLASH, (int)PrecedencesEnum.PRODUCT},
         {Constants.ASTERISK,(int) PrecedencesEnum.PRODUCT},
         {Constants.LPAREN,(int) PrecedencesEnum.CALL}
    };

    public delegate IExpression PrefixParseFn();

    public delegate IExpression InfixParseFn(IExpression expression);

    public Lexer Lexer { get; set; }
    public List<string> Errors { get; set; }
    public Token CurToken { get; set; }
    public Token PeekToken { get; set; }
    public ProgramCode Program { get; set; }

    private Dictionary<string, PrefixParseFn> PrefixParseFns;
    private Dictionary<string, InfixParseFn> InfixParseFns;

    public Parser(Lexer lexer)
    {
        Lexer = lexer;
        Errors = new List<string>();

        PrefixParseFns = new Dictionary<string, PrefixParseFn>();
        InfixParseFns = new Dictionary<string, InfixParseFn>();

        RegisterPrefix(Constants.IDENT, ParseIdentifier);
        RegisterPrefix(Constants.INT, ParseIntegerLiteral);
        RegisterPrefix(Constants.BANG, ParsePrefixExpression);
        RegisterPrefix(Constants.SLASH, ParsePrefixExpression);
        RegisterPrefix(Constants.ASTERISK, ParsePrefixExpression);
        RegisterPrefix(Constants.EQ, ParsePrefixExpression);
        RegisterPrefix(Constants.NOT_EQ, ParsePrefixExpression);
        RegisterPrefix(Constants.LT, ParsePrefixExpression);
        RegisterPrefix(Constants.GT, ParsePrefixExpression);
        RegisterPrefix(Constants.TRUE, ParseBoolean);
        RegisterPrefix(Constants.FALSE, ParseBoolean);
        RegisterPrefix(Constants.LPAREN, ParseGroupedExpression);
        RegisterPrefix(Constants.IF, ParseIfExpression);
        RegisterPrefix(Constants.FUNCTION, ParseFunctionLiteral);

        RegisterInfix(Constants.LPAREN, ParseCallExpression);

        NextToken();
        NextToken();
    }

    public ProgramCode ParseProgram()
    {
        Program = new ProgramCode();
        Program.Statements = new List<IStatement> { };

        while (CurToken.Type != Constants.EOF)
        {
            var stmt = ParseStatement();

            if (stmt != null)
            {
                Program.Statements.Add(stmt);
            }

            NextToken();
        }

        return Program;
    }

    public List<string> GetErrors()
    {
        return Errors;
    }

    private void PeekError(string tokenType)
    {
        var msg = $"Expected next token to be {tokenType}, got {PeekToken.Type}";
        Errors.Add(msg);
    }

    private IStatement ParseStatement()
    {
        switch (CurToken.Type)
        {
            case Constants.LET:
                return ParseLetStatement();
            case Constants.RETURN:
                return ReturnStatement();
            default:
                return null;
        }
    }

    private LetStatement ParseLetStatement()
    {
        var stmt = new LetStatement(CurToken);

        if (!ExpectPeek(Constants.IDENT))
        {
            return null;
        }

        stmt.Name = new Identifier(CurToken, CurToken.Literal);

        if (!ExpectPeek(Constants.ASSIGN))
        {
            return null;
        }

        while (!CurTokenIs(Constants.SEMICOLON))
        {
            NextToken();
        }

        return stmt;

    }

    private IExpression ParseExpression(int precendence)
    {
        var prefix = PrefixParseFns[CurToken.Type];

        if (prefix == null)
        {
            NoPrefixParseFnError(CurToken.Type);
            return null;
        }

        var leftExp = prefix();

        while (!PeekTokenIs(Constants.SEMICOLON) && precendence < PeekPrecedence())
        {
            var infix = InfixParseFns[PeekToken.Type];

            if (infix == null)
            {
                return leftExp;
            }

            NextToken();

            leftExp = infix(leftExp);

        }

        return leftExp;
    }

    private IExpression ParseIntegerLiteral()
    {
        var lit = new IntegerLiteral(CurToken);

        try
        {
            var value = int.Parse(CurToken.Literal);

            lit.Value = value;

        }
        catch (Exception ex)
        {
            Errors.Add($"could not parse {CurToken.Literal} as integer");
            return null;
        }

        return lit;

    }

    private IExpression ParsePrefixExpression()
    {
        var expression = new PrefixExpression(CurToken, CurToken.Literal);

        NextToken();

        expression.Right = ParseExpression((int)PrecedencesEnum.PREFIX);

        return expression;
    }

    private ReturnStatement ReturnStatement()
    {
        var stmt = new ReturnStatement(CurToken);

        NextToken();

        while (CurTokenIs(Constants.SEMICOLON))
        {
            NextToken();
        }

        return stmt;
    }

    private ExpressionStatement ExpressionStatement()
    {
        var stmt = new ExpressionStatement(CurToken);

        stmt.Value = ParseExpression(((int)PrecedencesEnum.LOWEST));

        if (PeekTokenIs(Constants.SEMICOLON))
        {
            NextToken();
        }

        return stmt;
    }

    private IExpression ParseBoolean()
    {
        return new ast.Boolean(CurToken, CurTokenIs(Constants.TRUE));
    }

    private IExpression ParseGroupedExpression()
    {
        NextToken();

        var exp = ParseExpression((int)PrecedencesEnum.LOWEST);

        if (ExpectPeek(Constants.RPAREN))
        {
            return null;
        }

        return exp;
    }

    private IExpression ParseIfExpression()
    {
        var expression = new IfExpression(CurToken);

        if (ExpectPeek(Constants.LPAREN))
        {
            return null;
        }

        NextToken();

        expression.Condition = ParseExpression((int)PrecedencesEnum.LOWEST);

        if (!ExpectPeek(Constants.RPAREN))
        {
            return null;
        }

        if (!ExpectPeek(Constants.LBRACE))
        {
            return null;
        }

        expression.Consequence = ParseBlockStatement();

        return expression;
    }

    private IStatement ParseBlockStatement()
    {
        var block = new BlockStatement(CurToken);
        block.Statements = new List<IStatement> { };

        NextToken();

        while (CurTokenIs(Constants.RBRACE) && !CurTokenIs(Constants.EOF))
        {
            var smt = ParseStatement();
            if (smt != null)
            {
                block.Statements.Add(smt);
            }
            NextToken();
        }

        return block;
    }

    private IExpression ParseFunctionLiteral()
    {
        var lit = new FunctionLiteral(CurToken);

        if (ExpectPeek(Constants.LPAREN))
        {
            return null;
        }

        lit.Parameters = ParseFunctionParameters();

        if (ExpectPeek(Constants.LBRACE))
        {
            return null;
        }

        lit.Body = ParseBlockStatement();

        return lit;
    }

    private IExpression ParseCallExpression(IExpression function)
    {
        var exp = new CallExpression(CurToken, function);
        exp.Arguments = ParseCallArguments();

        return exp;
    }

    private List<IExpression> ParseCallArguments()
    {
        var args = new List<IExpression> { };

        if (PeekTokenIs(Constants.RPAREN))
        {
            NextToken();
            return args;
        }

        NextToken();

        args.Add(ParseExpression((int)PrecedencesEnum.LOWEST));

        while (PeekTokenIs(Constants.COMMA))
        {
            NextToken();
            NextToken();
            args.Add(ParseExpression((int)PrecedencesEnum.LOWEST));
        }

        if (!ExpectPeek(Constants.RPAREN))
        {
            return null;
        }

        return args;
    }

    private List<IExpression> ParseFunctionParameters()
    {
        var identifiers = new List<IExpression> { };

        if (PeekTokenIs(Constants.RPAREN))
        {
            NextToken();
            return identifiers;
        }

        NextToken();

        var ident = new Identifier(CurToken, CurToken.Literal);
        identifiers.Add(ident);

        while (PeekTokenIs(Constants.COMMA))
        {
            NextToken();
            NextToken();

            ident = new Identifier(CurToken, CurToken.Literal);
            identifiers.Add(ident);
        }

        if (ExpectPeek(Constants.RPAREN))
        {
            return null;
        }

        return identifiers;
    }



    private bool CurTokenIs(string tokenType)
    {
        return CurToken.Type == tokenType;
    }

    private bool PeekTokenIs(string tokenType)
    {
        return PeekToken.Type == tokenType;
    }

    private bool ExpectPeek(string tokenType)
    {
        if (PeekTokenIs(tokenType))
        {
            NextToken();
            return true;
        }

        PeekError(tokenType);

        return false;
    }

    private int PeekPrecedence()
    {
        var precendence = Precedences[PeekToken.Type];

        if (precendence != default)
        {
            return precendence;
        }

        return (int)PrecedencesEnum.LOWEST;
    }

    private int CurPrecedence()
    {
        var precendence = Precedences[CurToken.Type];

        if (precendence != default)
        {
            return precendence;
        }

        return (int)PrecedencesEnum.LOWEST;
    }

    private void NextToken()
    {
        CurToken = PeekToken;
        PeekToken = Lexer.NextToken();
    }

    private void RegisterPrefix(string tokenType, PrefixParseFn prefixFn)
    {
        PrefixParseFns[tokenType] = prefixFn;
    }

    private void RegisterInfix(string tokenType, InfixParseFn infixFn)
    {
        InfixParseFns[tokenType] = infixFn;
    }

    private IExpression ParseIdentifier()
    {
        return new Identifier(CurToken, CurToken.Literal);
    }

    private void NoPrefixParseFnError(string tokenType)
    {
        Errors.Add($"no prefix parse function for {tokenType} found");
    }

    private IExpression ParseInfixExpreesion(IExpression left)
    {
        var expression = new InfixExpression(CurToken, CurToken.Literal, left);

        var precendence = CurPrecedence();
        NextToken();

        if (expression.Operator == "+")
        {
            expression.Right = ParseExpression(precendence - 1);
        }
        else
        {
            expression.Right = ParseExpression(precendence);
        }

        return expression;
    }

    private void ParseExpression()
    {
        try
        {
            ParserTracing.Trace("parseExpressionStatement");
        }
        finally
        {
            ParserTracing.Untrace("parseExpressionStatement");
        }

    }
}
