import { TestBed } from '@angular/core/testing';

import { StateService } from './state.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ApiResponse } from '../models/ApiResponse{T}';
import { State } from '../models/state.model';

describe('StateService', () => {
  let service: StateService;
  let httpMock: HttpTestingController;
  const mockApiResponse: ApiResponse<State[]> = {
    success: true,
    data: [
      {
        stateId: 1,
        stateName: 'state 1',
        countryId: 1,

      },
      {
        stateId: 2,
        stateName: 'state 2',
        countryId: 1,
      }
    ],
    message: ''
  }

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule]
    });    
    service = TestBed.inject(StateService);
    httpMock = TestBed.inject(HttpTestingController);

  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
  it('should fetch states by country id', ()=>{
    //Arrange
    
    const apiUrl = 'http://localhost:5104/api/State/GetStateByCountry/1';
    //Act
    service.getStatesByCountry(1).subscribe((response)=>{
      //Assert
      expect(response.data.length).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);
      });
    const req = httpMock.expectOne(apiUrl);
      expect(req.request.method).toBe('GET');
      req.flush(mockApiResponse);
      
  })

  it('should handle empty state list by country id', ()=>{
    //Arrange
    
    const apiUrl = 'http://localhost:5104/api/State/GetStateByCountry/1';
    const emptyResponse : ApiResponse<State[]>={
      success : true,
      data : [],
      message: ''
  
    }
    //Act
    service.getStatesByCountry(1).subscribe((response)=>{
      //Assert
      expect(response.data.length).toBe(0);
      expect(response.data).toEqual([]);
      });
    const req = httpMock.expectOne(apiUrl);
      expect(req.request.method).toBe('GET');
      req.flush(emptyResponse);
      
  })
  it('should handle HTTP error garcefully',()=>{
    //Arrange
    const apiUrl = 'http://localhost:5104/api/State/GetStateByCountry/1';
    const errorMessage = 'Failed to load states';

    //Act
    service.getStatesByCountry(1).subscribe(
      ()=> fail('expected an error, not states'),
      (error) => {
        //Assert
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
        
      }
    );
   

  })
});
