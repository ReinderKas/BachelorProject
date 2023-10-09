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
          + "    [root] - mandatory -> [mandatory3] \n"
          + "\n"
          + "    [mandatory1] - mandatory -> [mandatory11] \n"
          + "    [mandatory1] - mandatory -> [mandatory12] \n"
          + "    [mandatory1] - mandatory -> [mandatory13] \n"
          + "    [mandatory1] - mandatory -> [mandatory14] \n"
          + "\n"
          + "    [mandatory11] - mandatory -> [mandatory111] \n"
          + "    [mandatory11] - mandatory -> [mandatory112] \n"
          + "    [mandatory11] - mandatory -> [mandatory113] \n"
          + "    [mandatory11] - mandatory -> [mandatory114] \n"
          + "\n"
          + "    [mandatory2] - mandatory -> [mandatory21] \n"
          + "    [mandatory2] - mandatory -> [mandatory22] \n"
          + "    [mandatory2] - mandatory -> [mandatory23] \n"
          + "    [mandatory2] - mandatory -> [mandatory24] \n"
          + "\n"
          + "    [mandatory113] - excludes -> [mandatory22] \n"
          + "}",
      ];
}