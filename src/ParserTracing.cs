namespace interpreter_dotnet
{
    internal static class ParserTracing
    {
        static int TraceLevel = 0;
        static string TraceIdentPlaceholder = "\t";

        internal static string IdentLevel()
        {
            return new string(TraceIdentPlaceholder[0], TraceLevel - 1);
        }

        internal static void TracePrint(string fs)
        {
            Console.WriteLine("{0}{1}", IdentLevel(), fs);
        }

        internal static void IncIdent()
        {
            TraceLevel++;
        }

        internal static void DecIdent()
        {
            TraceLevel--;
        }

        internal static string Trace(string msg)
        {
            IncIdent();
            TracePrint("BEGIN " + msg);
            return msg;
        }

        internal static void Untrace(string msg)
        {
            TracePrint("END " + msg);
            DecIdent();
        }

    }
}