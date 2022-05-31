import {Component, Inject, OnInit} from '@angular/core';
import {DataService} from "../services/data.service";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {ticket} from "../models/ticket.model";

export interface DeleteStatusData {
  id: string;
  name: string;
}

@Component({
  selector: 'app-delete-status-dialog',
  templateUrl: './delete-status-dialog.component.html',
  styleUrls: ['./delete-status-dialog.component.less']
})
export class DeleteStatusDialogComponent implements OnInit {

  constructor(private dataService: DataService, public matDialogRef: MatDialogRef<DeleteStatusDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: DeleteStatusData) { }

  ticketsWithSelectedStatusId: ticket[] = [];

  ngOnInit(): void {
    this.dataService.getTickets()
      .subscribe((res: ticket[]) => {
        for (let ticket of res)
          if (ticket.statusId === this.data.id)
            this.ticketsWithSelectedStatusId.push(ticket);
      })
  }

  confirmButton() {
    this.dataService.deleteStatus(this.data.id)
      .subscribe((res) => {
        console.log(res);
        location.reload();
      })

    for (let ticket of this.ticketsWithSelectedStatusId) {
      this.dataService.deleteTicket(ticket.id)
        .subscribe((res) => {
          console.log(res);
        })
    }
  }

}
