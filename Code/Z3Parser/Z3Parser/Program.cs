using Antlr4.Runtime;

internal class Program
{
    private static void Main(string[] args)
    {



        // Broken Model 1
        Prove("(mp (asserted (= a (not a))) (rewrite (= (= a (not a)) false)) false)))");

        // Broken Model 2
        Prove("((proof" 
            + " (let (($x26 (= a b)))"
            + " (let (($x28 (not $x26)))"
            + " (let ((@x29 (asserted $x28)))"
            + " (let ((@x27 (asserted $x26)))"
            + " (unit-resolution @x27 (mp @x29 (rewrite (= $x28 $x28)) $x28) false)))))))");

        // Broken Model 3
        Prove("(let ((@x47 (asserted a))) (unit-resolution (asserted (or (not a) (not b))) (unit-resolution (asserted (or b (not a))) @x47 b) @x47 false))");

        // Broken Model 4
        Prove("(let ((@x60 (asserted a))) (unit-resolution (asserted (or (not a) (not b))) (unit-resolution (asserted (or b (not a))) @x60 b) @x60 false))");


        var pause = true;
    }

    private static void Prove(string proof){
        AntlrInputStream input = new AntlrInputStream(proof);
        z3proofLexer lexer = new z3proofLexer(input);
        CommonTokenStream tokens = new CommonTokenStream(lexer);
        z3proofParser parser = new z3proofParser(tokens);

        z3proofParser.ExprContext tree = parser.expr();
    }
}