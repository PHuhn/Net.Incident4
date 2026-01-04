// ===========================================================================
// File: global.module.ts
import { NgModule } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { withJsonpSupport } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
//
@NgModule({
	imports: [
		CommonModule
	],
	exports : [
		CommonModule,
		FormsModule,
		BrowserModule,
		ReactiveFormsModule
	],
	providers: [
		provideHttpClient(withJsonpSupport())
	]
})
export class GlobalModule { }
// ===========================================================================
