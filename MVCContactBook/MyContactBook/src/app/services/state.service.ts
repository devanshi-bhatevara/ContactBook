import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { State } from '../models/state.model';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StateService {
  private apiUrl = 'http://localhost:5104/api/State/';

  constructor(private http: HttpClient) { }

  getAllStates():Observable<ApiResponse<State[]>>{
    return this.http.get<ApiResponse<State[]>>(this.apiUrl + "GetAllStates");
  }
  getStatesByCountry(contactId : number):Observable<ApiResponse<State[]>>{
    return this.http.get<ApiResponse<State[]>>(`${this.apiUrl}GetStateByCountry/${contactId}`);
  }
  
}
