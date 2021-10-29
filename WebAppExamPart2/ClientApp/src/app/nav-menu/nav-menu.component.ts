import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { LoginService } from 'src/login.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  constructor(private _http: HttpClient, private router: Router, private loggInService: LoginService) { }
  LoggUt() {
    this._http.get<any>("api/loginout/loggut")
      .subscribe(retur => {
        this.loggInService.loggeUt();
        this.router.navigate(['/login']);
      });
  }

}

