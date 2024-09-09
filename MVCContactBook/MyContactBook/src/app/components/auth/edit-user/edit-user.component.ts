import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.css']
})
export class EditUserComponent {
  loading: boolean = false;
  contactForm!: FormGroup;
  imageUrl: string | ArrayBuffer | null = null;
  @ViewChild('imageInput') imageInput!: ElementRef;
  fileSizeExceeded =  false;
fileFormatInvalid =  false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder,
    private route: ActivatedRoute
    ) {}

  ngOnInit(): void {
    this.contactForm = this.fb.group({
      userId: [0],
      loginId: [''],
      firstName: ['',[Validators.required, Validators.minLength(2)]],
      lastName: ['',[Validators.required, Validators.minLength(2)]],
      contactNumber: ['',Validators.required],
      email: ['',[Validators.required, Validators.email]],
      imageByte:[''],
      fileName: [null],

    })

    this.getUser();

     }

     
     get formControl(){
      return this.contactForm.controls;
     }
  
     contactValidator(control: any){
      return control.value ==''? {invalidContact:true}:null;
     }

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
                this.fileSizeExceeded = true; // Set flag to true if file size exceeds the limit
              const inputElement = document.getElementById('imageByte') as HTMLInputElement;
              inputElement.value = ''; 
              return; 
             }
             this.fileFormatInvalid = false;
             this.fileSizeExceeded = false; 
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
    
    
    getUser():void{
      const contactId = String(this.route.snapshot.paramMap.get('loginId'));
      this.authService.getUserByLoginId(contactId).subscribe({
        next: (response) => {

          if (response.success) {
            this.contactForm.patchValue({
              userId: response.data.userId,
              loginId: response.data.loginId,
              firstName : response.data.firstName,
              lastName : response.data.lastName,
              contactNumber : response.data.contactNumber,
              email : response.data.email,
              fileName: response.data.fileName,
              imageByte: response.data.imageByte,
            });
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
        console.log(this.contactForm.value);
        this.authService.modifyUser(this.contactForm.value).subscribe({
          next: (response) => {
            if (response.success) {
            this.authService.emitProfileUpdated(); // Emit profile updated event
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
