import { Component, OnInit, Input } from '@angular/core';
import { Contact } from '../contact';

@Component({
  selector: 'app-contact-details',
  templateUrl: './contact-details.component.html',
  styleUrls: ['./contact-details.component.less']
})
export class ContactDetailsComponent implements OnInit {

  @Input() contact: Contact | null;

  constructor() { 
    this.contact = null;
  }

  ngOnInit(): void {
  }
}
