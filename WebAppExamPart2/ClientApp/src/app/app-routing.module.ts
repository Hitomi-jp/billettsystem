import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { KjopComponent } from './kjop/kjop.component';
import { EndreComponent } from './endre/endre.component';
import { HomeComponent } from './home/home.component';
import { KvitteringComponent } from './kvittering/kvittering.component';
import { KredittComponent } from './kreditt/kreditt.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { RuterComponent } from './ruter/ruter.component';
import { KjopBillettComponent } from './kjop-billett/kjop-billett.component';
import { LogoutComponent } from './logout/logout.component'

const appRoots: Routes = [
  { path: 'kjop', component: KjopBillettComponent },
  { path: 'endre', component: EndreComponent },
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'billett', component: FetchDataComponent },
  { path: 'kvittering', component: KvitteringComponent },
  { path: 'kreditt', component: KredittComponent },
  { path: 'ruter', component: RuterComponent },
  { path: 'logout', component: LogoutComponent },
  { path: '', redirectTo: 'home', pathMatch: 'full' }
]

@NgModule({
  imports: [RouterModule.forRoot(appRoots)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
