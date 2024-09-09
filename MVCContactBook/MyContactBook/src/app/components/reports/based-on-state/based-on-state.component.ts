import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { ContactSP } from 'src/app/models/contactSP.model';
import { Country } from 'src/app/models/country.model';
import { State } from 'src/app/models/state.model';
import { ContactService } from 'src/app/services/contacts.service';
import { CountryService } from 'src/app/services/country.service';
import { StateService } from 'src/app/services/state.service';

@Component({
  selector: 'app-based-on-state',
  templateUrl: './based-on-state.component.html',
  styleUrls: ['./based-on-state.component.css']
})
export class BasedOnStateComponent {
  country: Country[] = [];
  state: State[] = [];
  loading:boolean = false;
  contactForm!: FormGroup;
  contacts: ContactSP[] | undefined | null;



  constructor(
    private contactService: ContactService,
    private countryService: CountryService, 
    private stateService: StateService,
    private fb: FormBuilder
  ) {}

  ngOnInit():void{
    this.contactForm = this.fb.group({
      countryId : [0, [Validators.required, this.contactValidator]],
      stateId : [0, [Validators.required, this.contactValidator]],
    })
    this.loadCountries();
    this.fetchStateByCountry();
    this.loadContacts();
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

  fetchStateByCountry(): void {
    
    this.contactForm.get('countryId')?.valueChanges.subscribe((countryId: number) => {
      if (countryId !== 0) {
      this.state = [];
      this.contactForm.get('stateId')?.setValue(0); // Reset the state control's value to null
        this.loading = true;
        this.stateService.getStatesByCountry(countryId).subscribe({
          next: (response: ApiResponse<State[]>) => {
            if (response.success) {
              this.state = response.data;
            } else {
              console.error('Failed to fetch states', response.message);
            }
            this.loading = false;
          },
          error: (error) => {
            console.error('Failed to fetch states', error);
            this.loading = false;
          }
        });
      }
    });
  }

  loadContacts() {
    if(this.contactForm.get('stateId')?.valueChanges)
      {
    this.contactForm.get('stateId')?.valueChanges.subscribe((stateId: number) => {
    this.contactService.getContactByState(stateId)
      .subscribe({
        next:(response: ApiResponse<ContactSP[]>) => {
          if(response.success){
            this.contacts = response.data;
            console.log(response.data);
          }
          else {
            console.error('No data found for the selected state .');
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
  });
  }
  }


  
}
