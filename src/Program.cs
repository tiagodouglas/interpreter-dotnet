namespace interpreter_dotnet;

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
            var parser = new Parser(lexer);

            var program = parser.ParseProgram();

            if (parser.Errors.Count != 0)
            {
                PrintParserErrors(parser.GetErrors());
                continue;
            }

            Console.WriteLine(program.String());

            for (var tok = lexer.NextToken(); tok.Type != Constants.EOF; tok = lexer.NextToken())
            {
                Console.WriteLine(tok.Type);
            }
        }
    }

    static void PrintParserErrors(List<string> errors)
    {
        foreach (var err in errors)
        {
            Console.WriteLine(err);
        }
    }
}