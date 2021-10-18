import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Kunde } from "../Kunde";
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Billett } from '../Billett';

@Component({
  //selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  alleKunder: Kunde[]
  alleBilletter: Billett[]
  alle:any=[]

  constructor(private _http: HttpClient, private router: Router, private formBuilder: FormBuilder, modalService: NgbModal) { }

  ngOnInit() {
    this.hentAlleKunder();
    this.hentAlleBilletter();
  }

  hentAlleKunder() {
    this._http.get<Kunde[]>("api/kunde/")
      .subscribe(kundene => {
        this.alleKunder = kundene;
      },
        error => console.log(error),
        () => console.log(this.alleKunder)
      );

  };

  hentAlleBilletter() {
    this._http.get<Billett[]>("api/kunde/hentAlleBilletter")
      .subscribe(billettene => {
        this.alleBilletter = billettene;
        if (this.alleKunder && this.alleBilletter) {
          this.joinKundeOgBillett()
        }
        
      },
        error => console.log(error),
        () => console.log(this.alleBilletter)
      );
  };

  joinKundeOgBillett() {
    this.alleKunder.forEach((kunde: Kunde) => {
      this.alleBilletter.forEach((billett: Billett) => {
        if (kunde.id === billett.kundeId) {
          const data = { ...kunde, ...billett }
          console.log(data)
          this.alle.push(data)
        }
      })
    })
  }
}

