import { Component, OnInit } from '@angular/core';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import {DataService} from "../services/data.service";
import {status} from "../models/status.model";
import {ticket} from "../models/ticket.model";
import {MatDialog} from "@angular/material/dialog";
import {AddStatusDialogComponent} from "../add-status-dialog/add-status-dialog.component";
import {TicketInfoDialogComponent} from "../ticket-info-dialog/ticket-info-dialog.component";
import {DeleteTicketDialogComponent} from "../delete-ticket-dialog/delete-ticket-dialog.component";
import {AddTicketDialogComponent} from "../add-ticket-dialog/add-ticket-dialog.component";
import {DeleteStatusDialogComponent} from "../delete-status-dialog/delete-status-dialog.component";

@Component({
  selector: 'task-lists',
  templateUrl: './task-lists.component.html',
  styleUrls: ['./task-lists.component.less']
})
export class TaskListsComponent implements OnInit {

  statuses: status[] = [];
  tickets: ticket[] = [];

  constructor(private dataService: DataService, private dialog: MatDialog) {
  }

  ngOnInit() {
    this.dataService.getStatuses()
      .subscribe((statuses: status[]) => {
        this.statuses = statuses;
      })

    this.dataService.getTickets()
      .subscribe((tickets) => {
        this.tickets = tickets;
      })
  }

  getTicketsByStatusId(statusId: string): ticket[] {
    return this.tickets.filter((ticket) => ticket.statusId === statusId);
  }

  drop(event: CdkDragDrop<any>, statusId: string) {
    let needToUpdate: boolean = !(event.previousContainer.data[event.previousIndex].statusId === statusId);
    event.previousContainer.data[event.previousIndex].statusId = statusId;

    if (needToUpdate)
      this.dataService.updateTicketStatus(event.previousContainer.data[event.previousIndex])
        .subscribe((res) => {
          console.log(res);
        })

    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex,
      );
    }
  }

  showTicketInfo(ticket: ticket) {
    this.dialog.open(TicketInfoDialogComponent, {
      data: {
        title: ticket.title,
        description: ticket.description
      }
    });
  }

  deleteTicket(ticket: ticket) {
    this.dialog.open(DeleteTicketDialogComponent, {
      data: {
        id: ticket.id,
        title: ticket.title
      }
    });
  }

  editTicketButton(ticket: ticket) {
    this.dialog.open(AddTicketDialogComponent, {
      data: {
        isEditing: true,
        id: ticket.id,
        title: ticket.title,
        description: ticket.description,
        statusId: ticket.statusId
      }
    });
  }

  deleteStatusButton(status: status) {
    this.dialog.open(DeleteStatusDialogComponent, {
      data: {
        id: status.id,
        name: status.name
      }
    });
  }

}
