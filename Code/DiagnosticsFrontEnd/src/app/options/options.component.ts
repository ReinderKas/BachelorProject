import { Component, Inject,  OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DiagnoseOptions } from 'src/models/diagnoseDialogType';

@Component({
  selector: 'app-options',
  templateUrl: './options.component.html',
  styleUrls: ['./options.component.css']
})

export class OptionsComponent implements OnInit {
  public options: DiagnoseOptions[] = []; // Adjust type as needed
  public selectedOption: DiagnoseOptions | undefined;

  constructor(
    public dialogRef: MatDialogRef<OptionsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {   
                                              model: string;
                                              nodeId: string;
                                              nodeName: string;
                                          } 
    ) {}

  ngOnInit() {
    console.log(this.data.nodeId)
    fetch('http://localhost/diagnose/options/' + this.data.nodeName, {
      method: 'PUT',
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(this.data.model)
    })
    .then(async (response) => {
      this.options = await response.json();
    });
  }

  getOptionText(item: DiagnoseOptions): string{
    switch(item){
      case DiagnoseOptions.Optional:
        return "Turn into Optional";
      case DiagnoseOptions.Delete:
        return "Delete X-Tree Constraint";
      default:
        return "Unknown option";
    }
  }

  selectOption(item: DiagnoseOptions): void {
    if (this.selectedOption === item){
      this.selectedOption = undefined
    } else {
      this.selectedOption = item;
    }
  }

  close(): void {
    this.dialogRef.close(this.selectedOption);
  }
}