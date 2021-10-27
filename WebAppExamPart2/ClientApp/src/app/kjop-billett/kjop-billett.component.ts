import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Rute } from '../ruter/Rute';
type Strekning = {
  strekningId: string;
  fra: string;
  til: string;
}

@Component({
  selector: 'app-kjop-billett',
  templateUrl: './kjop-billett.component.html',
  styleUrls: ['./kjop-billett.component.css']
})
export class KjopBillettComponent implements OnInit {
  laster: boolean;
  skjema: FormGroup;
  lugarSkjema: FormGroup;
  kundeSkjema: FormGroup;
  kredittSkjema: FormGroup;
  visBillettSkjema: boolean = true;
  visKundeSkjema: boolean = false;
  visKredittSkjema: boolean = false;
  visRuteUtvalg: boolean = false;
  visLugarUtvalg: boolean = false;
  alleStrekninger: Strekning[];
  fraDestinasjoner: string[]
  gyldigTilDestinasjoner: string[];
  alleRuter: Rute[];
  funnetRuter: any[];
  valgtRute: any;
  dagsDato: string;
  billett: any = {};
  billettPris: number = 0;

  validering = {
    reiseMalFra: [
      null, Validators.compose([Validators.required])
    ],
    reiseMalTil: [
      null, Validators.compose([Validators.required])
    ],
    reiseDato: [
    ],
    antallVoksen: [
    ],
    antallBarn: [
    ],
    billettType: [],
  }

  lugarValidering = {
    lugar: [null]
  }

  kundeValidering = {
    id: [""],
    fornavn: [
      "", Validators.pattern("[a-zA-ZæøåÆØÅ\.\ \-]{2,40}")
    ],
    etternavn: [
      "", Validators.pattern("[a-zA-ZæøåÆØÅ\.\ \-]{2,40}")
    ],
    telefonnr: [
      "",  Validators.pattern("[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{2,7}$")
    ],
    epost: [
      "", Validators.pattern("[A-Za-z0-9]{1}[A-Za-z0-9_.-]*@{1}[A-Za-z0-9_.-]{1,}\.[A-Za-z0-9]{1,}$")
    ],
    adresse: [
      null, Validators.pattern("[a-zA-ZæøåÆØÅ.\-]+[a-zA-ZæøåÆØÅ0-9\ \_.]*[0-9]*")
    ],
    postnr: [
      null, Validators.pattern("[0-9]{4}")
    ],
    poststed: [
      null, Validators.pattern("[a-zA-ZæøåÆØÅ\ \.\-]{2,40}")
    ]
  }

  kredittValidering = {
    kortholdersnavn: [
      "", Validators.pattern("^[a-zA-ZæøåÆØÅ\.\ \-]{2,40}$")
    ],
    kortnr: [
      "", Validators.pattern("^[0-9]{16}$")
    ],
    cardVerificationCode: [
      "",  Validators.pattern("^[0-9]{3}$")
    ],
    utlopsAar: [
      "", Validators.pattern("[0-9]{2}")
    ],
    utlopsMaaned: [
      null, Validators.pattern("[0-9]{2}")
    ],

  }


  constructor(private _http: HttpClient, private router: Router, private fb: FormBuilder) {
    this.skjema = fb.group(this.validering);
    this.lugarSkjema = fb.group(this.lugarValidering);
    this.kundeSkjema = fb.group(this.kundeValidering);
    this.kredittSkjema = fb.group(this.kredittValidering);
  }

  ngOnInit() {
    this.hentAlleStrekninger()
    this.hentAlleRuter()
    this.dagsDato = this.getCurrentDateString();
    this.skjema.patchValue({
      reiseDato: this.dagsDato,
      antallVoksen: 1,
      antallBarn: 0,
      billettType: 'En vei'
    })
  }

  hentAlleStrekninger() {
    this.laster = true;
    this._http.get<any>("api/Rute/hentAlleStrekninger")
      .subscribe(strekninger => {
        console.log(strekninger)
        this.alleStrekninger = strekninger;
        let fra = [];
        this.alleStrekninger.forEach(strekning => {
          if (!fra.includes(strekning.fra)) {
            fra.push(strekning.fra)
          }
          this.fraDestinasjoner = fra;
        })
      },
        error => {
          console.log(error)
        });
    this.laster = false;
  };

  hentAlleRuter() {
    this._http.get<any>("api/Rute/")
      .subscribe(ruter => {
        this.alleRuter = ruter;
      },
        error => {
          console.log(error)
        });
  };

  finneRuter() {
    this.billett.fra = this.skjema.value.reiseMalFra;
    this.billett.til = this.skjema.value.reiseMalTil;
    this.billett.dato = this.skjema.value.reiseDato;
    this.billett.antallVoksen = this.skjema.value.antallVoksen;
    this.billett.antallBarn = this.skjema.value.antallBarn;
    this.billett.billettType = this.skjema.value.billettType;
    console.log(this.billett.fra + " " + this.billett.til)
    console.log(this.billett)

    const filteredRuter = [];
    this.alleRuter.forEach(rute => {
      if (rute.ruteFra === this.billett.fra && rute.ruteTil === this.billett.til) {
        filteredRuter.push(rute)
      }
    })
    this.funnetRuter = filteredRuter;
    this.oppdaterPrisUtenLugar()
    this.opdaterAnkomstDato()
  }

  vedFraChange() {
    this.hideRuteOgLugarUtvalg()
    let strekningerTil: string[] = [];
    this.alleStrekninger.forEach(strekning => {
      if (strekning.fra == this.skjema.value.reiseMalFra && !strekningerTil.includes(strekning.til)) {
        strekningerTil.push(strekning.til)
      }
    })
    this.gyldigTilDestinasjoner = strekningerTil;
    this.skjema.patchValue({
      reiseMalTil: this.gyldigTilDestinasjoner[0]
    })
    this.vedSokClick()
  }

  vedTilChange() {
    this.hideRuteOgLugarUtvalg()
  }

  vedBillettTypeChange() {
    this.hideRuteOgLugarUtvalg()
  }

  vedSokClick() {
    this.finneRuter()
    this.visRuteUtvalg = true;
    this.billettPris = 0;
  }

  vedFunnetRuteClick(rute) {
    this.billettPris = 0;
    this.valgtRute = rute;
    console.log(rute)
    this.lugarSkjema.patchValue({
      lugar: 'Standard'
    })
    this.visLugarUtvalg = true;
    this.billettPris += rute.totalPris;
    
    this.billettPris += this.lugarSkjema.value.lugar === 'Standard' ? this.valgtRute.prisStandardLugar : this.valgtRute.prisPremiumLugar
  }

  vedBilettNeste() {
    this.visBillettSkjema = false;
    this.visRuteUtvalg = false;
    this.visLugarUtvalg = false;
    this.visKundeSkjema = true;
    // this.hideRuteOgLugarUtvalg()
  }

  vedKundeNeste() {
    this.visKundeSkjema = false;
    this.visKredittSkjema = true;
  }

  vedKundeTilbake() {
    this.visBillettSkjema = true;
    this.visRuteUtvalg = true;
    this.visLugarUtvalg = true;
    this.visKundeSkjema = false;
  }

  vedLagre() {
    console.log("lagre ordre")
  }

  vedKredittTilbake() {
    this.visKredittSkjema = false;
    this.visKundeSkjema = true;
  }

  hideRuteOgLugarUtvalg() {
    this.visRuteUtvalg = false;
    this.visLugarUtvalg = false;
    this.vedSokClick()
  }

  
  oppdaterPrisUtenLugar() {
    this.funnetRuter.forEach(rute => {
      rute.totalPris = this.getPrisUtenLugar(rute)
    })
  }

  oppdaterPrisMedLugar() {
    this.billettPris += this.lugarSkjema.value.lugar === 'Standard' ? this.valgtRute.prisStandardLugar : this.valgtRute.prisPremiumLugar
    this.billettPris -= this.lugarSkjema.value.lugar === 'Standard' ? this.valgtRute.prisPremiumLugar : this.valgtRute.prisStandardLugar
  }
  
  opdaterAnkomstDato() {
    this.funnetRuter.forEach(rute => {
      const antallDager = this.billett.billettType === 'En vei' ? rute.antallDagerEnVei : rute.antallDagerToVei
      rute.ankomst = this.getAnkomstDato(this.billett.dato, antallDager)
    })
  }
  
  
  getPrisUtenLugar(rute) {
    let totalPris = 0;
    let veiPris = 0;
    if (this.billett.billettType === 'En vei') {
      veiPris += this.billett.antallVoksen * rute.prisEnvei;
      if (this.billett.antallBarn > 0) {
        let rabattBarn = parseInt(rute.prisRabattBarn)
        console.log(rabattBarn)
        veiPris += (this.billett.antallBarn * rute.prisEnvei) - ((this.billett.antallBarn * rute.prisEnvei)*(rabattBarn/100)) ;
      }
    } else {
      veiPris += this.billett.antallVoksen * rute.prisToVei;
      if (this.billett.antallBarn > 0) {
        let rabattBarn = parseInt(rute.prisRabattBarn)
        veiPris += (this.billett.antallBarn * rute.prisToVei) - ((this.billett.antallBarn * rute.prisToVei)*(rabattBarn/100)) ;
      }
    }
    totalPris += veiPris;
    return totalPris;
  }

  getCurrentDateString() {
    const currentDate: Date = new Date();
    const month = (currentDate.getMonth()) + 1;
    const date = currentDate.getFullYear() + "-" + month.toString().padStart(2, '0') + "-" + currentDate.getDate().toString().padStart(2, '0');
    return date;
  }
  
  getDate(date: Date) {
    const month = (date.getMonth()) + 1;
    const result = date.getFullYear() + "-" + month.toString().padStart(2, '0') + "-" + date.getDate().toString().padStart(2, '0');
    return result;

  }

  getAnkomstDato(avgang, antallDager) {
    let date = new Date(avgang)
    date.setDate(date.getDate() + antallDager);
    this.billett.ankomst = this.getDate(date);
  }
}