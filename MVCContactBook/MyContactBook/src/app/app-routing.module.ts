import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SigninComponent } from './components/auth/signin/signin.component';
import { SignupComponent } from './components/auth/signup/signup.component';
import { HomeComponent } from './components/home/home.component';
import { ContactListComponent } from './components/contacts/contact-list/contact-list.component';
import { AddContactComponent } from './components/contacts/add-contact/add-contact.component';
import { EditContactComponent } from './components/contacts/edit-contact/edit-contact.component';
import { ContactDetailComponent } from './components/contacts/contact-detail/contact-detail.component';
import { SignupSuccessComponent } from './components/auth/signup-success/signup-success.component';
import { authGuard } from './guards/auth.guard';
import { ContactListPaginatedComponent } from './components/contacts/contact-list-paginated/contact-list-paginated.component';
import { FavouriteListComponent } from './components/contacts/favourite-list/favourite-list.component';
import { ForgotPasswordComponent } from './components/auth/forgot-password/forgot-password.component';
import { ChangePasswordComponent } from './components/auth/change-password/change-password.component';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { ViewProfileComponent } from './components/auth/view-profile/view-profile.component';
import { EditUserComponent } from './components/auth/edit-user/edit-user.component';
import { AddContactTfComponent } from './components/contacts/add-contact-tf/add-contact-tf.component';
import { EditContactTfComponent } from './components/contacts/edit-contact-tf/edit-contact-tf.component';
import { BasedOnBirthdayComponent } from './components/reports/based-on-birthday/based-on-birthday.component';
import { BasedOnStateComponent } from './components/reports/based-on-state/based-on-state.component';
import { BasedOnCountryComponent } from './components/reports/based-on-country/based-on-country.component';
import { BasedOnGenderComponent } from './components/reports/based-on-gender/based-on-gender.component';

const routes: Routes = [
  {path:'',redirectTo:'home',pathMatch:'full'},
  {path:'home', component:HomeComponent},
  {path:'contacts', component:ContactListComponent},
  {path:'signup', component:SignupComponent},
  {path:'signin', component:SigninComponent},
  {path:'addContact', component:AddContactComponent, canActivate:[authGuard]},
  {path:'addContactTF', component:AddContactTfComponent, canActivate:[authGuard]},
  {path:'editContact/:id', component:EditContactComponent, canActivate:[authGuard]},
  {path:'editContactTF/:contactId', component:EditContactTfComponent, canActivate:[authGuard]},
  {path:'contactDetails/:id', component:ContactDetailComponent},
  {path: 'signupsuccess', component: SignupSuccessComponent},
  {path: 'paginatedContacts', component: ContactListPaginatedComponent},
  {path: 'favourites', component: FavouriteListComponent},
  {path: 'forgotPassword', component: ForgotPasswordComponent},
  {path: 'changePassword', component: ChangePasswordComponent},
  {path: 'profile/:loginId', component: ViewProfileComponent},
  {path: 'editUser/:loginId', component: EditUserComponent},
  {path: 'basedOnBirthday', component: BasedOnBirthdayComponent},
  {path: 'basedOnState', component: BasedOnStateComponent},
  {path: 'basedOnCountry', component: BasedOnCountryComponent},
  {path: 'basedOnGender', component: BasedOnGenderComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes), NgbDropdownModule],
  exports: [RouterModule]
})
export class AppRoutingModule { }
