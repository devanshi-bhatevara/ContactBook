import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BasedOnStateComponent } from './based-on-state.component';

describe('BasedOnStateComponent', () => {
  let component: BasedOnStateComponent;
  let fixture: ComponentFixture<BasedOnStateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BasedOnStateComponent]
    });
    fixture = TestBed.createComponent(BasedOnStateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
