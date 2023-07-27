using Microsoft.VisualBasic;
using System.Diagnostics.Metrics;

namespace interpreter_dotnet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Progamming language made for fun with C# after reading the book Writing An interpreter In Go");
            while (true)
            {
                Console.Write(">> ");
                var line = Console.ReadLine();

                var lexer = new Lexer(line);

                for (var tok = lexer.NextToken(); tok.Type != Constants.EOF; tok = lexer.NextToken())
                {
                    Console.WriteLine(tok.Type);
                }
            }
        }
    }
}