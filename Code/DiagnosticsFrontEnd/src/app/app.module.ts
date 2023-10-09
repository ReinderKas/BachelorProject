import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {MatDividerModule} from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import {MatCardModule} from '@angular/material/card';

import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { GraphComponent } from './graph/graph.component';
import { ProofComponent } from './proof/proof.component';
import { InteractiveGraphComponent } from './graph/interactive-graph.component';
import { UnsatCoreComponent } from './unsat-core/unsat-core.component';
import { UnsatCoreGraph } from './unsat-core/unsat-core-graph.component';

@NgModule({
  declarations: [
    AppComponent,
    GraphComponent,
    ProofComponent,
    InteractiveGraphComponent,
    UnsatCoreComponent,
    UnsatCoreGraph
  ],
  imports: [
    MatCardModule,
    MatListModule,
    MatDividerModule,
    BrowserModule,
    MatButtonModule,
    FormsModule,
    BrowserAnimationsModule, 
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
