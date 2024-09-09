import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiResponse } from '../models/ApiResponse{T}';
import { BehaviorSubject, Observable, Subject, tap } from 'rxjs';
import { LocalstorageService } from './helpers/localstorage.service';
import { User } from '../models/user.model';
import { LocalStorageKeys } from './helpers/localstoragekeys';
import { ForgotPassword } from '../models/forgot-password.model';
import { UpdateUser } from '../models/update-user.model';
import { UserDetail } from '../models/user-details.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl='http://localhost:5104/api/auth/';
  private authState = new BehaviorSubject<boolean>(this.localStorageHelper.hasItem(LocalStorageKeys.TokenName));
  private usernameSubject = new BehaviorSubject<string | null | undefined>(this.localStorageHelper.getItem(LocalStorageKeys.UserId));
    constructor(private localStorageHelper: LocalstorageService, private http:HttpClient) { }
   
    signUp(user: User): Observable<ApiResponse<string>>{
      const body =user;
      return this.http.post<ApiResponse<string>>(this.apiUrl+"Register",body);
    }

   signIn(username: string, password: string): Observable<ApiResponse<string>> {
    const body = { username, password};
     return this.http.post<ApiResponse<string>>(this.apiUrl+"Login",body).pipe(
      tap(response => {
        if (response.success) {
          this.localStorageHelper.setItem(LocalStorageKeys.TokenName, response.data);
          this.localStorageHelper.setItem(LocalStorageKeys.UserId, username);
          this.authState.next(this.localStorageHelper.hasItem(LocalStorageKeys.TokenName));
          this.usernameSubject.next(username);
        }
      })
     )
  
   }

   forgotPassword(forgotPassword: ForgotPassword){
    const body =forgotPassword;
    return this.http.post<ApiResponse<string>>(this.apiUrl+"ForgetPassword",body);
   }

   getUserByLoginId(loginId: string| null| undefined): Observable<ApiResponse<UserDetail>> {
    return this.http.get<ApiResponse<UserDetail>>(`${this.apiUrl}GetUserById/${loginId}`)
  }

  modifyUser(editContact: UpdateUser): Observable<ApiResponse<UpdateUser>> {
    return this.http.put<ApiResponse<UpdateUser>>(`${this.apiUrl}ModifyUser`, editContact);
  }

   signOut(){
    this.localStorageHelper.removeItem(LocalStorageKeys.TokenName);
    this.localStorageHelper.removeItem(LocalStorageKeys.UserId);
    this.authState.next(false);
          this.usernameSubject.next(null);
   }

   isAuthenticated(){
    return this.authState.asObservable();
   }

   getUsername(): Observable<string | null | undefined>{
    return this.usernameSubject.asObservable();
   }

   private profileUpdatedSource = new Subject<void>();
 
   // Method to emit profile update event
   emitProfileUpdated(): void {
    this.profileUpdatedSource.next();
  }
 
  // Method to subscribe to profile update event
  onProfileUpdated(): Observable<void> {
    return this.profileUpdatedSource.asObservable();
  }
}
