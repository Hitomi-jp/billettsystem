import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Kunde } from "../Kunde";
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Billett } from '../Billett';


@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html',
  styleUrls: ['./fetch-data.component.css']
})
export class FetchDataComponent {
  alleKunder: Kunde[]
  alleBilletter: Billett[]
  alle:any=[]
  laster: boolean;

  constructor(private _http: HttpClient, private router: Router, private formBuilder: FormBuilder, modalService: NgbModal) { }

  ngOnInit() {
    this.hentAlleKunder();
  }

  hentAlleKunder() {
    this.laster = true;
    this._http.get<Kunde[]>("api/kunde/")
      .subscribe(kundene => {
        this.alleKunder = kundene;
        this.hentAlleBilletter();
      },
        error => {
          if (error.status === 401) {
            this.router.navigate(['/login'])
          }
        });
      };

  hentAlleBilletter() {
    this._http.get<Billett[]>("api/kunde/hentAlleBilletter")
      .subscribe(billettene => {
        this.alleBilletter = billettene;
        if (this.alleKunder && this.alleBilletter) {
          this.joinKundeOgBillett()
        }
        this.laster = false;
      },
        error => {
          if (error === 401) {
            this.router.navigate(['/login'])
          }
        });
  };

  joinKundeOgBillett() {
    this.alleKunder.forEach((kunde: Kunde) => {
      this.alleBilletter.forEach((billett: Billett) => {
        if (kunde.id === billett.kundeId) {
          const data = { ...kunde, ...billett }
          this.alle.push(data)
        }
      })
    })
  }
}

