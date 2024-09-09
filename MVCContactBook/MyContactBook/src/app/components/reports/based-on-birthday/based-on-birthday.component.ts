import { Component } from '@angular/core';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { ContactSP } from 'src/app/models/contactSP.model';
import { ContactService } from 'src/app/services/contacts.service';

@Component({
  selector: 'app-based-on-birthday',
  templateUrl: './based-on-birthday.component.html',
  styleUrls: ['./based-on-birthday.component.css']
})
export class BasedOnBirthdayComponent {
  months = [
    { name: 'January', value: 1 },
    { name: 'February', value: 2 },
    { name: 'March', value: 3 },
    { name: 'April', value: 4 },
    { name: 'May', value: 5 },
    { name: 'June', value: 6 },
    { name: 'July', value: 7 },
    { name: 'August', value: 8 },
    { name: 'September', value: 9 },
    { name: 'October', value: 10 },
    { name: 'November', value: 11 },
    { name: 'December', value: 12 }
  ];
  isExist :boolean = false;
  monthNames: string[] = this.months.map(month => month.name);
  contacts: ContactSP[] | undefined | null;
  selectedMonth: number=0 ;
  loading: boolean = false;

  constructor(private contactService: ContactService) {
    
  }
  loadContacts() {
    this.loading = true;
    this.contactService.getContactsBasedOnBirthdayMonth(this.selectedMonth)
      .subscribe({
        next:(response: ApiResponse<ContactSP[]>) => {
          if(response.success){
            this.contacts = response.data;
            console.log(response.data);
            this.isExist = true;
          }
          else {
            console.error('No data found for the selected month .');
            this.contacts=null;
          }
          this.loading = false;

        },
        error:(error => {
          console.error('Failed to fetch contacts', error);
          this.loading = false;
          this.contacts=null;
        })
      });
  }

  onMonthSelected(): void {
    this.contacts = null; // Clear previous data
    this.loadContacts();
  }
}
