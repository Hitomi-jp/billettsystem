import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Rute } from './Rute';
import { Router } from '@angular/router';

@Component({
  selector: 'app-ruter',
  templateUrl: './ruter.component.html',
  styleUrls: ['./ruter.component.css']
})
export class RuterComponent implements OnInit {
  alleRuter: Rute[];
  visForm: boolean;
  visRuterListe: boolean;
  visFormLagre: boolean;
  visFormEndre: boolean;
  skjema: FormGroup;
  laster: boolean;
  endreRuteId: number;

  validering = {
    boatNavn: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
    ],
    ruteFra: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
    ],
    ruteTil: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
    ],
    prisEnvei: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9]+")])
    ],
    prisToVei: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9]+")])
    ],
    prisRabattBarn: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9]{1,2}")])
    ],
    prisStandardLugar: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9]+")])
    ],
    prisPremiumLugar: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9]+")])
    ],
    avgang: [
      null, Validators.compose([Validators.required, Validators.pattern("[012][0-9][:][0-5][0-9]")])
    ],
    antallDagerEnVei: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9]+")])
    ],
    antallDagerToVei: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9]+")])
    ],
  }

  constructor(private _http: HttpClient, private router: Router, private fb: FormBuilder) {
    this.skjema = fb.group(this.validering)
  }

  ngOnInit() {
    this.hentAlleRuter()
  }

  vedAvgangChange() {
    console.log(this.skjema.value.avgang)
  }
  vedSubmit() {
    if (this.visFormLagre) {
      this.registrerEnRute()
    } else {
      this.endreRute()
    }
  }

  visRegistrerRuteForm() {
    this.skjema.setValue({
      boatNavn: "",
      ruteFra: "",
      ruteTil: "",
      prisEnvei: "",
      prisToVei: "",
      prisRabattBarn: "",
      prisStandardLugar: "",
      prisPremiumLugar: "",
      avgang: "",
      antallDagerEnVei: "",
      antallDagerToVei: ""
    });
    this.skjema.markAsPristine();
    this.visForm = true;
    this.visFormLagre = true;
    this.visFormEndre = false;
    this.visRuterListe = false;
  }

  registrerEnRute() {
    const nyRute = new Rute();
    nyRute.boatNavn = this.skjema.value.boatNavn;
    nyRute.ruteFra = this.skjema.value.ruteFra;
    nyRute.ruteTil = this.skjema.value.ruteTil;
    nyRute.prisEnvei = this.skjema.value.prisEnvei;
    nyRute.prisToVei = this.skjema.value.prisToVei;
    let rabatt = this.skjema.value.prisRabattBarn.toString();
    if (this.skjema.value.prisRabattBarn < 10) {
      rabatt = this.skjema.value.prisRabattBarn.toString().padStart(2, 0);
    }
    nyRute.prisRabattBarn = rabatt;
    nyRute.prisStandardLugar = this.skjema.value.prisStandardLugar;
    nyRute.prisPremiumLugar = this.skjema.value.prisPremiumLugar;
    nyRute.avgang = this.skjema.value.avgang;
    nyRute.antallDagerEnVei = this.skjema.value.antallDagerEnVei;
    nyRute.antallDagerToVei = this.skjema.value.antallDagerToVei;

    this._http.post("api/rute", nyRute)
      .subscribe(retur => {
        this.hentAlleRuter();
        this.visForm = false;
        this.visFormLagre = false;
      })
      error => {
        console.log(error)
      }
  }

  hentAlleRuter() {
    this.visRuterListe = false;
    this.laster = true;
    this._http.get<any>("api/Rute")
      .subscribe(ruter => {
        console.log(ruter)
        this.alleRuter = ruter;
      },
        error => {
          if (error.status === 401) {
            this.router.navigate(['/login'])
          }
        });
    this.laster = false;
    this.visRuterListe = true;
  };

  hentEnRute(ruteId: number) {
    this.endreRuteId = ruteId;
    this.visRuterListe = false;
    this._http.get<Rute>("api/rute/" + ruteId)
      .subscribe(rute => {
        this.skjema.patchValue({ boatNavn: rute.boatNavn });
        this.skjema.patchValue({ ruteFra: rute.ruteFra });
        this.skjema.patchValue({ ruteTil: rute.ruteTil });
        this.skjema.patchValue({ prisEnvei: rute.prisEnvei });
        this.skjema.patchValue({ prisToVei: rute.prisToVei });
        this.skjema.patchValue({ prisRabattBarn: rute.prisRabattBarn });
        this.skjema.patchValue({ prisStandardLugar: rute.prisStandardLugar });
        this.skjema.patchValue({ prisPremiumLugar: rute.prisPremiumLugar });
        this.skjema.patchValue({ avgang: rute.avgang });
        this.skjema.patchValue({ antallDagerEnVei: rute.antallDagerEnVei });
        this.skjema.patchValue({ antallDagerToVei: rute.antallDagerToVei });
      },
        error => console.log(error)
      );
    this.visForm = true;
    this.visFormEndre = true;
    this.visFormLagre = false;
  }

  endreRute() {
    const endretRute = new Rute();
    endretRute.id = this.endreRuteId;
    endretRute.boatNavn = this.skjema.value.boatNavn;
    endretRute.ruteFra = this.skjema.value.ruteFra;
    endretRute.ruteTil = this.skjema.value.ruteTil;
    endretRute.prisEnvei = this.skjema.value.prisEnvei;
    endretRute.prisToVei = this.skjema.value.prisToVei;
    endretRute.prisRabattBarn = this.skjema.value.prisRabattBarn.toString();
    endretRute.prisStandardLugar = this.skjema.value.prisStandardLugar;
    endretRute.prisPremiumLugar = this.skjema.value.prisPremiumLugar;
    endretRute.avgang = this.skjema.value.avgang;
    endretRute.antallDagerEnVei = this.skjema.value.antallDagerEnVei;
    endretRute.antallDagerToVei = this.skjema.value.antallDagerToVei;

    this._http.put("api/rute/", endretRute)
      .subscribe(
        retur => {
          this.hentAlleRuter();
          this.visRuterListe = true;
          this.visForm = false;
        },
        error => console.log(error)
      );
  }

  slettEnRute(ruteId: number) {
    this._http.delete("api/rute/" + ruteId)
      .subscribe(retur => {
        this.hentAlleRuter();
      },
        error => console.log(error)
      );
  }

  tilbakeTilListe() {
    this.visForm = false;
    this.visRuterListe = true;
  }
}
