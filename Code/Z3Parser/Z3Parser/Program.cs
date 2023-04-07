
var toParse = "(mp (asserted (= a (not a))) (rewrite (= (= a (not a)) false)) false)))";



AntlrInputStream input = new AntlrInputStream("2 + 3 * 4");
MyLangLexer lexer = new MyLangLexer(input);
CommonTokenStream tokens = new CommonTokenStream(lexer);
MyLangParser parser = new MyLangParser(tokens);

MyLangParser.ExprContext tree = parser.expr();


var pause = true;