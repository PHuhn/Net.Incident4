/*
** ===========================================================================
** A collection of functions as follows:
** . nsg_toggleThemeIcon	match the icon to the selected light or dark theme
** . nsg_switchTheme		return string of YYYY-MM-DD format of a date
** . nsg_changeTheme        change the theme [data-bs-theme] value
** . nsg_toggleTheme        toggle the current theme
** . nsg_getTheme           returns current theme in local storage
** . init                   initialization steps
*/
$(function () {
    /**
    ** Change the font awesome from sun to moon and back to sun to match the
    ** current theme.
    ** @param {any/string} theme
    ** @returns
    */
    function nsg_toggleThemeIcon(theme) {
        const _methodName = 'nsg_toggleThemeIcon';
        const _icon = document.getElementById('switch-theme-icon');
        if (_icon) {
            _icon.className = theme === 'light' ? 'fa fa-sun' : 'fa fa-moon';
            return 0;
        }
        console.warn(`${_methodName}: Failed to find: 'switch-theme-icon'`);
    }
    /**
    ** 
    ** @param {any/string} theme
    */
    function nsg_changeTheme(theme) {
        window.localStorage.setItem('theme', theme);
        document.getElementsByTagName('body')[0].setAttribute('data-bs-theme', theme);
        nsg_toggleThemeIcon(theme);
    }
    /**
    ** Toggle the theme from light to dark and back again.
    */
    function nsg_toggleTheme() {
        nsg_getTheme() == 'light' ? nsg_changeTheme('dark') : nsg_changeTheme('light');
    }
    /**
    ** Get the current theme from local storage
    ** @returns
    */

    function nsg_getTheme() {
        return window.localStorage.getItem('theme') ?? 'dark';
    }
    /**
    ** Force the initialization of the current theme with the following steps.
    */
    function init() {
        let theme = nsg_getTheme();
        if (theme) {
            nsg_changeTheme(theme);
        }
    }
    /**
    ** Apply the click event to the toggle button.
    */
    document.getElementById('switch-theme-button').addEventListener(
        'click', () => {
            nsg_toggleTheme();
        }
    );
    /**
    ** Force the initialization of the current theme.
    */
    init();
});
// ===========================================================================