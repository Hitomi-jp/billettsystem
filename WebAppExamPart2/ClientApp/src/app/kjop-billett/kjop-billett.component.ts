import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Rute } from '../ruter/Rute';
type Strekning = {
  id: string;
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
  alleStrekninger: Strekning[];
  visForm: boolean = true;
  dagsDato: string;

  validering = {
    reiseMal: [
      null, Validators.compose([Validators.required])
    ],
    reiseDato: [
      null, Validators.compose([Validators.required])
    ]
  }

  constructor(private _http: HttpClient, private router: Router, private fb: FormBuilder) {
    this.skjema = fb.group(this.validering);
  }

  ngOnInit() {
    this.hentAlleStrekninger()
    this.dagsDato = this.getCurrentDateString();
  }

  hentAlleStrekninger() {
    this.laster = true;
    this._http.get<any>("api/Rute/hentAlleStrekninger")
      .subscribe(strekninger => {
        console.log(strekninger)
        this.alleStrekninger = strekninger;
      },
        error => {
          if (error.status === 401) {
            this.router.navigate(['/login'])
          }
        });
    this.laster = false;
  };

  getCurrentDateString() {
    const currentDate: Date = new Date();
    const month = (currentDate.getMonth()) + 1;
    const date = currentDate.getFullYear() + "-" + month.toString().padStart(2, '0') + "-" + currentDate.getDate().toString().padStart(2, '0');
    return date;
  }
}
