import { ChangeDetectorRef, Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EditContact } from 'src/app/models/edit-contact.model';
import { AuthService } from 'src/app/services/auth.service';
import { ContactService } from 'src/app/services/contacts.service';

@Component({
  selector: 'app-contact-detail',
  templateUrl: './contact-detail.component.html',
  styleUrls: ['./contact-detail.component.css']
})
export class ContactDetailComponent {
  contactId:number|undefined;
  isAuthenticated: boolean = false;
  username:string |null|undefined;

  contact:EditContact={
    contactId: 0,
    phone: '',
    countryId: 0,
    country: {
      countryId: 0,
      countryName: ''
    },
    stateId: 0,
    state: {
      countryId: 0,
      stateId: 0,
      stateName: ''
    },
    firstName: '',
    lastName: '',
    email: '',
    address: '',
    gender: '',
    isFavourite: false,
    fileName: null,
    imageByte: '',
    birthDate: new Date()
  };

  constructor(private contactService:ContactService,private route:ActivatedRoute, private router:Router,private authService: AuthService, private cdr: ChangeDetectorRef){}
  ngOnInit(): void {
     this.authService.isAuthenticated().subscribe((authState:boolean)=>{
      this.isAuthenticated=authState;
      this.cdr.detectChanges();
     });
     this.authService.getUsername().subscribe((username:string |null|undefined)=>{
      this.username=username;
      this.cdr.detectChanges();
     });
    const contactId = Number(this.route.snapshot.paramMap.get('id'));
    this.contactService.getContactById(contactId).subscribe({
      next: (response) => {
        if (response.success) {
          this.contact = response.data;
        } else {
          console.error('Failed to fetch contact', response.message);
        }
      },
      error: (error) => {
        console.error('Failed to fetch contact', error);
      },
    });
  }
  
  deleteContact(contactId: number) {
    if (confirm('Are you sure you want to delete this contact?')) {
      this.contactService.deleteContact(contactId).subscribe(() => {
        this.router.navigate(['/paginatedContacts']);

      });
    }
  }

}
