import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BasedOnCountryComponent } from './based-on-country.component';

describe('BasedOnCountryComponent', () => {
  let component: BasedOnCountryComponent;
  let fixture: ComponentFixture<BasedOnCountryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BasedOnCountryComponent]
    });
    fixture = TestBed.createComponent(BasedOnCountryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
