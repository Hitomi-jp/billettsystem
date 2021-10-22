import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { KjopComponent } from './kjop/kjop.component';
import { EndreComponent } from './endre/endre.component';
import { HomeComponent } from './home/home.component';
import { KvitteringComponent } from './kvittering/kvittering.component';
import { KredittComponent } from './kreditt/kreditt.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';

const appRoots: Routes = [
  { path: 'kjop', component: KjopComponent },
  { path: 'endre', component: EndreComponent },
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'billett', component: FetchDataComponent },
  { path: 'kvittering', component: KvitteringComponent },
  { path: 'kreditt', component: KredittComponent },
  { path: '', redirectTo: 'home', pathMatch: 'full' }
]

@NgModule({
  imports: [RouterModule.forRoot(appRoots)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
