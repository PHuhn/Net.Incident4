import { Component } from '@angular/core';
import { ThemeService } from './theme.service';
//
@Component({
	selector: 'app-toggle-theme',
	templateUrl: './toggle-theme.component.html',
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
