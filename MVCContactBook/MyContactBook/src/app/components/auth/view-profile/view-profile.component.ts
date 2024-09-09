import { ChangeDetectorRef, Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserDetail } from 'src/app/models/user-details.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-view-profile',
  templateUrl: './view-profile.component.html',
  styleUrls: ['./view-profile.component.css']
})
export class ViewProfileComponent {
  loginId: string|null| undefined;
  isAuthenticated: boolean = false;

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

  constructor(private authService: AuthService,  private cdr: ChangeDetectorRef, private route: ActivatedRoute){
  }

  ngOnInit(): void{
    this.authService.isAuthenticated().subscribe((authState:boolean)=>{
      this.isAuthenticated=authState;
      this.cdr.detectChanges();
     });
     this.authService.getUsername().subscribe((username:string |null|undefined)=>{
      this.loginId=username;
      this.cdr.detectChanges();
     });
     const loginId = String(this.route.snapshot.paramMap.get('loginId'));
     console.log(loginId);
     this.authService.getUserByLoginId(loginId).subscribe({
       next: (response) => {
         if (response.success) {
           this.user = response.data;
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
 