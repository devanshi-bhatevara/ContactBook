import { Component } from '@angular/core';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { ContactService } from 'src/app/services/contacts.service';

@Component({
  selector: 'app-based-on-gender',
  templateUrl: './based-on-gender.component.html',
  styleUrls: ['./based-on-gender.component.css']
})
export class BasedOnGenderComponent {
  totalMale!: number;
  totalFemale!: number;
  loading: boolean = false;

  
  constructor(
    private contactService : ContactService,
  ) {}

  ngOnInit(): void {
    this.totalMaleCount();
    this.totalFemaleCount();

  }


  totalMaleCount() {
    this.contactService.getContactsCountBasedOnGender('M')
    .subscribe({
      next: (response: ApiResponse<number>) => {
        if(response.success)
          {
            this.totalMale = response.data;
            console.log(this.totalMale);

          }
          else{
            console.error('Failed to fetch contacts', response.message);
          }
      },
      error:(error => {
        this.totalMale = 0;
        console.error('Failed to fetch contacts', error);
        this.loading = false;
      })
    });

}
totalFemaleCount() {
    this.contactService.getContactsCountBasedOnGender('F')
    .subscribe({
      next: (response: ApiResponse<number>) => {
        if(response.success)
          {
            this.totalFemale = response.data;
            console.log(this.totalFemale);

          }
          else{
            console.error('Failed to fetch contacts', response.message);
          }
      },
      error:(error => {
        this.totalFemale = 0;
        console.error('Failed to fetch contacts', error);
        this.loading = false;
      })
    });

}

}
