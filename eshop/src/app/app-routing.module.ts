import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ShopInterfaceComponent } from './pages/shop-interface/shop-interface.component';
import { ReportsComponent } from './pages/reports/reports.component';

const routes: Routes = [
  { path: "reports", component: ReportsComponent },
  { path: "**", component: ShopInterfaceComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
