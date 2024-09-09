import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Contact } from 'src/app/models/contact.model';
import { EditContact } from 'src/app/models/edit-contact.model';
import { AuthService } from 'src/app/services/auth.service';
import { ContactService } from 'src/app/services/contacts.service';

@Component({
  selector: 'app-contact-list-paginated',
  templateUrl: './contact-list-paginated.component.html',
  styleUrls: ['./contact-list-paginated.component.css']
})
export class ContactListPaginatedComponent implements OnInit {
  contacts: Contact[] | undefined;
  contactsForInitial: Contact[] = [];

  username:string |null|undefined;
  totalContacts!: number;
  pageSize = 2;
  currentPage = 1;
  loading: boolean = false;
  isAuthenticated: boolean = false;

  uniqueFirstLetters: string[] = [];
totalPages: number[] = [];
selectedLetter?: string = '';
sortOrder: string = 'asc';
colors: string[] = ['red', 'blue', 'green', 'orange', 'purple', 'teal', 'pink', 'brown','red', 'blue', 'green', 'orange', 'purple', 'teal', 'pink', 'brown','red', 'blue', 'green', 'orange', 'purple', 'teal', 'pink', 'brown','pink','blue'];
searchQuery: string = '';
  constructor(private contactService: ContactService,private authService: AuthService, private cdr: ChangeDetectorRef,private route : Router) { }

  ngOnInit() {
    this.loadContacts();
    this.loadAllContacts();
    this.totalContactsCount();
    this.authService.isAuthenticated().subscribe((authState:boolean)=>{
      this.isAuthenticated=authState;
      this.cdr.detectChanges();
     });
     this.authService.getUsername().subscribe((username:string |null|undefined)=>{
      this.username=username;
      this.cdr.detectChanges();
     });
  }

  totalContactsCount(letter?: string, search?: string) {
    this.contactService.fetchContactCount(letter, search)
    .subscribe({
      next: (response: ApiResponse<number>) => {
        if(response.success)
          {
            this.totalContacts = response.data;
            console.log(this.totalContacts);
            this.calculateTotalPages();

          }
          else{
            console.error('Failed to fetch contacts', response.message);
          }
      },
      error:(error => {
        console.error('Failed to fetch contacts', error);
        this.loading = false;
      })
    });
  }

  loadContacts(letter?: string, search?: string) {
    this.loading = true;
    this.contactService.getAllPaginatedContacts(this.currentPage, this.pageSize, this.sortOrder,letter,search)
      .subscribe({
        next:(response: ApiResponse<Contact[]>) => {
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

  loadAllContacts(): void {
    this.loading = true;
    this.contactService.getAllContacts().subscribe({
      next: (response: ApiResponse<Contact[]>) => {
        if (response.success) {
          console.log(response.data);
          this.contactsForInitial = response.data;
          this.updateUniqueFirstLetters();
        } else {
          console.error('Failed to fetch contacts', response.message);
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching contacts.', error);
        this.loading = false;
      }
    });
  }
  getUniqueFirstLetters(): string[] {
    // Extract first letters from contact names and filter unique letters
    const firstLetters = Array.from(new Set(this.contactsForInitial.map(contact => contact.firstName.charAt(0).toUpperCase())));
    return firstLetters.sort(); // Sort alphabetically
}
  updateUniqueFirstLetters(): void {
    this.uniqueFirstLetters = this.getUniqueFirstLetters();
}

  calculateTotalPages() {
    this.totalPages = [];
    const pages = Math.ceil(this.totalContacts / this.pageSize);
    for (let i = 1; i <= pages; i++) {
      this.totalPages.push(i);
    }
  }
  onPageChange(page: number, letter?: string) {
    this.currentPage = page;
    this.loadContacts(letter, this.searchQuery);
  }


  onLetterClick(letter: string) {
    if (this.selectedLetter === letter) {
      this.selectedLetter = ''; // Deselect the letter
      this.onShowAll(); // Call the "Show All" function
    } else {
      this.selectedLetter = letter; // Select the clicked letter
      this.currentPage = 1; // Reset to the first page when a letter is selected
      this.totalContactsCount(this.selectedLetter, this.searchQuery); // Update contacts based on the selected letter
      this.loadContacts(this.selectedLetter, this.searchQuery);
    }
  }
  

  onPageSizeChange(letter?: string) {
    this.currentPage = 1; // Reset to first page when page size changes
    this.loadContacts(letter, this.searchQuery);
    this.totalContactsCount(letter, this.searchQuery);
  }

  onShowAll() {
    this.selectedLetter = '';
    this.currentPage = 1;
    this.totalContactsCount('',this.searchQuery);
    this.loadContacts('',this.searchQuery);
  }

  deleteContact(contactId: number) {
    if (confirm('Are you sure you want to delete this contact?')) {
      this.contactService.deleteContact(contactId).subscribe(() => {
        this.loadContacts(this.selectedLetter); 
        this.totalContactsCount(this.selectedLetter); 
        this.loadAllContacts();  
        this.calculateTotalPages();

      // Check if the current page should be adjusted
      if (this.currentPage > 1 && this.contacts && this.contacts.length === 1) {

        this.onPageChange(this.currentPage - 1, this.selectedLetter);
      }
      });
    }
  }

  sortAsc(letter?: string)
  {
    this.sortOrder = 'asc'
    this.currentPage = 1;
    this.totalContactsCount(letter, this.searchQuery);
    this.loadContacts(letter, this.searchQuery);
  }

  sortDesc(letter?: string)
  {
    this.sortOrder = 'desc'
    this.currentPage = 1;
    this.totalContactsCount(letter, this.searchQuery);
    this.loadContacts(letter, this.searchQuery);
  }

  searchContacts() {
    this.currentPage = 1;
    this.loadContacts(this.selectedLetter, this.searchQuery);
    this.totalContactsCount(this.selectedLetter, this.searchQuery);
  }

  clearSearch() {
    this.currentPage = 1;
    this.searchQuery = '';
    this.loadContacts(this.selectedLetter);
    this.totalContactsCount(this.selectedLetter);
  }

  toggleFavourite(contact: EditContact): void {
    contact.isFavourite = !contact.isFavourite; // Toggle isFavourite property
    this.contactService.modifyContact(contact).subscribe({
        next: (response) => { 
          if(response.success){
            console.log(response.message)
          }
          else{
            alert(response.message)
          }
        },
        error:(error) => {
            console.error( error);
            contact.isFavourite = !contact.isFavourite;
        }
  });
}
}
