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
          + "    [mandatory1] - excludes -> [alt1] \n"
          + "    [mandatory2] - excludes -> [alt2] \n"
          + "}"
      ];
}