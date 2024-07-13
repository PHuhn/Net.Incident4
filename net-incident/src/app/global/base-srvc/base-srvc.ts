// ===========================================================================
// File: base-srvc.ts
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
//
import { ConsoleLogService } from '../console-log/console-log.service';
//
export class BaseSrvc {
	/*
	** --------------------------------------------------------------------
	** Data declaration.
	*/
	protected codeName: string = 'base-srvc';
	//
	private _baseUrl: string = '';
	get baseUrl(): string { return this._baseUrl; }
	set baseUrl(value: string) { this._baseUrl = value; }
	//
	private _className: string = '';
	get baseClassName(): string { return this._className; }
	set baseClassName(value: string) { this._className = value; }
    private _totalRecords: number = 0;
	get baseTotalRecords(): number { return this._totalRecords; }
	set baseTotalRecords(value: number) { this._totalRecords = value; }
	/*
	** Base service class that does 6 things:
	** * codeName string variable,
	** * injects console log service,
	*/
	constructor(
		protected _console: ConsoleLogService ) { }
	/**
	** Get a date-time string YYYY-MM-DDTHH:MM:SS.
	** @param dt Date type
	** @returns string
	*/
	baseLocaleDateTimeString( dt: Date ): string {
		const dateParts = dt.toString().split(' ')
		return `${dt.toLocaleDateString('fr-CA')}T${dateParts[4]}`;
	}
	/**
	** Parse object or primative data type to create request param,
	** returns string such as:
	**  /1
	**  /cat
	**  ?CompanyId=1&IncidentTypeId=3
	** Object of {CompanyId:1, IncidentTypeId:3}
	**  will result in string of ?CompanyId=1&IncidentTypeId=3
	** types: Date/string/number/bigint/boolean/symbol/undefined/object/function
	** @param param 
	** @returns string (see above)
	*/
	baseParamStringify( param: string | object | number | bigint | boolean | null ): string {
		let _ret: string = '';
		if( param === null ) {
			return `/`;
		}
		switch (typeof param) {
			case 'undefined':
				_ret = `/`;
				break; 
			case 'object':
				if (param instanceof Date) {
					_ret = `/${this.baseLocaleDateTimeString( param )}`;
				} else {
					let _chr: string = '?';
					for ( const [key, value] of Object.entries(param) ) {
						const tval: string = typeof value;
						if (value instanceof Date) {
							_ret = `${_ret}${_chr}${key}=${this.baseLocaleDateTimeString( value as Date )}`;
						} else {
							// if class has any functions, ignore them...
							if( tval !== 'function' ) {
								_ret = `${_ret}${_chr}${key}=${value}`;
							}
						}
						_chr = '&';
					}
				}
				break;
			case 'string':
				if( param.substring(0,1) === '?' || param.substring(0,1) === '/' ) {
					_ret = param;
				} else {
					_ret = `/${param}`;
				}
				break; 
			default:
				_ret = `/${param}`;
				break; 
		}
		return _ret;
	}
	/**
	** Handle an error from this service.
	** 1) Log a console error log,
	** 2) Throw an exception up the chain of execution.
	*/
	baseSrvcErrorHandler( error: HttpErrorResponse | string ): Observable<never> {
		if ( error instanceof HttpErrorResponse ) {
			this._console.Error(
				`${this.codeName}.baseSrvcErrorHandler: ${JSON.stringify(error)}` );
			const errMsg: string =
				`${(error.statusText ? error.statusText : 'Service error')} (${(error.status ? error.status : 'unk')})`;
			return throwError( ( ) => new Error( errMsg ) );
			// error.statusText || 'Service error' ) );
		}
		this._console.Error(
			`${this.codeName}.baseSrvcErrorHandler: ${error}` );
		return throwError( ( ) => new Error(
			error.toString() || 'Service error' ) );
	}
	//
}
// ===========================================================================
