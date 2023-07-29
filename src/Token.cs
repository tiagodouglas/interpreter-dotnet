namespace interpreter_dotnet;

internal class Token
{
    public string? Type { get; set; }
    public string? Literal { get; set; }

    private readonly Dictionary<string, string> _keywords = new()
    {
        { "fn", Constants.FUNCTION },
        { "let", Constants.LET },
        { "true", Constants.TRUE },
        { "false", Constants.FALSE },
        { "if", Constants.IF },
        { "else", Constants.ELSE },
        { "return", Constants.RETURN }
    };

    public Token()
    {
    }

    public Token(string? tokenType, string? literal)
    {
        Type = tokenType;
        Literal = literal;
    }

    public string LookupIdent(string ident)
    {
        if (_keywords.TryGetValue(ident, out var tok))
        {
            return tok;
        }

        return Constants.IDENT;
    }
}
