import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Country } from 'src/app/models/country.model';
import { State } from 'src/app/models/state.model';
import { ContactService } from 'src/app/services/contacts.service';
import { CountryService } from 'src/app/services/country.service';
import { StateService } from 'src/app/services/state.service';

@Component({
  selector: 'app-edit-contact',
  templateUrl: './edit-contact.component.html',
  styleUrls: ['./edit-contact.component.css']
})
export class EditContactComponent {
  loading: boolean = false;
  country: Country[] = [];
  state: State[] = [];
  contactForm!: FormGroup;
  imageUrl: string | ArrayBuffer | null = null;
@ViewChild('imageInput') imageInput!: ElementRef;
  fileSizeExceeded = false;
  fileFormatInvalid = false;

  constructor(
    private contactService: ContactService,
    private countryService: CountryService, 
    private stateService: StateService, 
    private router: Router,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private datePipe: DatePipe 
  ) {}

  ngOnInit(): void {
    this.contactForm = this.fb.group({
      contactId: [0],
      firstName: ['',[Validators.required, Validators.minLength(2)]],
      lastName: ['',[Validators.required, Validators.minLength(2)]],
      address: ['', Validators.required],
      phone: ['',[Validators.required, Validators.minLength(10), Validators.maxLength(10)]],
      countryId : [0, [Validators.required, this.contactValidator]],
      stateId : [0, [Validators.required, this.contactValidator]],
      email: ['',[Validators.required, Validators.email]],
      gender: [, Validators.required],
      isFavourite: [false],
      imageByte:[''],
      fileName: [null],
      birthDate: [,[this.validateBirthdate]]

    })

    this.getContact();
    this.loadCountries();
    this.fetchStateByCountry();

     }

     
     get formControl(){
      return this.contactForm.controls;
     }
  
     contactValidator(control: any){
      return control.value ==''? {invalidContact:true}:null;
     }

     validateBirthdate(control: AbstractControl): ValidationErrors | null {
      const selectedDate = new Date(control.value);
      const currentDate = new Date();
    
      // Set hours, minutes, seconds, and milliseconds to 0 to compare only the date part
      selectedDate.setHours(0, 0, 0, 0);
      currentDate.setHours(0, 0, 0, 0);
    
      if (selectedDate > currentDate) {
        return { invalidBirthDate: true };
      }
      return null;
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
          this.contactForm.get('stateId')?.setValue(null); // Reset the state control's value to null  
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

    // Add removeImage method to handle the removal of the image
removeImage(): void {
  this.contactForm.patchValue({
      imageByte: '', // Clear the imageByte field
      fileName: null,
        });
  this.imageUrl = null;
  const inputElement = document.getElementById('imageByte') as HTMLInputElement;
  inputElement.value = '';
}

    
onFileChange(event: any): void {
  const file = event.target.files[0];
  if (file) {
    const fileType = file.type; // Get the MIME type of the file
    if (fileType === 'image/jpeg' || fileType === 'image/png' || fileType === 'image/jpg')
       {
        if(file.size > 50 *1024)
          {
            this.fileSizeExceeded = true;
            const inputElement = document.getElementById('imageByte') as HTMLInputElement;
          inputElement.value = ''; 
          return; 
         }
         this.fileSizeExceeded= false;
         this.fileFormatInvalid = false;

    const reader = new FileReader();
    reader.onload = () => {
      this.contactForm.patchValue({
        imageByte: (reader.result as string).split(',')[1],
        fileName: file.name
      });
      this.imageUrl = reader.result;

    };
    reader.readAsDataURL(file);
  }
  else {
    // Alert user about invalid file format
    this.fileFormatInvalid = true;
        const inputElement = document.getElementById('imageByte') as HTMLInputElement;
    inputElement.value = '';
     
  }       
      }
    }

    getContact():void{
      const contactId = Number(this.route.snapshot.paramMap.get('id'));
      this.contactService.getContactById(contactId).subscribe({
        next: (response) => {

          if (response.success) {
            const formattedBirthDate = this.datePipe.transform(response.data.birthDate, 'yyyy-MM-dd');
            this.contactForm.patchValue({
              contactId: response.data.contactId,
              countryId: response.data.countryId,
              stateId: response.data.stateId,
              firstName : response.data.firstName,
              lastName : response.data.lastName,
              address : response.data.address,
              phone : response.data.phone,
              email : response.data.email,
              gender : response.data.gender,
              isFavourite : response.data.isFavourite,
              fileName: response.data.fileName,
              imageByte: response.data.imageByte,
              birthDate: formattedBirthDate      
            });
            console.log(formattedBirthDate)
             // Check if the response contains imageByte data
             if (response.data.imageByte) {
              // Set imageUrl to display the image
              this.imageUrl = 'data:image/jpeg;base64,' + response.data.imageByte;
          }
          } else {
            console.error('Failed to fetch contacts', response.message);
          }
        },
        error: (error) => {
          alert(error.error.message);
          this.loading = false;
        },
        complete: () => {
          this.loading = false;
        },
      });
      }
    

    OnSubmit(){
      this.loading = true;
  
      if(this.contactForm.valid){
        if (!this.contactForm.get('birthDate')?.value) {
          this.contactForm.get('birthDate')?.setValue(null);
        }
        console.log(this.contactForm.value);
        this.contactService.modifyContact(this.contactForm.value).subscribe({
          next: (response) => {
            if (response.success) {
              this.router.navigate(['/paginatedContacts']);
            }
            else{
              alert(response.message)
            }
          },
          error:(err)=>{
            alert(err.error.message);
            console.log(err);
            this.loading = false;
  
          },
          complete:()=>{
            console.log("Completed");
            this.loading = false;
  
          }
        })
      }
    }

}
