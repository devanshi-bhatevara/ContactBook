import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ContactService } from '../../../services/contacts.service';
import { Contact } from 'src/app/models/contact.model';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.css']
})
export class ContactListComponent implements OnInit{
  contacts: Contact[] | undefined;
  username:string |null|undefined;
  loading: boolean = false;
  isAuthenticated: boolean = false;
  constructor(private contactService: ContactService,private authService: AuthService, private cdr: ChangeDetectorRef) {
   
    
  }
  ngOnInit(): void {
     this.loadContacts();
     this.authService.isAuthenticated().subscribe((authState:boolean)=>{
      this.isAuthenticated=authState;
      this.cdr.detectChanges();
     });
     this.authService.getUsername().subscribe((username:string |null|undefined)=>{
      this.username=username;
      this.cdr.detectChanges();
     });
  }

  loadContacts():void{
      this.loading = true;
      this.contactService.getAllContacts().subscribe({
        next:(response: ApiResponse<Contact[]>) =>{
          if(response.success){
            this.contacts = response.data;
            console.log(response.data);
          }
          else{
            console.error('Failed to fetch contacts', response.message);
          }
          this.loading = false;
        },
        error:(error => {
          console.error('Failed to fetch contacts', error);
          this.loading = false;
        })
      });
    }

    deleteContact(contactId: number) {
      if (confirm('Are you sure you want to delete this contact?')) {
        this.contactService.deleteContact(contactId).subscribe(() => {
          this.loadContacts();
        });
      }
    }
  
  }
  