import { TestBed } from '@angular/core/testing';

import { CountryService } from './country.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Country } from '../models/country.model';
import { ApiResponse } from '../models/ApiResponse{T}';

describe('CountryService', () => {
  let service: CountryService;
  let httpMock : HttpTestingController;

  const mockApiResponse : ApiResponse<Country[]>={
    success : true,
    data : [
      {countryId:1,
        countryName:'Country 1',
        
      },
      {countryId:2,
        countryName:'Country 2',
      }
    ],
    message: ''

  }
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule],
      providers :[CountryService]

    });    
    service = TestBed.inject(CountryService);
    httpMock = TestBed.inject(HttpTestingController);

  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
  it('should fetch all countries successfully',() =>{
    //arrange
    const apiUrl = 'http://localhost:5104/api/Country/GetAllCountries';
    //act
    service.getAllCountries().subscribe((response)=>{
     //Assert
     expect(response.data.length).toBe(2);
     expect(response.data).toEqual(mockApiResponse.data);
     });
   const req = httpMock.expectOne(apiUrl);
     expect(req.request.method).toBe('GET');
     req.flush(mockApiResponse);
     
   });
 
   it('should handle empty country list',()=>{
     //arrange
     const apiUrl = 'http://localhost:5104/api/Country/GetAllCountries';
     const emptyResponse : ApiResponse<Country[]>={
       success : true,
       data : [],
       message: ''
   
     }
     //act
     service.getAllCountries().subscribe((response)=>{
       //Assert
       expect(response.data.length).toBe(0);
       expect(response.data).toEqual([]);
     });
 
     const req = httpMock.expectOne(apiUrl);
     expect(req.request.method).toBe('GET');
     req.flush(emptyResponse);
   });
 
   it('should handle HTTP error gracefully',()=>{
     //Arrange
     const apiUrl = 'http://localhost:5104/api/Country/GetAllCountries';
     const errorMessage = 'Failed to load countries';
     //Act
     service.getAllCountries().subscribe(
       ()=> fail('expected an error, not countries'),
       (error) => {
         //Assert
         expect(error.status).toBe(500);
         expect(error.statusText).toBe('Internal Server Error');
         
       }
     );
     
 
 
   });
});