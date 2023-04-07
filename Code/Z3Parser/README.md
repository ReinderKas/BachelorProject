# To generate the parser:
Move to the [ParserObject](https://github.com/ReinderKas/BachelorProject/tree/main/Code/Z3Parser/Z3Parser/ParserObjects) folder where the `.g4` grammar file exists. <br>
In the folder run the following command:

```Console
antlr4 -Dlanguage=CSharp z3proof.g4
```