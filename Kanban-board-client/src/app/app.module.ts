import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TaskListsComponent } from './task-lists/task-lists.component';
import { DragDropModule } from "@angular/cdk/drag-drop";
import { MatButtonModule } from "@angular/material/button";
import { MatInputModule } from "@angular/material/input";
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { MatDialogModule } from "@angular/material/dialog";
import { MatCardModule } from "@angular/material/card";
import { MatSliderModule } from '@angular/material/slider';
import {HttpClientModule} from "@angular/common/http";
import { AddStatusDialogComponent } from './add-status-dialog/add-status-dialog.component';
import { AddTicketDialogComponent } from './add-ticket-dialog/add-ticket-dialog.component';
import {FormsModule} from "@angular/forms";
import {MatSelectModule} from "@angular/material/select";
import {MatOptionModule} from "@angular/material/core";
import { DeleteTicketDialogComponent } from './delete-ticket-dialog/delete-ticket-dialog.component';
import { TicketInfoDialogComponent } from './ticket-info-dialog/ticket-info-dialog.component';
import { DeleteStatusDialogComponent } from './delete-status-dialog/delete-status-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    TaskListsComponent,
    AddStatusDialogComponent,
    AddTicketDialogComponent,
    DeleteTicketDialogComponent,
    TicketInfoDialogComponent,
    DeleteStatusDialogComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    DragDropModule,
    MatCardModule,
    MatButtonModule,
    MatDialogModule,
    MatInputModule,
    MatSnackBarModule,
    MatSliderModule,
    HttpClientModule,
    FormsModule,
    MatSelectModule,
    MatOptionModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
