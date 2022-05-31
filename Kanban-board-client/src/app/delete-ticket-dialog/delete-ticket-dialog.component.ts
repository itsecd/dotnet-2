import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {DataService} from "../services/data.service";

export interface DeleteDialogData {
  id: string;
  title: string;
}

@Component({
  selector: 'app-delete-ticket-dialog',
  templateUrl: './delete-ticket-dialog.component.html',
  styleUrls: ['./delete-ticket-dialog.component.less']
})
export class DeleteTicketDialogComponent implements OnInit {

  constructor(private dataService: DataService, public matDialogRef: MatDialogRef<DeleteTicketDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: DeleteDialogData) { }

  ngOnInit(): void {
  }

  confirmButton() {
    this.dataService.deleteTicket(this.data.id)
      .subscribe((res) => {
        console.log(res);
        location.reload();
      })
  }

}
