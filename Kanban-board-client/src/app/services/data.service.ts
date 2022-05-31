import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import {Observable, Subscription} from "rxjs";
import {ticket} from "../models/ticket.model";
import {status} from "../models/status.model";

@Injectable({
  providedIn: 'root'
})
export class DataService {

  url = 'https://localhost:44312/';

  constructor(private http: HttpClient) { }

  getTickets(): Observable<any> {
    return this.http.get(this.url + 'api/Ticket');
  }

  getStatuses(): Observable<any> {
    return this.http.get(this.url + 'api/Status');
  }

  addTicket(body: ticket): Observable<any> {
    return this.http.post(this.url + 'api/Ticket', body);
  }

  deleteTicket(id: string) {
    return this.http.delete(this.url + 'api/Ticket/' + id);
  }

  deleteStatus(id: string) {
    return this.http.delete(this.url + 'api/Status/' + id);
  }

  addStatus(body: status): Observable<any> {
    return this.http.post(this.url + 'api/Status', body);
  }

  updateTicketStatus(body: ticket): Observable<any> {
    return this.http.put(this.url + 'api/Ticket', body);
  }
}
