import { ChangeDetectorRef, Component } from '@angular/core';
import { ExampleModels } from 'src/models/exampleModels';
import { FmGraphResult } from 'src/models/fmGraphResult';
import { UnsatCoreResult } from 'src/models/unsatCoreResult';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  title = 'Z3 Theorem Prover';

  protected api: string = "http://localhost";





  protected proof: string = "";
  protected unsatisfiableCore: UnsatCoreResult | null = null;
  protected fmGraph: FmGraphResult | null = null;
  public modelToDiagnose: string = ExampleModels.models[0];
  public interactiveGraph: boolean = false;


  public componentHeight = 500;
  public componentWidth = 1500;
  public nodeSize = 25;
  public nodeSpacing = 1;


  constructor(
    private changeDetector : ChangeDetectorRef
  ) {}

  public resetVariables(){
    this.proof = "";
    this.fmGraph = null;
    this.unsatisfiableCore = null;
  }

  public selectModel(modelIndex: number) {
    this.modelToDiagnose = ExampleModels.models[modelIndex];
  }

  public modelCount(){
    return Array.from({ length: ExampleModels.models.length }, (_, index) => index);
  }

  public toggleGraph(){
    this.interactiveGraph = !this.interactiveGraph
    this.changeDetector.detectChanges()  
  }

  public proveModel(){
    this.resetVariables();

    fetch(this.api + "/diagnose/proof", {
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

    fetch(this.api + "/diagnose/featureModel", {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(this.modelToDiagnose),
    })
    .then(async (response) => {
      let dict = await response.json();
      this.fmGraph = new FmGraphResult(dict.nodes, dict.edges);
    })
    .catch((error) => {
      alert("Something went wrong trying to create a Feature Model graph for the model: \n\n" + error);
    });
  }

  public getUnsatCore(){
    this.resetVariables();

    fetch(this.api + "/diagnose/unsatCore", {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(this.modelToDiagnose),
    })
    .then(async (response) => {
      let dict = await response.json();
      this.unsatisfiableCore = new UnsatCoreResult(dict.unsatisfiableCore, dict.nodes, dict.edges, dict.toRemove)
    })
    .catch((error) => {
      alert("Something went wrong trying to find proof for the model: \n\n" + error);
    });
  }
}