import { Component, OnInit, Input } from '@angular/core';
import { Contact } from '../contact';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ContactService } from '../contact.service';

@Component({
  selector: 'app-contact-details',
  templateUrl: './contact-details.component.html',
  styleUrls: ['./contact-details.component.less']
})
export class ContactDetailsComponent implements OnInit {

  contact: Contact | null;

  constructor(private contactService: ContactService,
    private route: ActivatedRoute,
    private location: Location) { 
    this.contact = null;
  }

  ngOnInit(): void {
    this.getContact();
  }

  goBack(): void {
    this.location.back();
  }

  getContact() {
    const idString = this.route.snapshot.paramMap.get('id');

    if (idString != null) {
      const id = +idString;
 
      this.contactService.getContact(id)
        .subscribe(contact => this.contact = contact);
    }
  }
}
