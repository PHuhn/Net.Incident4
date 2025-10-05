// ===========================================================================
// File: toggle-theme.component.ts
import { Component } from '@angular/core';
import { DarkModeService } from './dark-mode.service';
//
@Component({
    selector: 'app-toggle-mode',
    template: `<button pButton
	class='nsg-toggle-theme-button'
	severity='secondary'
	(click)='changeTheme(!isDarkMode)'>
	<i class='pi ' [ngClass]="{'pi-moon': isDarkMode, 'pi-sun': !isDarkMode}"></i>
</button>`,
	styleUrl: './toggle-mode.component.css',
	standalone: false
})
export class ToggleModeComponent {
	//
	constructor(
		private _darkModeService: DarkModeService
	) { }
	//
	get isDarkMode( ) {
		return this._darkModeService.isDarkTheme;
	}
	//
	changeTheme( mode: boolean ): number {
		return this._darkModeService.switchTheme( mode );
	}
	//
}
// ===========================================================================
