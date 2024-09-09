import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { UserDetail } from './models/user-details.model';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root', 
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'MyContactBook';
  isAuthenticated = false;
  username: string | null | undefined;
  imageUrl: string | ArrayBuffer | null = null;
  user:UserDetail={
    firstName: '',
    lastName: '',
    loginId: '',
    contactNumber: '',
    fileName: null,
    imageByte: '',
    email: '',
    userId: 0
  }
  private userSubscription: Subscription | undefined

  constructor(private authService: AuthService, private cdr: ChangeDetectorRef, private route:ActivatedRoute){}

  ngOnInit(): void {
    this.userSubscription = this.authService.getUsername().subscribe((username: string | null | undefined) => {
      this.username = username;
      if (this.username) {
        this.getUser();
      }
      this.cdr.detectChanges(); //Manually trigger change detection.
    });
 
    this.authService.isAuthenticated().subscribe((authState: boolean) => {
      this.isAuthenticated = authState;
      this.cdr.detectChanges(); //Manually trigger change detection.
    });

     // Subscribe to profile updated event
     this.userSubscription = this.authService.onProfileUpdated().subscribe(() => {
      this.getUser();
    });
  }
 
  ngOnDestroy(): void {
    // Unsubscribe to prevent memory leaks
    if (this.userSubscription) {
      this.userSubscription.unsubscribe();
    }
  }
 
  signOut(){
    this.authService.signOut();
  }

  getUser()
  {
    const loginId = this.username;
    console.log(loginId);
    this.authService.getUserByLoginId(loginId).subscribe({
      next: (response) => {
        if (response.success) {
          this.user = response.data;
          if (this.user.imageByte) {
            this.imageUrl = 'data:image/jpeg;base64,' + this.user.imageByte;
          } else {
            this.imageUrl = null;
          }
        } else {
          console.error('Failed to fetch contact', response.message);
        }
      },
      error: (error) => {
        console.error('Failed to fetch contact', error);
      },
    });
  }
  
  }

