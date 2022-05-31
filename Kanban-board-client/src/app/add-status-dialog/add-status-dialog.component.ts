import { Component, OnInit } from '@angular/core';
import {DataService} from "../services/data.service";
import {status} from "../models/status.model";
import {NgForm} from "@angular/forms";
import {MatDialogRef} from "@angular/material/dialog";
import {v4 as uuidv4} from "uuid";

@Component({
  selector: 'app-add-status-dialog',
  templateUrl: './add-status-dialog.component.html',
  styleUrls: ['./add-status-dialog.component.less']
})
export class AddStatusDialogComponent implements OnInit {

  name: string = '';
  description: string = '';
  priority: string = '';

  constructor(private dataService: DataService, public matDialogRef: MatDialogRef<AddStatusDialogComponent>) {
  }

  ngOnInit(): void {
  }

  submit(form: NgForm) {
    this.dataService.addStatus({
      id: uuidv4(),
      name: this.name,
      description: this.description,
      priority: this.priority
    }).subscribe((res) => {
      console.log(res);
      location.reload();
    })
  }

}
