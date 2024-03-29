// ===========================================================================
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
//
import { AboutComponent } from './public/about/about.component';
import { ContactComponent } from './public/contact/contact.component';
import { HelpComponent } from './public/help/help.component';
import { RegisterComponent } from './public/register/register.component';
//
const routes: Routes = [
	{ path: 'about', component: AboutComponent },
	{ path: 'contacts', component: ContactComponent },
	{ path: 'register', component: RegisterComponent },
	{ path: 'help', component: HelpComponent },
	{ path: '', redirectTo: '', pathMatch: 'full' },
	{ path: '**', redirectTo: '', pathMatch: 'full' }
];
//
@NgModule({
	imports: [RouterModule.forRoot(routes, {})],
	exports: [RouterModule]
})
export class AppRoutingModule { }
// ===========================================================================
