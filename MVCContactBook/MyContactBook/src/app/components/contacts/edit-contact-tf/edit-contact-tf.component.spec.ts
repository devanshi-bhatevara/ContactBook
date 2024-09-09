import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditContactTfComponent } from './edit-contact-tf.component';
import { ContactService } from 'src/app/services/contacts.service';
import { CountryService } from 'src/app/services/country.service';
import { StateService } from 'src/app/services/state.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Contact } from 'src/app/models/contact.model';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule, NgForm } from '@angular/forms';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { of, throwError } from 'rxjs';
import { RouterTestingModule } from '@angular/router/testing';
import { State } from 'src/app/models/state.model';
import { Country } from 'src/app/models/country.model';
import { EditContact } from 'src/app/models/edit-contact.model';

describe('EditContactTfComponent', () => {
  let component: EditContactTfComponent;
  let fixture: ComponentFixture<EditContactTfComponent>;
  let contactServiceSpy: jasmine.SpyObj<ContactService>;
  let countryServiceSpy: jasmine.SpyObj<CountryService>;
  let stateServiceSpy: jasmine.SpyObj<StateService>;
  let routerSpy: Router
  // let route: ActivatedRoute;

  const mockContact: EditContact = {
    contactId: 1,
    firstName: 'Test Category',
    lastName: 'Test Description',
    phone: '1234567899',
    address: 'vadodara',
    email: 'test@gmail.com',
    gender: 'F',
    isFavourite: false,
    countryId: 1,
    stateId: 1,
    fileName: null,
    imageByte: '',
    country: {
      countryId: 1,
      countryName: 'USA'
    },
    state: {
      stateId: 1,
      stateName: 'California',
      countryId: 1
    },
    birthDate: new Date()
  };

  beforeEach(() => {
    contactServiceSpy = jasmine.createSpyObj('ContactService', ['getContactById','modifyContact']);
    countryServiceSpy = jasmine.createSpyObj('CountryService', ['getAllCountries']);
    stateServiceSpy = jasmine.createSpyObj('StateService', ['getStatesByCountry']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, FormsModule, RouterTestingModule.withRoutes([])],
      declarations: [EditContactTfComponent],
      providers: [     
        { provide: ContactService, useValue: contactServiceSpy },
        { provide: CountryService, useValue: countryServiceSpy },
        { provide: StateService, useValue: stateServiceSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: { paramMap: { get: () => '1' } }
          }
        }
      ]
    });
    fixture = TestBed.createComponent(EditContactTfComponent);
    component = fixture.componentInstance;
    routerSpy = TestBed.inject(Router)
    // route = TestBed.inject(ActivatedRoute);
    

  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  // it('should initialize contactId from route params and load contact details', () => {
  //   // Arrange
  //   const contactId = 1;
  //   const mockResponse: ApiResponse<EditContact> = { success: true, data: mockContact, message: '' };
  //   contactServiceSpy.getContactById.and.returnValue(of(mockResponse));

  //   // Act
  //   // component.ngOnInit();// ngOnInit is called here
  //   // component.contact.contactId=1;
  //   fixture.detectChanges();

  //   // Assert
  //   expect(component.contact.contactId).toBe(contactId);
  //   expect(contactServiceSpy.getContactById).toHaveBeenCalledWith(1);
  //   expect(component.contact).toEqual(mockContact);

  // });

  
  it('should initialize contactId from route params and load category details', () => {
    // Arrange
 
  
    const mockResponseCountries: ApiResponse<Country[]> = { success: true, data: [], message: '' };
    const mockResponseStates: ApiResponse<State[]> = { success: true, data: [], message: '' };
    const mockResponse: ApiResponse<EditContact> = { success: true, data: mockContact, message: '' };
    contactServiceSpy.getContactById.and.returnValue(of(mockResponse));
    countryServiceSpy.getAllCountries.and.returnValue(of(mockResponseCountries))
    stateServiceSpy.getStatesByCountry.and.returnValue(of(mockResponseStates))
    // Act
    component.ngOnInit();
    component.loadCountries();
    component.onSelectCountry(1); // ngOnInit is called here

    // Assert
    expect(component.contact.contactId).toBe(1);
    expect(contactServiceSpy.getContactById).toHaveBeenCalledWith(1);
    expect(component.contact).toEqual(mockContact);
  });

  

  it('should load countries', () => {
    // Arrange
    
  const mockCountries: Country[] = [
    { countryId: 1, countryName: 'Category 1'},
    { countryId: 2, countryName: 'Category 2'},
  ];
    const mockResponse: ApiResponse<Country[]> = { success: true, data: mockCountries, message: '' };
    countryServiceSpy.getAllCountries.and.returnValue(of(mockResponse));
 
    // Act
    component.loadCountries();
  // fixture.detectChanges();// ngOnInit is called here
 
    // Assert
    expect(countryServiceSpy.getAllCountries).toHaveBeenCalled();
    expect(component.country).toEqual(mockCountries);
  });
 
  it('should handle failed country loading', () => {
    // Arrange
    const mockResponse: ApiResponse<Country[]> = { success: false, data: [], message: 'Failed to fetch countries' };
    countryServiceSpy.getAllCountries.and.returnValue(of(mockResponse));
    spyOn(console, 'error');
 
    // Act
    component.loadCountries();
 
    // Assert
    expect(countryServiceSpy.getAllCountries).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch countries', 'Failed to fetch countries');
  });
 
  it('should handle error during country loading HTTP Error', () => {
    // Arrange
    const mockError = { message: 'Network error' };
    countryServiceSpy.getAllCountries.and.returnValue(throwError(() => mockError));
    spyOn(console, 'error');
 
    // Act
    component.loadCountries();
 
    // Assert
    expect(countryServiceSpy.getAllCountries).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Error fetching countries :', mockError);
  });

  it('should load state from country Id', () => {
    // Arrange
    const mockStates: State[] = [
      { countryId: 1, stateName: 'Category 1', stateId: 2},
      { countryId: 2, stateName: 'Category 2', stateId: 1},
    ];
    const mockResponse: ApiResponse<State[]> = { success: true, data: mockStates, message: '' };
    stateServiceSpy.getStatesByCountry.and.returnValue(of(mockResponse));
  const countryId = 1;
    // Act
    component.onSelectCountry(countryId) // ngOnInit is called here

    // Assert
    expect(stateServiceSpy.getStatesByCountry).toHaveBeenCalledWith(countryId);
    expect(component.state).toEqual(mockStates);
  });

  it('should not load state when response is false', () => {
    // Arrange

    const mockResponse: ApiResponse<State[]> = { success: false, data: [], message: 'Failed to fetch states' };
    stateServiceSpy.getStatesByCountry.and.returnValue(of(mockResponse));
    spyOn(console, 'error');

     const countryId = 1;
    // Act
    component.onSelectCountry(countryId) // ngOnInit is called here

    // Assert
    expect(stateServiceSpy.getStatesByCountry).toHaveBeenCalledWith(countryId);
    expect(console.error).toHaveBeenCalledWith('Failed to fetch states', 'Failed to fetch states');
  });

  it('should handle error during country loading HTTP Error', () => {
    // Arrange
    const mockError = { message: 'Network error' };
    stateServiceSpy.getStatesByCountry.and.returnValue(throwError(() => mockError));
    spyOn(console, 'error');
    const countryId = 1;

    // Act
    component.onSelectCountry(countryId) // ngOnInit is called here
 
    // Assert
    expect(stateServiceSpy.getStatesByCountry).toHaveBeenCalledWith(countryId);
    expect(console.error).toHaveBeenCalledWith('Error fetching states :', mockError);
  });

  it('should navigate to /paginatedContacts on successful contact modification', () => {
    // Arrange
    spyOn(routerSpy, 'navigate');
    const mockResponse: ApiResponse<EditContact> = { success: true, data: mockContact, message: '' };
    contactServiceSpy.modifyContact.and.returnValue(of(mockResponse));

    // Act
    component.contact.stateId = 2; // Ensure this.contact.stateId is set to match form.value.stateId
    component.onSubmit({ valid: true } as NgForm);

    // Assert
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/paginatedContacts']);
    expect(component.loading).toBe(false);
  });

  it('should alert error message on unsuccessful category modification', () => {
    // Arrange
    spyOn(window, 'alert'); // Spy on console.error method
    const mockResponse: ApiResponse<EditContact> = { success: false, data: mockContact, message: 'Error modifying category' };
    contactServiceSpy.modifyContact.and.returnValue(of(mockResponse));

    // Act
    component.contact.stateId = 2; // Ensure this.contact.stateId is set to match form.value.stateId
    component.onSubmit({ valid: true } as NgForm);

    // Assert
    expect(window.alert).toHaveBeenCalledWith(mockResponse.message); // Check if console.error was called with the correct error message
  });

  it('should alert error message on HTTP error', () => {
    // Arrange
    spyOn(window, 'alert');
    const mockError = { error: { message: 'HTTP error' } };
    contactServiceSpy.modifyContact.and.returnValue(throwError(mockError));

    // Act
    component.contact.stateId = 2; // Ensure this.contact.stateId is set to match form.value.stateId
    component.onSubmit({ valid: true } as NgForm);

    // Assert
    expect(window.alert).toHaveBeenCalledWith(mockError.error.message);
  });

  it('should not call categoryService.modifyCategory on invalid form submission', () => {
    // Arrange
    const form = { valid: false } as NgForm;

    // Act
    component.onSubmit(form);

    // Assert
    expect(contactServiceSpy.modifyContact).not.toHaveBeenCalled();
  });

});
