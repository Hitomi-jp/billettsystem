/*import { Component } from "@angular/core";
import { NgModule } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AppComponent } from '../app.component';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { Bruker } from "../Bruker";
import { Router } from "@angular/router";


@NgModule({
  declarations: [
    LoginComponent,
    ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    ReactiveFormsModule,
    HttpClientModule,
    HttpClient

  ],
  providers: [],
  bootstrap: [AppComponent]
})
@Component({

  templateUrl: "login.component.html"
})
export class LoginComponent {
  Skjema: FormGroup;
 // public laster: string;
  /*validering = {
    brukernavn:["", Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
    ],
    password: ["", Validators.compose([Validators.pattern('(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&]).{8,}')])
    ]
  }

  constructor(private fb: FormBuilder, private _http: HttpClient, private router: Router) {
    //this.Skjema = fb.group(this.validering);
      this.Skjema = fb.group({
        brukernavn: ["", Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])],
        passord: ["", Validators.compose([Validators.required, Validators.pattern("[a-zA-Z0-9]{6,}")])]
    
    });
} 

  loggIn() {
    const bruker = new Bruker();
    bruker.brukernavn = this.Skjema.value.brukernavn;
    bruker.passord = this.Skjema.value.passord;
    this._http.post<boolean>("api/kunde/loggInn", bruker)
        .subscribe( retur => {
          if (retur) {
            this.router.navigate(['/billett'])
          }
        },
        error => alert("Feil brukernavn eller passord"),
        () => console.log("ferdig get-/bruker")
      );
  }

  onSubmit() {
    console.log("Modellbasert skjema submitted:");
    console.log(this.Skjema);
    console.log(this.Skjema.value.brukernavn);
    console.log(this.Skjema.touched);
  }
}*/

import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { NgModule } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AppComponent } from '../app.component';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { Bruker } from "../Bruker";
import { Router } from "@angular/router";
import { MaterialModule } from '../material.module'

@NgModule({
  declarations: [
    LoginComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    ReactiveFormsModule,
    HttpClientModule,
    HttpClient,
    MaterialModule

  ],
  providers: [],
  bootstrap: [AppComponent]
})
@Component({
  templateUrl: "login.component.html"
})

export class LoginComponent {
  formGroup: FormGroup;
  constructor(private formBuilder: FormBuilder, private _http: HttpClient, private router: Router) { }

  ngOnInit() {
    this.createForm();
  }

  createForm() {
    this.formGroup = this.formBuilder.group({
      'brukernavn': ['', Validators.compose([Validators.required, Validators.pattern('[a-zA-ZøæåØÆÅ0-9_-]{3,}')])],
      'passord': ['', Validators.compose([Validators.pattern('[a-zA-Z0-9]{6,}')])],
    });
  }

  getError(el) {
    switch (el) {
      case 'brukernavn':
        if (this.formGroup.get('brukernavn').hasError('required')) {
          return 'Må Skrive Brukernavn';
        } else if (this.formGroup.get('brukernavn').hasError('pattern')) {
          return 'Minst 3 ';
        }
        break;
      case 'passord':
        if (this.formGroup.get('passord').hasError('required')) {
          return 'Må Skrive Passord';
        } else if (this.formGroup.get('passord').hasError('pattern')) {
          return 'Minst 6';
        }
        break;
      default:
        return '';
    }
  }

  onSubmit() {
    const bruker = new Bruker();
    bruker.brukernavn = this.formGroup.value.brukernavn;
    bruker.passord = this.formGroup.value.passord;
    this._http.post<boolean>("api/kunde/loggInn", bruker)
      .subscribe(retur => {
        if (retur) {
          console.log(retur);
          this.router.navigate(['/billett']);
          console.log("Ferdig get-brukren")
        }

        else {
          console.log(retur);
          alert("Feil brukernavn eller passord");
          console.log("Feil brukernavn eller passord");
          this.createForm();
        }
      });
  };
}

