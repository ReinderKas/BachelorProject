export class WorkingModels {
    public static models: string[] =  [
/* Model 1 */
      "model [Phone] { \n"
      + "    [Phone] - mandatory -> [Calls] \n"
      + "    [Phone] - optional -> [GPS] \n"
      + "\n"
      + "    [Phone] - mandatory -> [Screen] \n"
      + "    [Screen] - alternative -> [Basic] \n"
      + "    [Screen] - alternative -> [Color] \n"
      + "    [Screen] - alternative -> [HD] \n"
      + "\n"
      + "    [Phone] - optional -> [Media] \n"
      + "    [Media] - or -> [Camera] \n"
      + "    [Media] - or -> [MP3] \n"
      + "\n"
      + "    [Camera] - requires -> [HD] \n"
      + "    [GPS] - excludes -> [Basic] \n"
      + "}",
               
/* Model 2 */
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
    ];
  }