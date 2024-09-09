import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BasedOnBirthdayComponent } from './based-on-birthday.component';

describe('BasedOnBirthdayComponent', () => {
  let component: BasedOnBirthdayComponent;
  let fixture: ComponentFixture<BasedOnBirthdayComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BasedOnBirthdayComponent]
    });
    fixture = TestBed.createComponent(BasedOnBirthdayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
