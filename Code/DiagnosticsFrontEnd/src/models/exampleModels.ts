export class ExampleModels {
    public static models: string[] =  [
        "model [root] { \n"
          + "    [root] - mandatory -> [mandatory1] \n"
          + "    [mandatory1] - excludes -> [mandatory1] \n"
          + "}",


        "model [root] { \n"
          + "    [root] - mandatory -> [mandatory1] \n"
          + "    [root] - mandatory -> [mandatory2] \n"
          + "    [mandatory1] - excludes -> [mandatory2] \n"
          + "    [mandatory2] - requires -> [mandatory1] \n"
          + "}",


          "model [root] { \n"
          + "    [root] - mandatory -> [mandatory1] \n"
          + "    [root] - mandatory -> [mandatory2] \n"
          + "    [root] - alternative -> [alt1] \n"
          + "    [root] - alternative -> [alt2] \n"
          + "    [mandatory1] - excludes -> [mandatory2] \n"
          + "    [alt1] - requires -> [alt2] \n"
          + "}",
          
          "model [root] { \n"
          + "    [root] - mandatory -> [mandatory1] \n"
          + "    [root] - mandatory -> [mandatory2] \n"
          + "    [root] - alternative -> [alt1] \n"
          + "    [root] - alternative -> [alt2] \n"
          + "\n"
          + "    [alt1] - alternative -> [alt11] \n"
          + "    [alt1] - alternative -> [alt12] \n"
          + "    [alt1] - alternative -> [alt13] \n"
          + "    [alt1] - alternative -> [alt14] \n"
          + "\n"
          + "    [alt2] - alternative -> [alt21] \n"
          + "    [alt2] - alternative -> [alt22] \n"
          + "    [alt2] - alternative -> [alt23] \n"
          + "    [alt2] - alternative -> [alt24] \n"
          + "\n"
          + "    [alt12] - alternative -> [alt121] \n"
          + "    [alt12] - alternative -> [alt122] \n"
          + "}",

          
          "model [root] { \n"
          + "    [root] - mandatory -> [mandatory1] \n"
          + "    [root] - mandatory -> [mandatory2] \n"
          + "    [root] - alternative -> [alt1] \n"
          + "    [root] - alternative -> [alt2] \n"
          + "\n"
          + "    [alt1] - alternative -> [alt11] \n"
          + "    [alt1] - alternative -> [alt12] \n"
          + "    [alt1] - alternative -> [alt13] \n"
          + "    [alt1] - alternative -> [alt14] \n"
          + "\n"
          + "    [alt2] - alternative -> [alt21] \n"
          + "    [alt2] - alternative -> [alt22] \n"
          + "    [alt2] - alternative -> [alt23] \n"
          + "    [alt2] - alternative -> [alt24] \n"
          + "\n"
          + "    [alt12] - alternative -> [alt121] \n"
          + "    [alt12] - alternative -> [alt122] \n"
          + "\n"
          + "    [alt2] - requires-> [alt1]  \n"
          + "    [alt1] - requires-> [alt2]  \n"
          + "}",

          
          "model [root] { \n"
          + "    [root] - mandatory -> [mandatory1] \n"
          + "    [root] - mandatory -> [mandatory2] \n"
          + "    [root] - alternative -> [Conflicting Alternative 1] \n"
          + "    [root] - alternative -> [Conflicting Alternative 2] \n"
          + "\n"
          + "    [Conflicting Alternative 1] - alternative -> [alt11] \n"
          + "    [Conflicting Alternative 1] - alternative -> [alt12] \n"
          + "    [Conflicting Alternative 1] - alternative -> [alt13] \n"
          + "    [Conflicting Alternative 1] - alternative -> [alt14] \n"
          + "\n"
          + "    [Conflicting Alternative 2] - alternative -> [alt21] \n"
          + "    [Conflicting Alternative 2] - alternative -> [alt22] \n"
          + "    [Conflicting Alternative 2] - alternative -> [alt23] \n"
          + "    [Conflicting Alternative 2] - alternative -> [alt24] \n"
          + "\n"
          + "    [alt12] - alternative -> [alt121] \n"
          + "    [alt12] - alternative -> [alt122] \n"
          + "\n"
          + "    [Conflicting Alternative 1] - requires-> [Conflicting Alternative 2]  \n"
          + "    [Conflicting Alternative 2] - requires-> [Conflicting Alternative 1]  \n"
          + "}"
      ];

      
}