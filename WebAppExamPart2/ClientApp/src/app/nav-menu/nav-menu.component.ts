import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

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

  constructor(private _http: HttpClient, private router: Router) { }
  LoggUt() {
    this._http.get<any>("api/rute/loggut")
      .subscribe(retur => {
        this.router.navigate(['/login']);
      });
  }

}

