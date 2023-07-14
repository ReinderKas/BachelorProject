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
      
      public static phoneModel = {
        name: 'Phone',
        children: [
          {
            name: 'Screen',
            children: [
              { 
                name: 'Size' ,
                children: [
                  { 
                    name: 'Huge' ,
                    children: []
                  },
                  { 
                    name: 'Big' ,
                    children: []
                  },
                  { 
                    name: 'Medium' ,
                    children: []
                  },
                  { 
                    name: 'Small' ,
                    children: []
                  }
                ]
              },
              { 
                name: 'Resolution' ,
                children: []
              },
            ],
          },
          {
            name: 'OS',
            children: [
              { 
                name: 'Android' ,
                children: []
              },
              { 
                name: 'iOS' ,
                children: []
              },
            ],
          },
          {
            name: 'Battery' ,
            children: []
          },
        ],
      };
}