import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Kunde } from "../Kunde";

@Component({
  templateUrl: "kjop.component.html"
})
export class KjopComponent {
  destinasjoner;
  gyldigDestinasjoner;
  selectedFra;

  skjema: FormGroup;

  validering = {
    id: [""],
    fornavn: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZæøåÆØÅ\.\ \-]{2,40}")])
    ],
    etternavn: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZæøåÆØÅ\.\ \-]{2,40}")])
    ],
    adresse: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZæøåÆØÅ.\-]+[a-zA-ZæøåÆØÅ0-9\ \_.]*[0-9]*")])
    ],
    postnr: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9]{4}")])
    ],
    poststed: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZæøåÆØÅ\ \.\-]{2,40}")])
    ]
  }

  constructor(private http: HttpClient, private fb: FormBuilder, private router: Router) {
    this.skjema = fb.group(this.validering);
  }

  ngOnInit() {
    this.hentAlleDestinasjoner();
  }

  hentAlleDestinasjon() {
    this.regKunde();
  }

  regKunde() {
    const registeredKunde = new Kunde();

    registeredKunde.fornavn = this.skjema.value.fornavn;
    registeredKunde.etternavn = this.skjema.value.etternavn;
    registeredKunde.adresse = this.skjema.value.adresse;
    registeredKunde.postnr = this.skjema.value.postnr;
    registeredKunde.poststed = this.skjema.value.poststed;

    this.http.post("api/kunde", registeredKunde)
      .subscribe(retur => {
        this.router.navigate(['/home']);
      },

       error => console.log(error)
      );
  };

  hentAlleDestinasjoner() {
    this.http.get<any>("api/kunde/hentAlleDestinasjon")
      .subscribe(destinasjonerList=> {
        this.destinasjoner = destinasjonerList;
      },
      error => {
        console.log(error)
      });
  };

  hentGyldigDestinasjoner(id) {
    this.http.get<any>("api/kunde/hentAlleDestinasjon", id)
      .subscribe(gyldigDestinasjoner => {
        this.gyldigDestinasjoner = gyldigDestinasjoner;
        console.log(this.gyldigDestinasjoner)
      },
      error => {
        console.log(error)
      });
  };

  onFraDestinasjonChange() {
    console.log(this.selectedFra)
  }
}
