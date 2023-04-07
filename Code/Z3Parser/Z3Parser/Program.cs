using Antlr4.Runtime;

internal class Program
{
    private static void Main(string[] args)
    {
        var toParse = "(mp (asserted (= a (not a))) (rewrite (= (= a (not a)) false)) false)))";



        AntlrInputStream input = new AntlrInputStream("2 + 3 * 4");
        z3proofLexer lexer = new z3proofLexer(input);
        CommonTokenStream tokens = new CommonTokenStream(lexer);
        z3proofParser parser = new z3proofParser(tokens);

        z3proofParser.ExprContext tree = parser.expr();


        var pause = true;
    }
}