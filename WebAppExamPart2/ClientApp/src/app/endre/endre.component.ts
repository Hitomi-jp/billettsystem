import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Rute } from '../ruter/Rute';
import { Billett } from '../Billett';
import { Kunde } from '../Kunde';
import { Kreditt } from '../Kreditt';
type Strekning = {
  strekningId: string;
  fra: string;
  til: string;
}

@Component({
  templateUrl: './endre.component.html',
  styleUrls:['../kjop-billett/kjop-billett.component.css']
})
export class EndreComponent {
  laster: boolean;
  skjema: FormGroup;
  lugarSkjema: FormGroup;
  kundeSkjema: FormGroup;
  kredittSkjema: FormGroup;
  visBillettSkjema: boolean = true;
  visKundeSkjema: boolean = true;
  visRuteUtvalg: boolean = true;
  visLugarUtvalg: boolean = false;
  gyldigEpostKarakter: boolean = true;
  alleStrekninger: Strekning[];
  fraDestinasjoner: string[]
  gyldigTilDestinasjoner: string[];
  alleRuter: Rute[];
  funnetRuter: any[];
  valgtRute: Rute;
  dagsDato: string;
  billettPris: number = 0;
  billett: Billett;
  kunde: Kunde;

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
      "", Validators.pattern(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/)
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


  constructor(private _http: HttpClient, private router: Router, private fb: FormBuilder, private activatedRouter: ActivatedRoute) {
    this.skjema = fb.group(this.validering);
    this.kundeSkjema = fb.group(this.kundeValidering)
    this.lugarSkjema = fb.group(this.lugarValidering);
  }

  ngOnInit() {
    this.hentAlleStrekninger()
    this.dagsDato = this.getCurrentDateString();
  }

  patchValues() {
    this.skjema.patchValue({
      reiseMalFra: this.billett.destinationFrom,
      reiseMalTil: this.billett.destinationTo,
      reiseDato: this.billett.departureDato,
      antallVoksen: this.billett.antallAdult,
      antallBarn: this.billett.antallChild,
      billettType: this.billett.ticketType
    })
    this.vedFraChange()
    this.skjema.patchValue({
      reiseMalTil: this.billett.destinationTo
    })

    this.lugarSkjema.patchValue({
      lugar: this.billett.lugarType
    })
    this.billettPris = this.billett.pris;
    this.valgtRute = this.finnValgtRute()[0];
    this.visLugarUtvalg = true;

    this.kundeSkjema.patchValue({
      fornavn: this.kunde.fornavn,
      etternavn: this.kunde.etternavn,
      telefonnr: this.kunde.telfonnr,
      epost: this.kunde.epost,
      adresse: this.kunde.adresse,
      postnr: this.kunde.postnr,
      poststed: this.kunde.poststed
    })
  }

  finnValgtRute() {
    return this.alleRuter.filter(rute => rute.id === this.billett.ruteId)
  }

  hentAlleStrekninger() {
    this.laster = true;
    this._http.get<any>("api/Rute/hentAlleStrekninger")
      .subscribe(strekninger => {
        this.alleStrekninger = strekninger;
        let fra = [];
        this.alleStrekninger.forEach(strekning => {
          if (!fra.includes(strekning.fra)) {
            fra.push(strekning.fra)
          }
          this.fraDestinasjoner = fra;
        })
        this.hentAlleRuter()
      },
        error => {
          console.log(error)
        });
  };

  hentAlleRuter() {
    this._http.get<any>("api/Rute/")
      .subscribe(ruter => {
        this.alleRuter = ruter;
        this.activatedRouter.paramMap.subscribe(params => {
          const id = parseInt(params.get('id'))
          this.hentEnBillett(id);
        })
      },
        error => {
          console.log(error)
        });
  };

  hentEnBillett(billettId: number) {
    this._http.get<Billett>("api/billett/hentEnBillett?billettId=" +  billettId) 
      .subscribe(hentetBillett => {
        this.billett = hentetBillett;
        this.hentEnKunde(hentetBillett.kundeId)
      })
  }

  hentEnKunde(kundeId: number) {
    this._http.get<Kunde>("api/kunde/hentEnKunde?kundeId=" +  kundeId) 
      .subscribe(hentetKunde => {
        this.kunde = hentetKunde;
        this.patchValues()
      })
      this.laster = false;
  }

  endreBillett() {
    this._http.put<any>("api/billett/", this.billett)
    .subscribe(endreOk => {
      this.endreKunde()
    },
      error => {
        console.log(error)
      }); 
  }

  endreKunde() {
    this._http.put<any>("api/kunde/", this.kunde)
    .subscribe(endreOk => {
      this.router.navigate(['/billett'])
    },
      error => {
        console.log(error)
      }); 
  }

  vedAntallVoksenPlus() {
    this.skjema.patchValue({
      antallVoksen: this.skjema.value.antallVoksen + 1
    })
    this.hideRuteOgLugarUtvalg()
  }

  vedAntallVoksenMinus() {
    if (this.skjema.value.antallVoksen === 1) {
      return;
    }
    this.skjema.patchValue({
      antallVoksen: this.skjema.value.antallVoksen - 1
    })
    this.hideRuteOgLugarUtvalg()
  }

  vedAntallBarnPlus() {
    this.skjema.patchValue({
      antallBarn: this.skjema.value.antallBarn + 1
    })
    this.hideRuteOgLugarUtvalg()
  }

  vedAntallBarnMinus() {
    if (this.skjema.value.antallBarn < 1) {
      return;
    }
    this.skjema.patchValue({
      antallBarn: this.skjema.value.antallBarn - 1
    })
    this.hideRuteOgLugarUtvalg()
  }

  vedEndre() {
    this.billett.ruteId = this.valgtRute.id;
    this.billett.destinationFrom = this.skjema.value.reiseMalFra;
    this.billett.destinationTo = this.skjema.value.reiseMalTil;
    this.billett.ticketType = this.skjema.value.billettType;
    this.billett.lugarType = this.lugarSkjema.value.lugar;
    this.billett.departureDato = this.skjema.value.reiseDato;
    this.billett.antallAdult = this.skjema.value.antallVoksen;
    this.billett.antallChild = this.skjema.value.antallBarn;
    this.billett.pris = this.billettPris;

    this.kunde.fornavn = this.kundeSkjema.value.fornavn;
    this.kunde.etternavn = this.kundeSkjema.value.etternavn;
    this.kunde.telfonnr = this.kundeSkjema.value.telefonnr;
    this.kunde.epost = this.kundeSkjema.value.epost;
    this.kunde.adresse = this.kundeSkjema.value.adresse;
    this.kunde.postnr = this.kundeSkjema.value.postnr;
    this.kunde.poststed = this.kundeSkjema.value.poststed;

    this.endreBillett()

  }

  finneRuter() {
    this.billett.destinationFrom = this.skjema.value.reiseMalFra;
    this.billett.destinationTo = this.skjema.value.reiseMalTil;
    this.billett.departureDato = this.skjema.value.reiseDato;
    this.billett.antallAdult = this.skjema.value.antallVoksen;
    this.billett.antallChild = this.skjema.value.antallBarn;
    this.billett.ticketType = this.skjema.value.billettType;

    const filteredRuter = [];
    this.alleRuter.forEach(rute => {
      if (rute.ruteFra === this.billett.destinationFrom && rute.ruteTil === this.billett.destinationTo) {
        filteredRuter.push(rute)
      }
    })
    this.funnetRuter = filteredRuter;
    this.oppdaterPrisUtenLugar()
    this.opdaterAnkomstDato()
  }

  vedFraEndre() {
    this.valgtRute = null;
    this.filterStrekninger(true)
    this.vedFraChange()
  }

  vedFraChange() {
    this.visRuteUtvalg = false;
    this.visLugarUtvalg = false;
    this.oppdaterRuter()

    this.filterStrekninger(false)
    this.oppdaterRuter()
  }
  
  filterStrekninger(endre) {
    let strekningerTil: string[] = [];
    this.alleStrekninger.forEach(strekning => {
      if (strekning.fra == this.skjema.value.reiseMalFra && !strekningerTil.includes(strekning.til)) {
        strekningerTil.push(strekning.til)
      }
    })
    if (endre) {
      this.skjema.patchValue({
        reiseMalTil: strekningerTil[0]
      })
    }
    this.gyldigTilDestinasjoner = strekningerTil;
  }

  vedTilChange() {
    this.hideRuteOgLugarUtvalg()
  }

  vedBillettTypeChange() {
    this.valgtRute = null;
    this.billett.ruteId = null;
    this.hideRuteOgLugarUtvalg()
  }

  oppdaterRuter() {
    this.finneRuter()
    this.visRuteUtvalg = true;
    this.billettPris = 0;
  }

  vedFunnetRuteClick(rute) {
    this.billettPris = 0;
    this.valgtRute = rute;
   
    this.visLugarUtvalg = true;
    this.billettPris += rute.totalPris;
    
    this.billettPris += this.lugarSkjema.value.lugar === 'Standard' ? this.valgtRute.prisStandardLugar : this.valgtRute.prisPremiumLugar

    this.billett.ruteId = rute.id; 
  }

  vedEndreTilbake() {
    this.router.navigate(['/billett'])
  }

  hideRuteOgLugarUtvalg() {
    if (!this.billett.destinationFrom && !this.billett.destinationTo) {
      return;
    }
    this.billett.ruteId = null;
    this.valgtRute = null;
    this.visRuteUtvalg = false;
    this.visLugarUtvalg = false;
    // this.removeSelectedClass()
    this.oppdaterRuter()
  }
  
  oppdaterPrisUtenLugar() {
    this.funnetRuter.forEach(rute => {
      rute.totalPris = this.getPrisUtenLugar(rute)
    })
  }

  oppdaterPrisMedLugar() {
    this.billett.lugarType = this.lugarSkjema.value.lugar;
    this.billettPris += this.lugarSkjema.value.lugar === 'Standard' ? this.valgtRute.prisStandardLugar : this.valgtRute.prisPremiumLugar
    this.billettPris -= this.lugarSkjema.value.lugar === 'Standard' ? this.valgtRute.prisPremiumLugar : this.valgtRute.prisStandardLugar
  }
  
  opdaterAnkomstDato() {
    this.funnetRuter.forEach(rute => {
      const antallDager = this.billett.ticketType === 'En vei' ? rute.antallDagerEnVei : rute.antallDagerToVei
      rute.ankomst = this.getAnkomstDato(this.billett.departureDato, antallDager)
    })
  }
  
  getPrisUtenLugar(rute) {
    let totalPris = 0;
    let veiPris = 0;
    if (this.billett.ticketType === 'En vei') {
      veiPris += this.billett.antallAdult * rute.prisEnvei;
      if (this.billett.antallChild > 0) {
        let rabattBarn = parseInt(rute.prisRabattBarn)
        veiPris += (this.billett.antallChild * rute.prisEnvei) - ((this.billett.antallChild * rute.prisEnvei)*(rabattBarn/100)) ;
      }
    } else {
      veiPris += this.billett.antallAdult * rute.prisToVei;
      if (this.billett.antallChild > 0) {
        let rabattBarn = parseInt(rute.prisRabattBarn)
        veiPris += (this.billett.antallChild * rute.prisToVei) - ((this.billett.antallChild * rute.prisToVei)*(rabattBarn/100)) ;
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
    this.billett.returnDato = this.getDate(date);
  }

  removeSelectedClass() {
    const ruteUtvalg = document.getElementsByClassName("selectedRute");
    for(let i = 0; i < ruteUtvalg.length; i++) {
      ruteUtvalg[i].classList.remove("selectedRute")
      i--;
    }

  }
  valideringEpostKarakter() {
      const epost = this.kundeSkjema.value.epost;
      if (/[øæåØÆÅ]/.test(epost)) {
        console.log("yess")
        this.gyldigEpostKarakter = false;
        return 'Ugyldig karakter'
      }else {

        this.gyldigEpostKarakter = true;
        console.log("no")
        return ''
      }
    }
}
