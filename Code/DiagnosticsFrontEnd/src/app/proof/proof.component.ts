import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';

@Component({
  selector: 'app-proof',
  templateUrl: './proof.component.html',
  styleUrls: ['./proof.component.css'],
  encapsulation: ViewEncapsulation.None
})

export class ProofComponent implements OnInit {
    @Input() proof: string | null = null;


    
    ngOnInit(): void {
        console.log("Initializing with Proof: ");
        console.log(this.proof);
    }
}