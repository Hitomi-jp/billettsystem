import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  isLoggedIn: boolean = false;

  constructor(private _http: HttpClient) { }

  loggeInn() {
    this.isLoggedIn = true;
  }

  loggeUt() {
    this.isLoggedIn = false;
  }

  getIsLoggedIn() {
    return this.isLoggedIn;
  }

  sjekkIsLoggetInn() {
    this._http.get("api/logInOut/sjekkIsLoggetInn")
      .subscribe(response => {
        if (response === true) {
          this.isLoggedIn = true;
        } else {
          this.isLoggedIn = false;
        }
      })
  }
}
