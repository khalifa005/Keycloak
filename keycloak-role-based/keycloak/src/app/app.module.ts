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
import { GroupManagementComponent } from 'src/auth/group-management/group-management.component';
import { TreeviewModule } from '@treeview/ngx-treeview';
import { TableModule } from 'ngx-easy-table';
import { UserComponent } from 'src/auth/user/user.component';
import { CamundaPermissionComponent } from 'src/auth/camunda-permission/camunda-permission.component';

@NgModule({
  declarations: [
    AppComponent,
      ProductsComponent,
      GroupManagementComponent,
      UserComponent,
      CamundaPermissionComponent
   ],
  imports: [
    BrowserModule,
    HttpClientModule,
    KeycloakAngularModule,
    AppRoutingModule,
    NbThemeModule.forRoot(),
    NgxPermissionsModule.forRoot(),
    TreeviewModule.forRoot(),
    TableModule

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
