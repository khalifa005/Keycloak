import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductsComponent } from './products/products.component';
import { AuthGuard } from 'src/auth/guard/auth-guard';
import { GroupManagementComponent } from 'src/auth/group-management/group-management.component';
import { UserComponent } from 'src/auth/user/user.component';
import { CamundaPermissionComponent } from 'src/auth/camunda-permission/camunda-permission.component';

const routes: Routes = [
  // { path: 'products', component: ProductsComponent },
  { path: 'products', component: ProductsComponent , canActivate: [AuthGuard]},
  { path: 'auth/:id', component: GroupManagementComponent },
  { path: 'users', component:UserComponent },
  { path: 'camunda', component:CamundaPermissionComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
