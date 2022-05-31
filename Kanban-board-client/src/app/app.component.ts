import { Component } from '@angular/core';
import {MatDialog} from "@angular/material/dialog";
import {AddStatusDialogComponent} from "./add-status-dialog/add-status-dialog.component";
import {AddTicketDialogComponent} from "./add-ticket-dialog/add-ticket-dialog.component";
import {ticket} from "./models/ticket.model";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {
  title = 'Kanban-board-client';

  constructor(private dialog: MatDialog) {
  }

  addStatusButton() {
    this.dialog.open(AddStatusDialogComponent);
  }

  addTicketButton() {
    this.dialog.open(AddTicketDialogComponent, {
      data: {
        isEditing: false
      }
    });
  }

}
