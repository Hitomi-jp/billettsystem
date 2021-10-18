import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { KjopComponent } from './kjop/kjop.component';
import { HomeComponent } from './home/home.component';

const appRoots: Routes = [
  { path: 'kjop', component: KjopComponent },
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LoginComponent, },
  { path: '', redirectTo: 'home', pathMatch: 'full' }
]

@NgModule({
  imports: [RouterModule.forRoot(appRoots)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
