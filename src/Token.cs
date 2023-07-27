using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interpreter_dotnet
{
    internal class Token
    {
        public string? Type { get; set; }
        public string? Literal { get; set; }

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
            var keywords = new Dictionary<string, string>
            {
                { "fn", Constants.FUNCTION },
                { "let", Constants.LET},
                { "true", Constants.TRUE},
                { "false", Constants.FALSE },
                { "if", Constants.IF},
                { "else", Constants.ELSE},
                { "return", Constants.RETURN }
            };

            var result = keywords.FirstOrDefault(x => x.Key == ident);

            return string.IsNullOrEmpty(result.Value) ? Constants.IDENT : result.Value;
        }
    }
}
