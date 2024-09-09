import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { ContactListComponent } from './components/contacts/contact-list/contact-list.component';
import { SigninComponent } from './components/auth/signin/signin.component';
import { SignupComponent } from './components/auth/signup/signup.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AddContactComponent } from './components/contacts/add-contact/add-contact.component';
import { EditContactComponent } from './components/contacts/edit-contact/edit-contact.component';
import { ContactDetailComponent } from './components/contacts/contact-detail/contact-detail.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from './services/auth.service';
import { SignupSuccessComponent } from './components/auth/signup-success/signup-success.component';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { ContactListPaginatedComponent } from './components/contacts/contact-list-paginated/contact-list-paginated.component';
import { FavouriteListComponent } from './components/contacts/favourite-list/favourite-list.component';
import { DatePipe } from '@angular/common';
import { ForgotPasswordComponent } from './components/auth/forgot-password/forgot-password.component';
import { ChangePasswordComponent } from './components/auth/change-password/change-password.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ViewProfileComponent } from './components/auth/view-profile/view-profile.component';
import { EditUserComponent } from './components/auth/edit-user/edit-user.component';
import { AddContactTfComponent } from './components/contacts/add-contact-tf/add-contact-tf.component';
import { EditContactTfComponent } from './components/contacts/edit-contact-tf/edit-contact-tf.component';
import { BasedOnBirthdayComponent } from './components/reports/based-on-birthday/based-on-birthday.component';
import { BasedOnStateComponent } from './components/reports/based-on-state/based-on-state.component';
import { BasedOnCountryComponent } from './components/reports/based-on-country/based-on-country.component';
import { BasedOnGenderComponent } from './components/reports/based-on-gender/based-on-gender.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ContactListComponent,
    SigninComponent,
    SignupComponent,
    AddContactComponent,
    EditContactComponent,
    ContactDetailComponent,
    SignupSuccessComponent,
    ContactListPaginatedComponent,
    FavouriteListComponent,
    ForgotPasswordComponent,
    ChangePasswordComponent,
    ViewProfileComponent,
    EditUserComponent,
    AddContactTfComponent,
    EditContactTfComponent,
    BasedOnBirthdayComponent,
    BasedOnStateComponent,
    BasedOnCountryComponent,
    BasedOnGenderComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    NgbModule

  ],
  providers: [AuthService,{provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true},DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
