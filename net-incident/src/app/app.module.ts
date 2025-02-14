// File: app.module.ts
import { NgModule } from '@angular/core';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
//
import { providePrimeNG } from 'primeng/config';
//
import { APP_PROVIDERS } from './APP.PROVIDERS';
import { APP_COMPONENTS } from './APP.COMPONENTS';
import { APP_MODULE_PRIMENG, APP_PRIMENG_PROVIDERS } from './APP.MODULE-PRIMENG';
import { APP_GLOBAL_COMPONENTS, APP_GLOBAL_PROVIDERS } from './global/APP.GLOBAL';
import { AppRoutingModule } from './app-routing.module';
import { GlobalModule } from './global/global.module';
import { AppComponent } from './app.component';
//
@NgModule({
	declarations: [
		AppComponent,
		APP_GLOBAL_COMPONENTS,
		APP_COMPONENTS,
	],
	imports: [
		AppRoutingModule,
		GlobalModule,
		APP_MODULE_PRIMENG
	],
	providers: [
		APP_GLOBAL_PROVIDERS,
		APP_PRIMENG_PROVIDERS,
		APP_PROVIDERS,
		provideAnimationsAsync( ),
		providePrimeNG( ) 
	],
	bootstrap: [AppComponent]
})
export class AppModule { }
