import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductsComponent } from './products/products.component';
import { AuthGuard } from 'src/auth/guard/auth-guard';

const routes: Routes = [
  // { path: 'products', component: ProductsComponent },
  { path: 'products', component: ProductsComponent , canActivate: [AuthGuard]},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
