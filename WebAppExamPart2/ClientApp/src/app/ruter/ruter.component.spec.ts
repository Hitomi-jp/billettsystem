import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RuterComponent } from './ruter.component';

describe('RuterComponent', () => {
  let component: RuterComponent;
  let fixture: ComponentFixture<RuterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RuterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RuterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
