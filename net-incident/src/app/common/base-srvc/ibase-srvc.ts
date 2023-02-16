// ===========================================================================
// File: ibase-srvc.ts
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
//
import { LazyLoadEvent } from 'primeng/api';
/**
** lazy loading return results
*/
export interface ILazyResults {
	/**
	** results is an array of generic objects
	*/
	results: Array<any>;
	/**
	** totalRecords is the total number of records
	*/
	totalRecords: number;
	/**
	** the calling lazy-load-event
	*/
	loadEvent: string;
	/**
	** message if any from the service
	*/
	message: string;
}
/**
** base http services
*/
export interface IBaseSrvc {
	/**
	** Properties
	*/
	baseUrl: string;
	//
	baseClassName: string;
    //
	baseTotalRecords: number;
	/**
	** Get a date-time string YYYY-MM-DDTHH:MM:SS.
	** @param dt Date type
	** @returns string
	*/
	baseLocaleDateTimeString( dt: Date ): string;
	/**
	** Parse object or data type to create request param,
	** returns string such as:
	**  /1
	**  /cat
	**  ?CompanyId=1&IncidentTypeId=3
	** types: Date/string/number/bigint/boolean/symbol/undefined/object/function
	** @param param 
	** @returns string
	*/
	baseParamStringify( param: any ): string;
	/**
	** Read (get) all models.
	*/
	getModelAll<T>( ): Observable<T[] | never>;
	/**
	** Read (get) lazy loading page of models using primeng
	*/
	getModelLazy<T>( event: LazyLoadEvent ): Observable<ILazyResults | never>;
	/**
	** Read (get) some models.
	** @param param needs to be an object much like the compound key in getModelById
	*/
	getModelSome<T>( param: any ): Observable<T[] | never>;
	/**
	** Read (get) model with id.
	** @param id needs to be a primative data-type (url param /) or object (url param ?),
	** object would be used for a compound key
	*/
	getModelById<T>( id: any ): Observable<T | never>;
	/**
	** Get text service.
	** @param id 
	** @returns text/string
	*/
	getText( id: any ): Observable<string | never>;
	/**
	** post (get) passing body with JSON options.  Can be used
	** with PrimeNG's lazy load table and with authenication
	** @param body 
	** @returns T
	*/
	postJsonBody<T>( body: any ): Observable<T | never>;
	/**
	** Create (post) model.
	** @param model object of class T
	*/
	createModel<T>( model: T ): Observable<T | never>;
	/**
	** Update (put) model.
	** @param id needs to start with either / or ?
	*/
	updateModel<T>( id: any, model: T ): Observable<T | never>;
	/**
	** Delete (delete) model with id.
	** @param id needs to start with either / or ?
	** @param model object of class T
	*/
	deleteModel<T>( id: any ): Observable<T>;
	/**
	** Handle an error from this service.
	** 1) Log a console error log,
	** 2) Throw an exception up the chain of execution.
	*/
	baseSrvcErrorHandler( error: any ): Observable<never>;
	//
}
// ===========================================================================
