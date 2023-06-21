import { Component } from '@angular/core';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'DiagnosticsFrontEnd';

  public proof: string = "";
  public models: string[] = 
                  [
                    "model [root] { \n"
                      + "    [root] - mandatory -> [mandatory1] \n"
                      + "    [root] - mandatory -> [mandatory2] \n"
                      + "    [mandatory1] - excludes -> [mandatory1] \n"
                      + "}",
                      "model [root] { \n"
                      + "    [root] - mandatory -> [mandatory1] \n"
                      + "    [root] - mandatory -> [mandatory2] \n"
                      + "    [root] - alternative -> [alt1] \n"
                      + "    [root] - alternative -> [alt2] \n"
                      + "    [mandatory1] - excludes -> [alt1] \n"
                      + "    [mandatory2] - excludes -> [alt2] \n"
                      + "}"
                  ]

  public modelToDiagnose: string = "model [root] { \n"
                                  + "    [root] - mandatory -> [mandatory1] \n"
                                  + "    [root] - mandatory -> [mandatory2] \n"
                                  + "    [mandatory1] - excludes -> [mandatory1] \n"
                                  + "}"

  public selectModel(model: string) {
    console.log("Selection: " + model)
    this.modelToDiagnose = model
  }

  public proveModel(){
    console.log("Model:" + this.modelToDiagnose)

    // con.setRequestProperty("Content-Type", "application/json; charset=utf8")

    fetch("http://localhost/diagnose", {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(this.modelToDiagnose),
    })
    .then(async (response) => {
      this.proof = await response.text()
      console.log(this.proof);
    })
    // .then((data) => {
    //   // Handle the response data
    //   console.log(data);
    // })
    .catch((error) => {
      // Handle any errors
      console.error("Could not find proof of the model: " + error);
    });
  }
}
 