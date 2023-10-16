import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { initKeycloak } from 'src/auth/init-keycloak';
import { KeycloakAngularModule, KeycloakService } from 'keycloak-angular';
import { HttpClientModule } from '@angular/common/http';
import { ProductsComponent } from './products/products.component';
import { NbThemeModule } from '@nebular/theme';
import { NgxPermissionsModule } from 'ngx-permissions';
@NgModule({
  declarations: [
    AppComponent,
      ProductsComponent
   ],
  imports: [
    BrowserModule,
    HttpClientModule,
    KeycloakAngularModule,
    AppRoutingModule,
    NbThemeModule.forRoot(),
    NgxPermissionsModule.forRoot(),

  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: initKeycloak,
      multi: true,
      deps: [KeycloakService]
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
