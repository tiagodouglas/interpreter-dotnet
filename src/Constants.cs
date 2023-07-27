using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interpreter_dotnet
{
    internal static class Constants
    {
        public const string ILLEGAL = "ILLEGAL";
        public const string IDENT = "IDENT";
        public const string EOF = "EOF";
        public const string INT = "INT";

        public const string ASSIGN = "=";
        public const string PLUS = "+";
        public const string MINUS = "-";
        public const string BANG = "!";
        public const string ASTERISK = "*";
        public const string SLASH = "/";

        public const string LT = "<";
        public const string GT = ">";

        public const string EQ = "==";
        public const string NOT_EQ = "!=";

        public const string COMMA = ",";
        public const string SEMICOLON = ";";

        public const string LPAREN = "(";
        public const string RPAREN = ")";
        public const string LBRACE = "{";
        public const string RBRACE = "}";

        public const string FUNCTION = "FUNCTION";
        public const string LET = "LET";
        public const string FALSE = "FALSE";
        public const string TRUE = "TRUE";
        public const string IF = "IF";
        public const string ELSE = "ELSE";
        public const string RETURN = "RETURN";
    }
}
