import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewProfileComponent } from './view-profile.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { AuthService } from 'src/app/services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UserDetail } from 'src/app/models/user-details.model';
import { BehaviorSubject, of, throwError } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';

describe('ViewProfileComponent', () => {
  let component: ViewProfileComponent;
  let fixture: ComponentFixture<ViewProfileComponent>;
  let authService: jasmine.SpyObj<AuthService>;
  const mockUser: UserDetail = {
    firstName: '',
    lastName: '',
    loginId: 'testUser',
    contactNumber: '',
    fileName: null,
    imageByte: '',
    email: '',
    userId: 0
  }
  beforeEach(() => {
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['getUserByLoginId', 'getUsername','isAuthenticated']);
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule,RouterTestingModule],
      declarations: [ViewProfileComponent],
      providers: [
        { provide: AuthService, useValue: authServiceSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: { paramMap: { get: () => 'testUser'} }
          }
        }
      ]
    });
    fixture = TestBed.createComponent(ViewProfileComponent);
    component = fixture.componentInstance;
    authService = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set isAuthenticated, get username and get userDetails correctly', () => {
    // Arrange
    const mockAuthState = new BehaviorSubject<boolean>(true);
    authService.isAuthenticated.and.returnValue(mockAuthState.asObservable());
    const usernameSubject = new BehaviorSubject<string | null | undefined>('testUser');
    authService.getUsername.and.returnValue(usernameSubject.asObservable());
    const mockResponse: ApiResponse<UserDetail> = { success: true, data: mockUser, message: '' };
    authService.getUserByLoginId.and.returnValue(of(mockResponse));
    // Act (ngOnInit will be called automatically)
    fixture.detectChanges();

    // Assert
    expect(authService.isAuthenticated).toHaveBeenCalledWith();
    expect(component.isAuthenticated).toEqual(true);
    expect(authService.getUsername).toHaveBeenCalled();
    expect(component.loginId).toEqual('testUser');
    expect(authService.getUserByLoginId).toHaveBeenCalledWith("testUser"); // Adjust based on your actual implementation
  });

  it('should set isAuthenticated false,not get username and not get userDetails', () => {
    // Arrange
    const mockAuthState = new BehaviorSubject<boolean>(false);
    authService.isAuthenticated.and.returnValue(mockAuthState.asObservable());
    const usernameSubject = new BehaviorSubject<string | null | undefined>(null);
    authService.getUsername.and.returnValue(usernameSubject.asObservable());
    const mockError = { message: 'Network error' };
    authService.getUserByLoginId.and.returnValue(throwError(()=> mockError));
    spyOn(console,'error');
    // Act (ngOnInit will be called automatically)
    fixture.detectChanges();

    // Assert
    expect(authService.isAuthenticated).toHaveBeenCalledWith();
    expect(component.isAuthenticated).toEqual(false);
    expect(authService.getUsername).toHaveBeenCalled();
    expect(authService.getUserByLoginId).toHaveBeenCalled(); 
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contact', mockError)
  });

  it('should set isAuthenticated true,not get username and not get userDetails', () => {
    // Arrange
    const mockAuthState = new BehaviorSubject<boolean>(true);
    authService.isAuthenticated.and.returnValue(mockAuthState.asObservable());
    const usernameSubject = new BehaviorSubject<string | null | undefined>(null);
    authService.getUsername.and.returnValue(usernameSubject.asObservable());
    const mockError = { message: 'Network error' };
    authService.getUserByLoginId.and.returnValue(throwError(()=> mockError));
    spyOn(console,'error');
    // Act (ngOnInit will be called automatically)
    fixture.detectChanges();

    // Assert
    expect(authService.isAuthenticated).toHaveBeenCalledWith();
    expect(component.isAuthenticated).toEqual(true);
    expect(authService.getUsername).toHaveBeenCalled();
    expect(authService.getUserByLoginId).toHaveBeenCalled(); 
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contact', mockError)
  });
 
  it('should set isAuthenticated true, get username and not get userDetails', () => {
    // Arrange
    const mockAuthState = new BehaviorSubject<boolean>(true);
    authService.isAuthenticated.and.returnValue(mockAuthState.asObservable());
    const usernameSubject = new BehaviorSubject<string | null | undefined>('testUser');
    authService.getUsername.and.returnValue(usernameSubject.asObservable());
    const mockError = { message: 'Network error' };
    authService.getUserByLoginId.and.returnValue(throwError(()=> mockError));
    spyOn(console,'error');
    // Act (ngOnInit will be called automatically)
    fixture.detectChanges();

    // Assert
    expect(authService.isAuthenticated).toHaveBeenCalledWith();
    expect(component.isAuthenticated).toEqual(true);
    expect(authService.getUsername).toHaveBeenCalled();
    expect(component.loginId).toEqual('testUser');
    expect(authService.getUserByLoginId).toHaveBeenCalled(); 
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contact', mockError)
  });

  it('should set isAuthenticated true, get username and get success false in userDetails', () => {
    // Arrange
    const mockAuthState = new BehaviorSubject<boolean>(true);
    authService.isAuthenticated.and.returnValue(mockAuthState.asObservable());
    const usernameSubject = new BehaviorSubject<string | null | undefined>('testUser');
    authService.getUsername.and.returnValue(usernameSubject.asObservable());
    const mockResponse: ApiResponse<UserDetail> = { success: false, data: mockUser, message: '' };
    authService.getUserByLoginId.and.returnValue(of(mockResponse));
    spyOn(console,'error');
    // Act (ngOnInit will be called automatically)
    fixture.detectChanges();

    // Assert
    expect(authService.isAuthenticated).toHaveBeenCalledWith();
    expect(component.isAuthenticated).toEqual(true);
    expect(authService.getUsername).toHaveBeenCalled();
    expect(component.loginId).toEqual('testUser');
    expect(authService.getUserByLoginId).toHaveBeenCalledWith('testUser'); 
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contact', mockResponse.message)
  });
});
