import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";

export interface InfoDialogData {
  title: string;
  description: string;
}

@Component({
  selector: 'app-ticket-info-dialog',
  templateUrl: './ticket-info-dialog.component.html',
  styleUrls: ['./ticket-info-dialog.component.less']
})
export class TicketInfoDialogComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: InfoDialogData, public matDialogRef: MatDialogRef<TicketInfoDialogComponent>) { }

  ngOnInit(): void {
  }

}
