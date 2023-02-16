// ===========================================================================
// File: base-srvc.service.ts
// Base generic service
// https://nichola.dev/generic-approach-to-consume-rest-api/
// https://betterprogramming.pub/a-generic-http-service-approach-for-angular-applications-a7bd8ff6a068
// https://dev.to/this-is-angular/generic-approach-to-consume-rest-api-in-angular-4poj
//
import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
//
import { Observable, throwError, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { LazyLoadEvent } from 'primeng/api';
//
import { ConsoleLogService } from '../../global/console-log/console-log.service';
import { ILazyResults, IBaseSrvc } from './ibase-srvc';
import { BaseSrvc } from './base-srvc';
//
@Injectable({ providedIn: 'root' })
export class BaseSrvcService extends BaseSrvc implements IBaseSrvc {
	/*
	** Base service class that does 6 things:
	** * codeName string variable,
	** * injects console log service,
	*/
	constructor(
		// Angular http client
		protected _http: HttpClient,
		// to write console logs condition on environment log-level
		_console: ConsoleLogService,
		@Inject('url') _url: string,
		@Inject('name') _className: string ) {
			super( _console, _url, _className );
	}
	/*
	** --------------------------------------------------------------------
	** CRUD (Create/Read/Update/Delete)
	*/
	/**
	** Read (get) all models.
	*/
	getModelAll<T>( ): Observable<T[] | never> {
		const urlPath: string = `${this.baseUrl}/`;
		this._console.Information(
			`${this.codeName}.get${this.baseClassName}All: ${urlPath}` );
		return this._http.get<Array<T>>( urlPath )
			.pipe( catchError( this.baseSrvcErrorHandler.bind( this ) ) );
	}
	/**
	** Read (get) lazy loading page of models.
	** post (get) passing body with JSON options.  Can be used
	** with PrimeNG's lazy load table and with authenication
	** @param body 
	** @returns T
	*/
	getModelLazy<T>( event: LazyLoadEvent ): Observable<T | never> {
		// {"first":0,"rows":3,"filters":{"ServerId":{"value":1,"matchMode":"equals"}}}
		return this.postJsonBody<T>( event );
	}
	/**
	** post (get) passing body with JSON options.  Can be used with
	** 1) PrimeNG's lazy load table
	** 2) authenication
	** @param body 
	** @returns T
	*/
	postJsonBody<T>( body: any ): Observable<T | never> {
		const urlPath: string = this.baseUrl + '/';
		const headerJSON = new HttpHeaders().set('content-type', 'application/json' as const);
		const options = { headers: headerJSON };
		this._console.Information(
			`${this.codeName}.get${this.baseClassName}Lazy: ${urlPath}` );
		return this._http.post<T>( urlPath, body, options )
			.pipe( catchError( this.baseSrvcErrorHandler.bind(this) ) );
	}
	/**
	** Read (get) some models.
	** @param param object or data type
	*/
	getModelSome<T>( param: any ): Observable<T[] | never> {
		const srvcParam: string = this.baseParamStringify(param);
		const urlPath: string = `${this.baseUrl}${srvcParam}`;
		this._console.Information(
			`${this.codeName}.get${this.baseClassName}Some: ${urlPath}` );
		return this._http.get<Array<T>>( urlPath )
			.pipe( catchError( this.baseSrvcErrorHandler.bind( this ) ) );
	}
	/**
	** Read (get) model with id.
	** @param id object or data type
	*/
	getModelById<T>( id: any ): Observable<T | never> {
		const srvcParam: string = this.baseParamStringify(id);
		const urlPath: string = `${this.baseUrl}${srvcParam}`;
		this._console.Information(
			`${this.codeName}.get${this.baseClassName}: ${urlPath}` );
		return this._http.get<T>( urlPath )
			.pipe( catchError( this.baseSrvcErrorHandler.bind( this ) ) );
	}
	/**
	** Get text service.
	** @param id 
	** @returns text/string
	*/
	getText( id: any ): Observable<string | never> {
		const srvcParam: string = this.baseParamStringify(id);
		const urlPath: string = `${this.baseUrl}${srvcParam}`;
		this._console.Information(
			`${this.codeName}.getText: ${urlPath}` );
		return this._http.get( urlPath, {responseType: 'text'} )
			.pipe( catchError( this.baseSrvcErrorHandler.bind(this) ) );
	}
	/**
	** Create (post) model.
	** @param model object of class T
	*/
	createModel<T>( model: T ): Observable<T | never> {
		const urlPath: string = `${this.baseUrl}/`;
		this._console.Information(
			`${this.codeName}.create${this.baseClassName}: ${ JSON.stringify( model ) }` );
		return this._http.post<T>( urlPath, model )
			.pipe( catchError( this.baseSrvcErrorHandler.bind( this ) ) );
	}
	/**
	** Update (put) model.
	** @param id object or data type describing the primary key
	** @param model object of class T
	*/
	updateModel<T>( id: any, model: T ): Observable<T | never> {
		const srvcParam: string = this.baseParamStringify(id);
		const urlPath: string = `${this.baseUrl}${srvcParam}`;
		this._console.Information(
			`${this.codeName}.update${this.baseClassName}: ${urlPath}` );
		return this._http.put<T>( urlPath, model )
			.pipe( catchError( this.baseSrvcErrorHandler.bind( this ) ) );
	}
	/**
	** Delete (delete) model with id.
	** @param id object or data type describing the primary key
	*/
	deleteModel<T>( id: any ): Observable<T | never> {
		const srvcParam: string = this.baseParamStringify(id);
		const urlPath: string = `${this.baseUrl}${srvcParam}`;
		this._console.Information(
			`${this.codeName}.delete${this.baseClassName}: ${urlPath}` );
		return this._http.delete<T>( urlPath )
			.pipe( catchError( this.baseSrvcErrorHandler.bind( this ) ) );
	}
	//
}
// ===========================================================================
