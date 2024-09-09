import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {


  loading: boolean = false;
  forgotPassForm!: FormGroup;

  constructor(
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.forgotPassForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(8), this.passwordStrengthValidator]],
      confirmPassword: ['',[Validators.required]]
  }, { validator: this.passwordMatchValidator });
  
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
  
//   passwordsMatch(): boolean {
//     const password = this.forgotPassForm.get('password')?.value;
//     const confirmPassword = this.forgotPassForm.get('confirmPassword')?.value;
//     return password === confirmPassword;
// }
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

      console.log(this.forgotPassForm.value);
      this.authService.forgotPassword(this.forgotPassForm.value).subscribe({
        next: (response) => {
          if (response.success) {
            alert("Password changed successfully")
            this.router.navigate(['/signin']);
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

