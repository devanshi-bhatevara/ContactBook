import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Contact } from '../models/contact.model';
import { ApiResponse } from '../models/ApiResponse{T}';
import { AddContact } from '../models/add-contact.model';
import { EditContact } from '../models/edit-contact.model';
import { ContactSP } from '../models/contactSP.model';


@Injectable({
  providedIn: 'root'
})
export class ContactService {

  private apiUrl = 'http://localhost:5104/api/Contact/';

  constructor(private http: HttpClient) { }

  getAllContacts(): Observable<ApiResponse<Contact[]>> {
    return this.http.get<ApiResponse<Contact[]>>(this.apiUrl + "GetAllContacts");
  }

  getAllFavouriteContacts():Observable<ApiResponse<Contact[]>>{
    return this.http.get<ApiResponse<Contact[]>>(this.apiUrl + 'GetAllFavouriteContacts')
  }

  getAllPaginatedContacts(page: number, pageSize: number, sortOrder: string,letter?: string,search?: string): Observable<ApiResponse<Contact[]>> {
    if (letter != null && search !=null) {
      return this.http.get<ApiResponse<Contact[]>>(this.apiUrl + `GetAllContactsByPagination?letter=${letter}&search=${search}&page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`);
    }
    else if(letter==null && search != null){
      return this.http.get<ApiResponse<Contact[]>>(this.apiUrl + `GetAllContactsByPagination?search=${search}&page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`);
    }
    else if(letter!=null && search == null){
      return this.http.get<ApiResponse<Contact[]>>(this.apiUrl + `GetAllContactsByPagination?letter=${letter}&page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`);
    }
    else {
      return this.http.get<ApiResponse<Contact[]>>(this.apiUrl + `GetAllContactsByPagination?page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`);
    }
  }

  fetchContactCount(letter?: string,search?: string): Observable<ApiResponse<number>> {
    if (letter != null && search!=null) {
      return this.http.get<ApiResponse<number>>(this.apiUrl + `GetContactsCount?letter=${letter}&search=${search}`);
    }
    else if (letter == null && search!=null) {
      return this.http.get<ApiResponse<number>>(this.apiUrl + `GetContactsCount?search=${search}`);
    }
    else if (letter != null && search==null) {
      return this.http.get<ApiResponse<number>>(this.apiUrl + `GetContactsCount?letter=${letter}`);
    }
    else {
      return this.http.get<ApiResponse<number>>(this.apiUrl + 'GetContactsCount');
    }
  }

  getAllFavouritePaginatedContacts(page: number, pageSize: number,sortOrder: string, letter?: string): Observable<ApiResponse<Contact[]>> {
    if (letter != null) {
      return this.http.get<ApiResponse<Contact[]>>(this.apiUrl + `GetAllFavouriteContactsByPagination?letter=${letter}&sortOrder=${sortOrder}&page=${page}&pageSize=${pageSize}`);
    }
    else {
      return this.http.get<ApiResponse<Contact[]>>(this.apiUrl + `GetAllFavouriteContactsByPagination?sortOrder=${sortOrder}&page=${page}&pageSize=${pageSize}`);
    }
  }

  fetchFavouriteContactCount(letter?: string): Observable<ApiResponse<number>> {
    if (letter != null) {
      return this.http.get<ApiResponse<number>>(this.apiUrl + 'GetFavouriteContactsCount?letter=' + letter);
    }
    else {
      return this.http.get<ApiResponse<number>>(this.apiUrl + 'GetFavouriteContactsCount');
    }
  }


  addContact(addContact: AddContact): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(`${this.apiUrl}Create`, addContact);
  }

  getContactById(contactId: number): Observable<ApiResponse<EditContact>> {
    return this.http.get<ApiResponse<EditContact>>(`${this.apiUrl}GetContactById/${contactId}`)
  }

  modifyContact(editContact: EditContact): Observable<ApiResponse<EditContact>> {
    return this.http.put<ApiResponse<EditContact>>(`${this.apiUrl}ModifyContact`, editContact);
  }

  deleteContact(contactId: number): Observable<ApiResponse<Contact>> {
    return this.http.delete<ApiResponse<Contact>>(`${this.apiUrl}Remove/${contactId}`)

  }
  getContactsBasedOnBirthdayMonth(month: number):Observable<ApiResponse<ContactSP[]>>{
     return this.http.get<ApiResponse<ContactSP[]>>(`${this.apiUrl}GetContactsBasedOnBirthdayMonth/?month=${month}`)
  }

  getContactByState(state: number):Observable<ApiResponse<ContactSP[]>>{
    return this.http.get<ApiResponse<ContactSP[]>>(`${this.apiUrl}getContactByState/?state=${state}`)
  }

  getContactsCountBasedOnCountry(countryId: number):Observable<ApiResponse<number>>{
    return this.http.get<ApiResponse<number>>(`${this.apiUrl}GetContactsCountBasedOnCountry/?countryId=${countryId}`)
 }

  getContactsCountBasedOnGender(gender: string):Observable<ApiResponse<number>>{
    return this.http.get<ApiResponse<number>>(`${this.apiUrl}GetContactsCountBasedOnGender/?gender=${gender}`)
 }



}
