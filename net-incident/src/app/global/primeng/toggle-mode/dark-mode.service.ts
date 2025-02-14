// ===========================================================================
// file: theme.service.ts
// After: https://github.com/yigitfindikli/primeng-dynamic-theming
// and Showcase in 
import { Inject, Injectable, Renderer2, RendererFactory2  } from '@angular/core';
import { DOCUMENT } from '@angular/common';
//
import { ConsoleLogService } from '../../console-log/console-log.service';
//
@Injectable({
	providedIn: 'root',
})
export class DarkModeService {
	//
	codeName: string = 'theme-service';
	private _renderer: Renderer2;
	private _primeDark: string = 'p-dark';
	private _colorSchemeDataTheme: string = 'data-color-scheme';
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
	** to different light or dark theme.  Additionally, set 'data-color-scheme' to
	** the theme color, so one can set extra CSS values beyond the PrimeNG theme.
	** @param isDark boolean
	*/
	switchTheme(isDark: boolean): number {
		const _codeMethod: string = `${this.codeName}.switchTheme`;
		if ( isDark) {
			this._document.documentElement.classList.add( this._primeDark );
		} else {
			this._document.documentElement.classList.remove( this._primeDark );
		}
		// additionally set 'data-color-scheme' to the theme color
		const _theme = isDark ? 'dark' : 'light';
		this._isDarkTheme = isDark;
		const _documentBody = this._document.body;
		if( _documentBody !== null ) {
			this._renderer.setAttribute( _documentBody, this._colorSchemeDataTheme, _theme );
			this._console.Information( `${_codeMethod}: New color theme: ${_theme}` );
			return 0;
		}
		this._console.Warning( `${_codeMethod}: Failed to find: 'body' tag` );
		return 1;
	}
	//
}
// ===========================================================================
