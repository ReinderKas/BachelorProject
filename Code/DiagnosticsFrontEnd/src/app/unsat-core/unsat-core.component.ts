import { AfterViewInit, Component, Input } from '@angular/core';
import { FmGraphResult } from 'src/models/fmGraphResult';
import { UnsatCoreResult } from 'src/models/unsatCoreResult';

@Component({
  selector: 'app-unsat-core',
  templateUrl: './unsat-core.component.html',
  styleUrls: ['./unsat-core.component.css']
})
export class UnsatCoreComponent implements AfterViewInit {
  @Input() unsatisfiableCore: UnsatCoreResult = new UnsatCoreResult([], [], [], []);
  @Input() modelToDiagnose: string = "";
  @Input() componentHeight: number = 500;
  @Input() componentWidth: number = 500;
  @Input() nodeSize: number = 15;
  @Input() nodeSpacing: number = 1;

  
  protected api: string = "http://localhost";

  public fmGraph: FmGraphResult | null = null;


 
  
  ngAfterViewInit(): void {
    this.getFeatureModel();
    console.log(this.unsatisfiableCore);
  }

  public async getFeatureModel(){
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
      console.log(this.fmGraph)
    })
    .catch((error) => {
      alert("Something went wrong trying to create a Feature Model graph for the model: \n\n" + error);
    });
  }
}
