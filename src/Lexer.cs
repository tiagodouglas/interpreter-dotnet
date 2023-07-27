using Microsoft.VisualBasic;

namespace interpreter_dotnet
{
    internal class Lexer
    {
        public string Input { get; set; }
        public int Position { get; set; }
        public int ReadPosition { get; set; }
        public char Ch { get; set; }

        public Lexer(string input)
        {
            Input = input;
            ReadChar();
        }

        public Token NextToken()
        {
            var token = new Token();
            SkipWhiteSpace();

            switch (Ch)
            {
                case '=':
                    if (PeekChar() == '=')
                    {
                        var ch = Ch;
                        ReadChar();
                        var literal = $"{ch}{Ch}";


                        token = new Token(Constants.EQ, literal);

                    }
                    else
                    {
                        token = new Token(Constants.ASSIGN, Ch.ToString());
                    }
                    break;
                case '+':
                    token = new Token(Constants.PLUS, Ch.ToString());
                    break;
                case '-':
                    token = new Token(Constants.MINUS, Ch.ToString());
                    break;
                case '!':
                    if (PeekChar() == '=')
                    {
                        var ch = Ch;
                        ReadChar();
                        var literal = $"{ch}{Ch}";
                        token = new Token(Constants.NOT_EQ, literal);
                    }
                    else
                    {
                        token = new Token(Constants.BANG, Ch.ToString());
                    }
                    break;
                case '/':
                    token = new Token(Constants.SLASH, Ch.ToString());
                    break;
                case '*':
                    token = new Token(Constants.ASTERISK, Ch.ToString());
                    break;
                case '<':
                    token = new Token(Constants.LT, Ch.ToString());
                    break;
                case '>':
                    token = new Token(Constants.GT, Ch.ToString());
                    break;
                case ';':
                    token = new Token(Constants.SEMICOLON, Ch.ToString());
                    break;
                case ',':
                    token = new Token(Constants.COMMA, Ch.ToString());
                    break;
                case '{':
                    token = new Token(Constants.LBRACE, Ch.ToString());
                    break;
                case '}':
                    token = new Token(Constants.RBRACE, Ch.ToString());
                    break;
                case '(':
                    token = new Token(Constants.LPAREN, Ch.ToString());
                    break;
                case ')':
                    token = new Token(Constants.RPAREN, Ch.ToString());
                    break;

                case (char)0:
                    token = new Token(Constants.EOF, string.Empty);
                    break;
                default:
                    if (IsLetter())
                    {
                        token.Literal = ReadIdentifier();
                        token.Type = token.LookupIdent(token.Literal);
                        return token;
                    }
                    else if (IsDigit())
                    {
                        token.Literal = ReadNumber();
                        token.Type = Constants.INT;
                        return token;
                    }
                    else
                    {
                        token = new Token(Constants.ILLEGAL, Ch.ToString());
                    }

                    break;
            };

            ReadChar();

            return token;
        }

        private void ReadChar()
        {
            if (ReadPosition >= Input.Length)
            {
                Ch = (char)0;
            }
            else
            {
                Ch = Input[ReadPosition];
            }

            Position = ReadPosition;
            ReadPosition += 1;
        }

        private Char PeekChar()
        {
            if (ReadPosition >= Input.Length)
            {
                return (char)0;
            }
            else
            {
                return Input[ReadPosition];
            }
        }

        private void SkipWhiteSpace()
        {
            if (Ch == ' ' || Ch == '\t' || Ch == '\n' || Ch == '\r')
            {
                ReadChar();
            }
        }

        private string ReadIdentifier()
        {
            var position = Position;
            while (IsLetter())
            {
                ReadChar();
            }

            return Input.Substring(position, Position - position);
        }

        private string ReadNumber()
        {
            var position = Position;
            while (IsDigit())
            {
                ReadChar();
            }

            return Input.Substring(position, Position - position);
        }

        private bool IsLetter()
        {
            return 'a' <= Ch && Ch <= 'z' || 'A' <= Ch && Ch <= 'Z' || Ch == '_';
        }

        private bool IsDigit()
        {
            return '0' <= Ch && Ch < '9';
        }
    }
}
