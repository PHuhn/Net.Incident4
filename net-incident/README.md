# Net-Incident

This project was generated with [Angular CLI](https://github.com/angular/angular-cli).  Net-Incident was first built as an Angular 4 application and is currently built with Angular 21.

## About Net-Incident

Given classified incident logs (like SQL injection, XSS or PHP vulnerabilities), this application groups the incidents by an IP address.  Then the application looks up the ISP abuse email address, ISP name and NIC (Network Information Center) via whois command on the back-end server. The application can generate an email message via the incident type template.  If mail is configured in the back-end server, it will send the message to the ISP’s abuse email address.

## Net-Incident Documentation

* [UI Documentation](https://github.com/PHuhn/net-incident/wiki/UI-Help)

* [UI Installation](https://github.com/PHuhn/net-incident/wiki/Installation-of-Angular-net-incident)

* [Manual UI Testing](https://github.com/PHuhn/net-incident/wiki/Testing-Angular-net-incident-application)

## Construction

### PrimeNG library

Net-Inchdent is an Angular CLI application that uses the following PrimeFaces PrimeNG library components:
* p-table,
* p-ComfirmDialog,
* p-dialog (window/popup),
* pButton (directive),
* p-menubar (menu),
*	p-progressSpinner,
*	p-card,
* p-select.

### Application components structure

The app-component is the conventional root component. The component structure is as follows:

* app-component
  * app-alerts
  * router-outlet (app-routing.module)
  * p-confirmDialog
  * app-header
    * p-menubar
      * app-about
      * app-contact
      * app-help
  * app-login
    * app-server-selection-window (p-dialog)
  * app-incident-grid (p-table)
    * app-server-selection-window (p-dialog)
    * app-incident-detail-window (p-dialog)
    * app-incident-note-grid (p-table)
      * app-incident-note-detail-window (p-dialog)
    * app-networklog-grid (p-table)

### Base Classes

Good design practices inherit the components from an application specific class.

I have three different base classes as follows:

* BaseClass which extends class with the following:
  * Clone( )
  * toString( )
* BaseComponent which extends @Component with the following methods:
  *	baseDeleteConfirm<T>( id: T, callBack: DeleteCallback<T>, label: string = '' )
  *	baseErrorHandler( where: string, what: string, error: string )
* BaseSrvcService which extends @Injectable (service) with the following methods:
  *	getModelAll<T>( )
  *	getModelLazy<T>( event: LazyLoadMeta )
  *	getModelSome<T>( param: any )
  * postJsonBody<T>( body: any )
  *	getModelById<T>( id: any )
  *	getText( id: any )
  *	postJsonBody<T>( body: any )
  *	createModel<T>( model: T )
  *	updateModel<T>( id: any, model: T )
  *	deleteModel<T>( id: any )
  *	baseSrvcErrorHandler( error: any )

In addition the application has three custom services:

* alerts.service,
* console-log.service,
* dark-mode.service.

In addition the application has four custom components:

* alerts.component (uses alerts.service),
* filter-summary.component,
* loading-spinner.component,
* toggle-mode.component (uses dark-mode.service).

# Angular CLI Applications

This project was generated with [Angular CLI](https://github.com/angular/angular-cli).

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `-prod` flag for a production build.

## Running unit tests

Run `ng test` or `ng test --code-coverage` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running lint

Run `ng lint` to execute [es-lint](https://github.com/eslint/eslint).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/).
Before running the tests make sure you are serving the app via `ng serve`.

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).
