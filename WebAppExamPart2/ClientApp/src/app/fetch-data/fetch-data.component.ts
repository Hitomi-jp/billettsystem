import { Component, Inject } from '@angular/core';
import { NgModule } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Kunde } from "../Kunde";
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Billett } from '../Billett';
import { MaterialModule } from '../material.module'
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from '../app.component';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html',
  styleUrls: ['./fetch-data.component.css']
})

@NgModule({
  declarations: [

  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    ReactiveFormsModule,
    HttpClient,
    MaterialModule

  ],
  providers: [],
  bootstrap: [AppComponent]
})

export class FetchDataComponent {
  alleKunder: Kunde[]
  alleBilletter: Billett[]
  alleBillettOgKunder: any = []
  laster: boolean;

  constructor(private _http: HttpClient, private router: Router, private formBuilder: FormBuilder, modalService: NgbModal) { }

  ngOnInit() {
    this.hentAlleKunder();
  }

  hentAlleKunder() {
    this.laster = true;
    this._http.get<Kunde[]>("api/kunde/HentAlleKunder")
      .subscribe(kundene => {
        this.alleKunder = kundene;
        this.hentAlleBilletter();
      },
        error => {
          if (error.status === 401) {
            this.router.navigate(['/login'])
          }
        });
  }

  hentAlleBilletter() {
    this._http.get<Billett[]>("api/billett/hentAlleBilletter")
      .subscribe(billettene => {
        this.alleBilletter = billettene;
        console.log(billettene)
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

  slettEnBillett(billettId: number) {
    const slettBillett = this.findBillett(billettId);
    this._http.delete("api/billett?billettId=" + billettId)
      .subscribe(response => {
        console.log("slettet billett")
        this.slettEnKunde(slettBillett.kundeId);
      })
  }

  slettEnKunde(kundeId: number) {
    this._http.delete("api/kunde/slettEnKunde?kundeId=" + kundeId)
      .subscribe(response => {
        console.log("slettet kunde")
        this.alleBillettOgKunder = [];
        this.hentAlleKunder()
      })
  }

  joinKundeOgBillett() {
    this.alleKunder.forEach((kunde: Kunde) => {
      this.alleBilletter.forEach((billett: Billett) => {
        if (kunde.id === billett.kundeId) {
          const data = { ...kunde, ...billett }
          this.alleBillettOgKunder.push(data)
        }
      })
    })
  }

  findBillett(billettId) {
    return this.alleBilletter.find(billett => billett.id === billettId)
  }

  slettAlle() {

  }
}
