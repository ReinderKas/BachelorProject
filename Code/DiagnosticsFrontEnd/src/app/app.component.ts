import { Component, ElementRef } from '@angular/core';
import { FmGraphResult } from 'src/models/fmGraphResult';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  title = 'DiagnosticsFrontEnd';

  public proof: string = "";
  public fmGraph: FmGraphResult | null = null;

  public models: string[] = 
                  [
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
                  ]

  public modelToDiagnose: string = "model [root] { \n"
                                  + "    [root] - mandatory -> [mandatory1] \n"
                                  + "    [mandatory1] - excludes -> [mandatory1] \n"
                                  + "}"

  public selectModel(model: string) {
    this.modelToDiagnose = model
  }

  public resetVariables(){
    this.proof = "";
    this.fmGraph = null;
  }

  public proveModel(){
    this.resetVariables();

    fetch("http://localhost/diagnose/proof", {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(this.modelToDiagnose),
    })
    .then(async (response) => {
      this.proof = await response.text()
    })
    .catch((error) => {
      alert("Something went wrong trying to find proof for the model: \n\n" + error);
    });
  }

  public getFeatureModel(){
    this.resetVariables();

    fetch("http://localhost/diagnose/featureModel", {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(this.modelToDiagnose),
    })
    .then(async (response) => {
      this.fmGraph = await response.json() as FmGraphResult;
      console.log(this.fmGraph);

      this.drawGraph();
    })
    .catch((error) => {
      alert("Something went wrong trying to create a Feature Model graph for the model: \n\n" + error);
    });
  }

  private drawGraph(){
    console.log("Draw Graph Function.")
  }
}