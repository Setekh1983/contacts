import { Injectable } from '@angular/core';
import { Contact } from './contact';
import { CONTACTS } from './mock-contacts';
import { Observable, of } from 'rxjs';
import { MessageService } from './message.service';

@Injectable({
  providedIn: 'root'
})
export class ContactService {

  constructor(private messageService: MessageService) { }

  getContact(id: number) : Observable<Contact> {
    
      this.messageService.add(`ContactService: fetched contact id=${id}`)

      var result: Contact | undefined = CONTACTS.find(contact => contact.id === id);

      if (result === undefined || result === null) {
        result = {
          id: 0,
          foreName: "",
          lastName: "",
          fullName: ""
        };
      }

      return of(result)
  }

  getContacts(): Observable<Contact[]> {
    // TODO: send the message _after_ fetching the heroes
    this.messageService.add("ContactService: fetched contacts");
    return of(CONTACTS);
  }
}
