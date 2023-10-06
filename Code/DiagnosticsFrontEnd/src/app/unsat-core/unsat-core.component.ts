import { AfterViewInit, Component, Input } from '@angular/core';
import { UnsatCoreResult } from 'src/models/unsatCoreResult';

@Component({
  selector: 'app-unsat-core',
  templateUrl: './unsat-core.component.html',
  styleUrls: ['./unsat-core.component.css']
})
export class UnsatCoreComponent implements AfterViewInit {
  @Input() unsatisfiableCore: UnsatCoreResult = new UnsatCoreResult([], [], []);



  
  ngAfterViewInit(): void {
    console.log(this.unsatisfiableCore)
    // Nothing to do as of yet.
  }
}
