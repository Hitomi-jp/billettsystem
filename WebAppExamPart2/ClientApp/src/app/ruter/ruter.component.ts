import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Rute } from './Rute';
import { Router } from '@angular/router';
import { Modal } from '../modal/modal';

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
      null, Validators.compose([Validators.required, Validators.pattern("[1-9]([0-9]+)?")])
    ],
    prisToVei: [
      null, Validators.compose([Validators.required, Validators.pattern("[1-9]([0-9]+)?")])
    ],
    prisRabattBarn: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9]{1,2}([0]{1})?")])
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
    ],
    antallDagerToVei: [
    ],
  }

  constructor(private _http: HttpClient, private router: Router, private fb: FormBuilder, private modalService: NgbModal) {
    this.skjema = fb.group(this.validering)
  }

  ngOnInit() {
    this.sjekkIsLoggetInn()
  }

  visModal(rute) {
    const modalRef = this.modalService.open(Modal, {
      backdrop: 'static',
      keyboard: false
    });

    modalRef.componentInstance.navn = "Rute: " + rute.boatNavn;

    modalRef.result.then(retur => {
      console.log('Lukket med:'+ retur);
      if (retur === 'Slett klikk') {
        this.slettEnRute(rute.id)
      }
    });
  }

  vedSubmit() {
    if (this.visFormLagre) {
      this.registrerEnRute()
    } else {
      this.endreRute()
    }
  }

  sjekkIsLoggetInn() {
    this._http.get<any>("api/Rute/sjekkAdminLoggetInn")
    .subscribe(isLoggetInn => {
      if (isLoggetInn) {
        this.hentAlleRuter()
      }
      else {
        this.router.navigate(['/login'])
      }
    },
      error => {
        console.log(error)
      }); 
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
      antallDagerEnVei: 0,
      antallDagerToVei: 0
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
    nyRute.prisEnvei = parseInt(this.skjema.value.prisEnvei);
    nyRute.prisToVei = parseInt(this.skjema.value.prisToVei);
    let rabatt = this.skjema.value.prisRabattBarn.toString();
    nyRute.prisRabattBarn = rabatt;
    nyRute.prisStandardLugar = parseInt(this.skjema.value.prisStandardLugar);
    nyRute.prisPremiumLugar = parseInt(this.skjema.value.prisPremiumLugar);
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
    endretRute.prisEnvei = parseInt(this.skjema.value.prisEnvei);
    endretRute.prisToVei = parseInt(this.skjema.value.prisToVei);
    endretRute.prisRabattBarn = this.skjema.value.prisRabattBarn.toString();
    endretRute.prisStandardLugar = parseInt(this.skjema.value.prisStandardLugar);
    endretRute.prisPremiumLugar = parseInt(this.skjema.value.prisPremiumLugar);
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

  vedAntallDagerEnveiPlus() {
    this.skjema.patchValue({
      antallDagerEnVei: this.skjema.value.antallDagerEnVei + 1
    })
  }

  vedAntallDagerEnveiMinus() {
    if (this.skjema.value.antallDagerEnVei < 1) {
      return;
    }
    this.skjema.patchValue({
      antallDagerEnVei: this.skjema.value.antallDagerEnVei - 1
    })
  }

  vedAntallDagerToVeiPlus() {
    this.skjema.patchValue({
      antallDagerToVei: this.skjema.value.antallDagerToVei + 1
    })
  }

  vedAntallDagerToVeiMinus() {
    if (this.skjema.value.antallDagerToVei < 1) {
      return;
    }
    this.skjema.patchValue({
      antallDagerToVei: this.skjema.value.antallDagerToVei - 1
    })
  }

  valideringNummer(tallInputType) {
    let pris;
    switch (tallInputType) {
      case 'rabatt':
        pris = this.skjema.value.prisRabattBarn;
        break;
      case 'prisEnvei':
        pris = this.skjema.value.prisEnvei;
        break;
      case 'prisToVei':
        pris = this.skjema.value.prisToVei;
        break;
      case 'prisStandardLugar':
        pris = this.skjema.value.prisStandardLugar;
        break;
      case 'prisPremiumLugar':
        pris = this.skjema.value.prisPremiumLugar;
        break;
      case 'antallDagerToVei':
        pris = this.skjema.value.prisPremiumLugar;
        break;
      default:
        break;
    }
    if (typeof pris !== 'number') {
      const arr = pris.split('');
      if (arr[0] === '0' && arr.length > 1) {
        return 'Ugyldig tall'
      }
    }
    return;
  }
}
