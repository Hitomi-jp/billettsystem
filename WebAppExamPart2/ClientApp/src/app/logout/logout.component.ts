import { Component, OnInit } from "@angular/core";
import { NgModule } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from '../app.component';



@NgModule({
  declarations: [
    LogoutComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClient

  ],
  providers: [],
  bootstrap: [AppComponent]
})

@Component({
  templateUrl: './logout.component.html'
})

export class LogoutComponent {
  constructor( private _http: HttpClient, private router: Router) { }
  LoggUt() {
    this._http.get<any>("api/rute/loggut")
      .subscribe(retur => {
        this.router.navigate(['/login']);
      });
  }
}
