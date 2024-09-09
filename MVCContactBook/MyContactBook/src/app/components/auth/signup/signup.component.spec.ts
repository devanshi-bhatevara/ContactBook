import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SignupComponent } from './signup.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule, NgForm, ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { of, throwError } from 'rxjs';

describe('SignupComponent', () => {
  let component: SignupComponent;
  let fixture: ComponentFixture<SignupComponent>;
  let authService: jasmine.SpyObj<AuthService>;
  let routerSpy: Router;



  beforeEach(() => {
    authService = jasmine.createSpyObj('AuthService',['signUp'])
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule,RouterTestingModule, FormsModule],
      declarations: [SignupComponent],
      providers: [{provide: AuthService, useValue: authService}]
    });
    fixture = TestBed.createComponent(SignupComponent);
    component = fixture.componentInstance;
    // fixture.detectChanges();
    routerSpy = TestBed.inject(Router);

  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should navigate to /signupsuccess on successful user addition', () => {
    spyOn(routerSpy,'navigate');
    const mockResponse: ApiResponse<string> = { success: true, data: '', message: '' };
    authService.signUp.and.returnValue(of(mockResponse));
 
    const form = <NgForm><unknown>{
      valid: true,
      value: {
       firstName: 'Test name',
        lastName: 'last name',
        loginId: 'test',
          email: "Test@gmail.com",
          contactNumber: "1234567891",
          password: "Password@1234",
          confirmPassword: "Password@1234",
          fileName: '',
          imageByte: "", 
        },
      controls: {
               firstName: {value:'Test name'},
        lastName: {value:'last name'},       
          loginId:{value: "test"},
          email: {value:"Test@gmail.com"},
          contactNumber: {value:"1234567891"},
          password: {value: "Password@1234"},
          confirmPassword: {value: "Password@1234"},
          fileName: {value:''},
          imageByte: {value:""},
          
      }
    };

     component.onSubmit(form);
 
    expect(authService.signUp).toHaveBeenCalledWith(component.user); // Verify addContact was called with component.contact
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/signupsuccess']);
    expect(component.loading).toBe(false);
  });

  it('should alert error message on unsuccessful user addition', () => {
    spyOn(window,'alert');
    const mockResponse: ApiResponse<string> = { success: false, data: '', message: '' };
    authService.signUp.and.returnValue(of(mockResponse));
 

    const form = <NgForm><unknown>{
      valid: true,
      value: {
       firstName: 'Test name',
        lastName: 'last name',
        loginId: 'test',
          email: "Test@gmail.com",
          contactNumber: "1234567891",
          password: "Password@1234",
          confirmPassword: "Password@1234",
          fileName: '',
          imageByte: "", 
        },
      controls: {
               firstName: {value:'Test name'},
        lastName: {value:'last name'},       
          loginId:{value: "test"},
          email: {value:"Test@gmail.com"},
          contactNumber: {value:"1234567891"},
          password: {value: "Password@1234"},
          confirmPassword: {value: "Password@1234"},
          fileName: {value:''},
          imageByte: {value:""},
          
      }
    };

     component.onSubmit(form);
 
    expect(window.alert).toHaveBeenCalledWith(mockResponse.message);
    expect(component.loading).toBe(false);
  });

  it('should alert error message on HTTP error', () => {
    spyOn(console, 'log');
    spyOn(window, 'alert');
    const mockError = { error: { message: 'HTTP error' } };
    authService.signUp.and.returnValue(throwError(mockError));
 
    const form = <NgForm><unknown>{
      valid: true,
      value: {
       firstName: 'Test name',
        lastName: 'last name',
        loginId: 'test',
          email: "Test@gmail.com",
          contactNumber: "1234567891",
          password: "Password@1234",
          confirmPassword: "Password@1234",
          fileName: '',
          imageByte: "", 
        },
      controls: {
               firstName: {value:'Test name'},
        lastName: {value:'last name'},       
          loginId:{value: "test"},
          email: {value:"Test@gmail.com"},
          contactNumber: {value:"1234567891"},
          password: {value: "Password@1234"},
          confirmPassword: {value: "Password@1234"},
          fileName: {value:''},
          imageByte: {value:""},
          
      }
    };

    component.onSubmit(form);
 
    expect(console.log).toHaveBeenCalledWith(mockError);
    expect(console.log).toHaveBeenCalledWith(mockError.error.message);
    expect(window.alert).toHaveBeenCalledWith(mockError.error.message)
    expect(component.loading).toBe(false);
  });
 
  it('should not call authService.signUp on invalid form submission', () => {
    const form = <NgForm>{ valid: false };
 
    component.onSubmit(form);
 
    expect(authService.signUp).not.toHaveBeenCalled();
    expect(component.loading).toBe(false);
  });

});
