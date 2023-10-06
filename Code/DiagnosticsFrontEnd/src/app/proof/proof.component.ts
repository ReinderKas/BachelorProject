import { Component, ViewEncapsulation, Input, AfterViewInit } from '@angular/core';

@Component({
  selector: 'app-proof',
  templateUrl: './proof.component.html',
  styleUrls: ['./proof.component.css'],
  encapsulation: ViewEncapsulation.None
})

export class ProofComponent implements AfterViewInit {
    @Input() proof: string | null = null;


    ngAfterViewInit(): void {
      // Nothing to do as of yet.
    }
}