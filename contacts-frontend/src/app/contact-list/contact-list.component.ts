import { Component, OnInit } from '@angular/core';
import { Contact } from '../contact';
import { CONTACTS } from '../mock-contacts';

@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.less']
})
export class ContactListComponent implements OnInit {

  contacts = CONTACTS;
  selectedContact: Contact | null;

  constructor() { 
    this.selectedContact = null;
  }

  ngOnInit(): void {
  }

  onSelect(contact: Contact): void {
    this.selectedContact = contact;
  }
}
