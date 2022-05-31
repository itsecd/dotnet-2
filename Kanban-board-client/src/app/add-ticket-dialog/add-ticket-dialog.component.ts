import {Component, Inject, OnInit} from '@angular/core';
import {status} from "../models/status.model";
import {DataService} from "../services/data.service";
import {NgForm} from "@angular/forms";
import {ticket} from "../models/ticket.model";
import { v4 as uuidv4 } from 'uuid';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";

export interface AddDialogData {
  isEditing: boolean;
  id: string;
  title: string;
  description: string;
  statusId: string;
}

@Component({
  selector: 'app-add-ticket-dialog',
  templateUrl: './add-ticket-dialog.component.html',
  styleUrls: ['./add-ticket-dialog.component.less']
})
export class AddTicketDialogComponent implements OnInit {
  id: string = '';
  title: string = '';
  description: string = '';
  statusId: string = '';
  tickets: ticket[] = [];
  selectStatusOptions: { value: string, viewValue: string }[] = [];

  constructor(private dataService: DataService, public matDialogRef: MatDialogRef<AddTicketDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: AddDialogData) {
  }

  ngOnInit(): void {
    this.dataService.getStatuses()
      .subscribe((statuses: status[]) => {
        for (let status of statuses) {
          this.selectStatusOptions.push({
            value: status.id,
            viewValue: status.name
          })
        }
      })

    if (this.data.isEditing) {
      this.title = this.data.title;
      this.description = this.data.description;
      this.statusId = this.data.statusId;
    }
  }

  submit(form: NgForm) {
    if (this.data.isEditing) {
      this.dataService.updateTicketStatus({
        id: this.data.id,
        title: this.title,
        description: this.description,
        statusId: this.statusId
      }).subscribe((res) => {
        console.log(res);
        location.reload();
      })
    } else {
      this.dataService.addTicket({
        id: uuidv4(),
        title: this.title,
        description: this.description,
        statusId: this.statusId
      }).subscribe((res) => {
        console.log(res);
        location.reload();
      })
    }

    form.reset();
    this.matDialogRef.close('');
  }
}
