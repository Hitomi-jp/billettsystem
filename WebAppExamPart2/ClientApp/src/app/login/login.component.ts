import { Component } from "@angular/core";
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
  }*/

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
}
