import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  isLoggedIn: boolean = false;

  constructor() { }

  loggeInn() {
    this.isLoggedIn = true;
  }

  loggeUt() {
    this.isLoggedIn = false;
  }

  getIsLoggedIn() {
    return this.isLoggedIn;
  }
}
