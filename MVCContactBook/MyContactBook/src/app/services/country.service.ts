import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Country } from '../models/country.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CountryService {

  private apiUrl = 'http://localhost:5104/api/Country/';

  constructor(private http: HttpClient) { }

  getAllCountries():Observable<ApiResponse<Country[]>>{
    return this.http.get<ApiResponse<Country[]>>(this.apiUrl + "GetAllCountries");
  }
}
