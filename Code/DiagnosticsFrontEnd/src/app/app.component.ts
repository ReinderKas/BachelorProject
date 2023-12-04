import { ChangeDetectorRef, Component } from '@angular/core';
import { WorkingModels } from 'src/models/workingModels';
import { BrokenModels } from 'src/models/brokenModels';
import { FmGraphResult } from 'src/models/fmGraphResult';
import { UnsatCoreResult } from 'src/models/unsatCoreResult';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  title = 'Z3 Theorem Prover';

  protected proof: string = "";
  protected unsatisfiableCore: UnsatCoreResult | null = null;
  protected fmGraph: FmGraphResult | null = null;
  public modelToDiagnose: string = BrokenModels.models[0];
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

  public selectBrokenModel(modelIndex: number) {
    this.modelToDiagnose = BrokenModels.models[modelIndex];
  }

  public brokenModelCount(){
    return Array.from({ length: BrokenModels.models.length }, (_, index) => index);
  }
  
  public selectWorkingModel(modelIndex: number) {
    this.modelToDiagnose = WorkingModels.models[modelIndex];
  }

  public workingModelCount(){
    return Array.from({ length: WorkingModels.models.length }, (_, index) => index);
  }

  public toggleGraph(){
    this.interactiveGraph = !this.interactiveGraph
    this.changeDetector.detectChanges()  
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
      let dict = await response.json();
      this.fmGraph = new FmGraphResult(dict.nodes, dict.edges);
    })
    .catch((error) => {
      alert("Something went wrong trying to create a Feature Model graph for the model: \n\n" + error);
    });
  }

  public getUnsatCore(){
    this.resetVariables();

    fetch("http://localhost/diagnose/unsatCore", {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(this.modelToDiagnose),
    })
    .then(async (response) => {
      let dict = await response.json();
      this.unsatisfiableCore = new UnsatCoreResult(dict.unsatisfiableCore, dict.nodes, dict.edges)
    })
    .catch((error) => {
      alert("Something went wrong trying to find proof for the model: \n\n" + error);
    });
  }
}