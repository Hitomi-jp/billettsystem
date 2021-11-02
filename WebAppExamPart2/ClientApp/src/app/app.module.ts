import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { LoginComponent } from './login/login.component';
import { KjopComponent } from './kjop/kjop.component';
import { EndreComponent } from './endre/endre.component';
import { KvitteringComponent } from './kvittering/kvittering.component';
import { KredittComponent } from './kreditt/kreditt.component';
import { HomeComponent } from './home/home.component';
import { AppRoutingModule } from './app-routing.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material.module';
import { RuterComponent } from './ruter/ruter.component';
import { KjopBillettComponent } from './kjop-billett/kjop-billett.component';
import { LogoutComponent } from './logout_will_Delete/logout.component';
import { Modal } from './modal/modal';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    LoginComponent,
    KjopComponent,
    EndreComponent,
    HomeComponent,
    KvitteringComponent,
    KredittComponent,
    FetchDataComponent,
    RuterComponent,
    KjopBillettComponent,
    LogoutComponent,
    Modal
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule,
    NgbModule,
    BrowserAnimationsModule,
    MaterialModule
  ],
  providers: [],
  bootstrap: [AppComponent],
  entryComponents: [Modal] // merk!  
})
export class AppModule { }
