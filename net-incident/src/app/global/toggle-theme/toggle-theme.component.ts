// ===========================================================================
// File: toggle-theme.component.ts
import { Component } from '@angular/core';
import { ThemeService } from './theme.service';
//
@Component({
	selector: 'app-toggle-theme',
	template: `<button type='button'
	class='nsg-toggle-theme-button'
	(click)='changeTheme(!isDarkMode)'>
	<i class='pi ' [ngClass]="{'pi-moon': isDarkMode, 'pi-sun': !isDarkMode}"></i>
</button>`,
	styleUrl: './toggle-theme.component.css'
})
export class ToggleThemeComponent {
	//
	constructor(
		private themeService: ThemeService
	) { }
	//
	get isDarkMode() {
		return this.themeService.isDarkTheme;
	}
	//
	changeTheme(theme: boolean): number {
		return this.themeService.switchTheme(theme);
	}
	//
}
// ===========================================================================
