import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KjopBillettComponent } from './kjop-billett.component';

describe('KjopBillettComponent', () => {
  let component: KjopBillettComponent;
  let fixture: ComponentFixture<KjopBillettComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KjopBillettComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KjopBillettComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
