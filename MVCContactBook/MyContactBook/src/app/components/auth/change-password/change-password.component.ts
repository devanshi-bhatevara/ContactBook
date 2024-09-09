import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ChangePassword } from 'src/app/models/change-password.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {


  loading: boolean = false;
  forgotPassForm!: FormGroup;
  username: string | null | undefined;

  constructor(
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.forgotPassForm = this.fb.group({
      username: [''],
      password: ['', [Validators.required, Validators.minLength(8), this.passwordStrengthValidator]],
      confirmPassword: ['',[Validators.required]]
  }, { validator: this.passwordMatchValidator });

  this.authService.getUsername().subscribe(username => {
    this.username = username;
  });
  
  }

  

  get formControl() {
    return this.forgotPassForm.controls;
  }

  


  passwordMatchValidator(form: FormGroup): { [key: string]: any } | null {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
  
    const passwordsMatch = password === confirmPassword;
    console.log('Passwords Match:', passwordsMatch);
  
    return passwordsMatch ? null : { passwordMismatch: true };
  }
  
passwordStrengthValidator(control: any) {
  const password = control.value;
  if (!password) return null;

  const hasSpecialCharacter = /[ `!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/.test(password);
  const hasUpperCase = /[A-Z]/.test(password);
  const hasLowerCase = /[a-z]/.test(password);
  const hasDigit = /[0-9]/.test(password);

  const isValid = hasSpecialCharacter && hasUpperCase && hasLowerCase && hasDigit;
  return isValid ? null : { passwordStrength: true };
}


  OnSubmit() {
    this.loading = true;

    if (this.forgotPassForm.valid) {

      this.forgotPassForm.patchValue({
        username: this.username
      });
      console.log(this.username)
      console.log(this.forgotPassForm.value);
      this.authService.forgotPassword(this.forgotPassForm.value).subscribe({
        next: (response) => {
          if (response.success) {
            alert("Password changed successfully")
            this.router.navigate(['/home']);
          }
        },
        error: (err) => {
          alert(err.error.message);
          console.log(err);
          this.loading = false;

        },
        complete: () => {
          console.log("Completed");
          this.loading = false;

        }
      })
    }
  }
}

