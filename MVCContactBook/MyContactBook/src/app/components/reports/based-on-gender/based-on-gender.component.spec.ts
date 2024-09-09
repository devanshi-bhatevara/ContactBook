import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BasedOnGenderComponent } from './based-on-gender.component';

describe('BasedOnGenderComponent', () => {
  let component: BasedOnGenderComponent;
  let fixture: ComponentFixture<BasedOnGenderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BasedOnGenderComponent]
    });
    fixture = TestBed.createComponent(BasedOnGenderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
