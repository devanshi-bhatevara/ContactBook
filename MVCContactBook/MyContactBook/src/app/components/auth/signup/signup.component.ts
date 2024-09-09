import { Component, ElementRef, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent {
  user: User = {
    userId: 0,
    firstName: "",
    lastName: "",
    loginId: "",
    email: "",
    contactNumber: "",
    password: "",
    confirmPassword: "",
    fileName: null,
    imageByte: ''
  };
  imageUrl: string | ArrayBuffer | null = null;
  loading:boolean=false;
 @ViewChild('imageInput') imageInput!: ElementRef;
 fileSizeExceeded = false;
fileFormatInvalid = false;
  constructor(private authService:AuthService,private router:Router) {}
  onSubmit(signUpForm:NgForm):void{
    if(signUpForm.valid){
      this.loading=true;
      console.log(signUpForm.value);
      if (this.imageUrl === null) {
        // If file has been removed, clear the imageByte and fileName in the contact object
        this.user.imageByte = '';
        this.user.fileName = null;
      }
      this.authService.signUp(this.user).subscribe({
        next:(response)=>{
          if(response.success){
            this.router.navigate(['/signupsuccess']);
          }
          else{
            alert(response.message);
          }
          this.loading=false;
        },
        error:(err)=>{
          console.log(err.error.message);
          console.log(err);
          this.loading=false;
          alert(err.error.message);
        },
   
      });
     
    }
  }
  checkPasswords(form: NgForm):void {
    const password = form.controls['password'];
    const confirmPassword = form.controls['confirmPassword'];
 
    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
    } else {
      confirmPassword.setErrors(null);
    }
  }

  onFileChange(event: any): void {
    const file = event.target.files[0];
  

    if (file) {

      const fileType = file.type; // Get the MIME type of the file
      if (fileType === 'image/jpeg' || fileType === 'image/png' || fileType === 'image/jpg')
        {
          if(file.size > 50 * 1024)
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
        this.user.imageByte = (reader.result as string).split(',')[1];
        this.user.fileName = file.name;
        this.imageUrl = reader.result;
      };
      reader.readAsDataURL(file);
    }
    else{
              // Alert user about invalid file format
              this.fileFormatInvalid = true;
              const inputElement = document.getElementById('imageByte') as HTMLInputElement;
              inputElement.value = '';
      
               
    }
    }
   
  }


 removeFile() {
    this.imageUrl = null; // Clear the imageUrl variable to remove the image
    // You may want to also clear any associated form data here if needed
    this.user.fileName = null;
    this.imageInput.nativeElement.value = '';
}
  }
