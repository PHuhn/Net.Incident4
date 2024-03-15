/*
** ===========================================================================
** A collection of functions as follows:
** . nsg_isDarkTheme		boolean value is the current theme light or dark
** . nsg_switchTheme		return string of YYYY-MM-DD format of a date
**
** Includes various constants:
** . _nsg_isDarkTheme		boolean value
*/
let _nsg_isDarkTheme = false;
/**
** Change the 'data-bs-theme' value in the index.html
** to different light or dark theme.
*/
function nsg_switchTheme() {
	const _colorTheme = document.getElementById('body-color-theme');
	if (_colorTheme !== null) {
		const _theme = !_nsg_isDarkTheme ? 'dark' : 'light';
		_colorTheme.setAttribute('data-bs-theme', _theme);
		console.log(`nsg_switchTheme: New color theme: ${_theme}`);
		_nsg_isDarkTheme = !_nsg_isDarkTheme;
		const _icon = document.getElementById('switch-theme-icon');
		if (_icon) {
			_icon.className = _theme === 'dark' ? 'fa fa-moon' : 'fa fa-sun';
			return 0;
		}
		console.warn(`nsg_switchTheme: Failed to find: 'switch-theme-icon'`);
		return 1;
	}
	console.warn(`nsg_switchTheme: Failed to find: 'body-color-theme'`);
	return 2;
}
//
// ===========================================================================
