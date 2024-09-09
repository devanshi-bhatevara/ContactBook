import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContactDetailComponent } from './contact-detail.component';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ContactService } from 'src/app/services/contacts.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Contact } from 'src/app/models/contact.model';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { of, throwError } from 'rxjs';

describe('ContactDetailComponent', () => {
  let component: ContactDetailComponent;
  let fixture: ComponentFixture<ContactDetailComponent>;
  let categoryService: jasmine.SpyObj<ContactService>;
  let router: Router;


  const mockCategory: Contact = {
    contactId: 1,
    firstName: 'Test Category',
    lastName: 'Test description',
    phone: '',
    address: '',
    email: '',
    gender: '',
    isFavourite: false,
    countryId: 1,
    stateId: 1,
    fileName: null,
    imageByte: '',
    country: {
      countryId: 1,
      countryName: ''
    },
    state: {
      stateId: 1,
      stateName: '',
      countryId: 1
    },
    birthDate: new Date()
  };
  beforeEach(() => {
    const categoryServiceSpy = jasmine.createSpyObj('ContactService', ['getContactById', 'deleteContact']);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule,RouterTestingModule],
      declarations: [ContactDetailComponent],
      providers: [
        { provide: ContactService, useValue: categoryServiceSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: { paramMap: { get: () => '1' } }
          }
        }
      ]
    });
    fixture = TestBed.createComponent(ContactDetailComponent);
    component = fixture.componentInstance;
    categoryService = TestBed.inject(ContactService) as jasmine.SpyObj<ContactService>;
    // fixture.detectChanges();
    router = TestBed.inject(Router);

  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize categoryId from route params and load category details', () => {
    // Arrange
    const mockResponse: ApiResponse<Contact> = { success: true, data: mockCategory, message: '' };
    categoryService.getContactById.and.returnValue(of(mockResponse));

    // Act
    fixture.detectChanges(); // ngOnInit is called here

    // Assert
    expect(component.contact.contactId).toBe(1);
    expect(categoryService.getContactById).toHaveBeenCalledWith(1);
    expect(component.contact).toEqual(mockCategory);
  });

  it('should fail to load category details', () => {
    // Arrange
    const mockResponse: ApiResponse<Contact> = { success: false, data: mockCategory, message: '' };
    categoryService.getContactById.and.returnValue(of(mockResponse));
    spyOn(console, 'error')
    
    // Act
    fixture.detectChanges(); // ngOnInit is called here

    // Assert
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contact',mockResponse.message)
    expect(categoryService.getContactById).toHaveBeenCalledWith(1);
  });

  it('should handle http error', () => {
    // Arrange
    const mockError = { message: 'Network error' };
    categoryService.getContactById.and.returnValue(throwError(() => mockError));
    spyOn(console, 'error')

    // Act
    fixture.detectChanges(); // ngOnInit is called here

    // Assert
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contact',mockError)
    expect(categoryService.getContactById).toHaveBeenCalledWith(1);
  });

  it('should delete contact and navigate to categories list', () => {
    // Arrange
    const mockDeleteResponse: ApiResponse<Contact> = { success: true, data: mockCategory, message: 'Contact deleted successfully' };
    const contactId = 1;
    spyOn(window, 'confirm').and.returnValue(true);
    spyOn(router, 'navigate').and.stub();
    categoryService.deleteContact.and.returnValue(of(mockDeleteResponse));

    // Act
    component.deleteContact(contactId);

    // Assert
    expect(window.confirm).toHaveBeenCalledWith("Are you sure you want to delete this contact?")
    expect(categoryService.deleteContact).toHaveBeenCalledWith(contactId);
    expect(router.navigate).toHaveBeenCalledWith(['/paginatedContacts']);
  });


  it('should not delete category if user cancels', () => {
    // Arrange
    const mockDeleteResponse: ApiResponse<Contact> = { success: false, data: mockCategory, message: 'Failed to delete category' };
    const contactId = 1;
    spyOn(window, 'confirm').and.returnValue(false);
    spyOn(router, 'navigate').and.stub();
    categoryService.deleteContact.and.returnValue(of(mockDeleteResponse));

    // Act
    component.deleteContact(contactId);

    // Assert
    expect(window.confirm).toHaveBeenCalledWith("Are you sure you want to delete this contact?")
    expect(categoryService.deleteContact).not.toHaveBeenCalled();
    expect(router.navigate).not.toHaveBeenCalled();
  });

});
