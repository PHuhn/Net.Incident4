// ===========================================================================
// file: theme.service.ts
// After: https://github.com/yigitfindikli/primeng-dynamic-theming
// and Showcase in 
import { Inject, Injectable, Renderer2, RendererFactory2  } from '@angular/core';
import { DOCUMENT } from '@angular/common';
//
import { ConsoleLogService } from '../console-log/console-log.service';
//
@Injectable({
	providedIn: 'root',
})
export class ThemeService {
	//
	codeName: string = 'theme-service';
	private _renderer: Renderer2;
	_folder: string = '/assets/themes';
	_rootTheme: string = 'lara';
	_rootColor: string = 'blue';
	/**
	** get the current value of _isDarkTheme
	*/
	_isDarkTheme: boolean = false;
	get isDarkTheme(): boolean {
		return this._isDarkTheme;
	}
	//
	constructor(
		public _console: ConsoleLogService,
		@Inject(DOCUMENT) private _document: Document,
		rendererFactory: RendererFactory2 ) {
			this._renderer = rendererFactory.createRenderer(null, null);
	}
	/**
	** Change the <link id="app-theme" rel="stylesheet" ... in the index.html
	** to different light or dark theme.  Additionally, set 'data-bs-theme' to
	** the theme color, so one can set extra CSS values beyond the PrimeNG theme.
	** @param isDark boolean
	*/
	switchTheme(isDark: boolean): number {
		const _codeMethod: string = `${this.codeName}.switchTheme`;
		const _themeLink = this._document.getElementById('app-theme') as HTMLLinkElement;
		if (_themeLink) {
			// change the <link id="app-theme" rel="stylesheet" ... 
			const _theme = isDark ? 'dark' : 'light';
			const _link = `${this._folder}/${this._rootTheme}-${_theme}-${this._rootColor}/theme.css`;
			_themeLink.href = _link;
			this._isDarkTheme = isDark;
			// additionally set 'data-bs-theme' to the theme color
			const _colorTheme = this._document.getElementById('body-color-theme');
			if( _colorTheme !== null ) {
				this._renderer.setAttribute( _colorTheme, 'data-bs-theme', _theme );
				this._console.Information( `${_codeMethod}: New color theme: ${_theme}` );
				return 0;
			}
			this._console.Warning( `${_codeMethod}: Failed to find: 'body-color-theme'` );
			return 1;
		}
		this._console.Warning( `${_codeMethod}: Failed to find: 'app-theme'` );
		return 2;
	}
	//
}
// ===========================================================================
