import { Component } from '@angular/core';
import { ExampleModels } from 'src/models/exampleModels';
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
  public modelToDiagnose: string = ExampleModels.models[0];


  public resetVariables(){
    this.proof = "";
    this.fmGraph = null;
  }

  public selectModel(modelIndex: number) {
    this.modelToDiagnose = ExampleModels.models[modelIndex];
  }

  public modelCount(){
    return Array.from({ length: ExampleModels.models.length }, (_, index) => index);
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