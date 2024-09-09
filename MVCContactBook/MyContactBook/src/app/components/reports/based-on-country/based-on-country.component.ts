import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Country } from 'src/app/models/country.model';
import { ContactService } from 'src/app/services/contacts.service';
import { CountryService } from 'src/app/services/country.service';

@Component({
  selector: 'app-based-on-country',
  templateUrl: './based-on-country.component.html',
  styleUrls: ['./based-on-country.component.css']
})
export class BasedOnCountryComponent {
  totalContacts!: number;
  loading: boolean = false;
  country : Country[] = [];
  contactForm!: FormGroup;

  constructor(
    private contactService : ContactService,
    private countryService : CountryService,
    private fb: FormBuilder

  ) {}

  ngOnInit(): void {
    this.contactForm = this.fb.group({
      countryId : [0, [Validators.required, this.contactValidator]],

    })
    this.loadCountries();
    this.totalContactsCount();
  } 

  get formControl(){
    return this.contactForm.controls;
   }

   contactValidator(control: any){
    return control.value ==''? {invalidContact:true}:null;
   }

  loadCountries():void{
    this.loading = true;
    this.countryService.getAllCountries().subscribe({
      next:(response: ApiResponse<Country[]>) =>{
        if(response.success){
          this.country = response.data;
        }
        else{
          console.error('Failed to fetch countries', response.message);
        }
        this.loading = false;
      },
      error:(error => {
        console.error('Failed to fetch countries', error);
        this.loading = false;
      })
    }
  )
  }

  totalContactsCount() {
    this.contactForm.get('countryId')?.valueChanges.subscribe((countryId: number) => {
    this.contactService.getContactsCountBasedOnCountry(countryId)
    .subscribe({
      next: (response: ApiResponse<number>) => {
        if(response.success)
          {
            this.totalContacts = response.data;
            console.log(this.totalContacts);

          }
          else{
            console.error('Failed to fetch contacts', response.message);
          }
      },
      error:(error => {
        this.totalContacts = 0;
        console.error('Failed to fetch contacts', error);
        this.loading = false;
      })
    });
  });
}

}
