// ===========================================================================
// File: ibase-srvc.ts
import { HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
//
import { LazyLoadMeta } from 'primeng/api';
//
import { ID } from '../global';
import { ILazyResults } from './ilazy-results'
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
	baseParamStringify( param: ID ): string;
	/**
	** Read (get) all models.
	*/
	getModelAll<T>( ): Observable<T[] | never>;
	/**
	** Read (get) lazy loading page of models using primeng
	*/
	getModelLazy<T>( event: LazyLoadMeta ): Observable<ILazyResults<T> | never>;
	/**
	** Read (get) some models.
	** @param param needs to be an object much like the compound key in getModelById
	*/
	getModelSome<T>( param: ID ): Observable<T[] | never>;
	/**
	** Read (get) model with id.
	** @param id needs to be a primative data-type (url param /) or object (url param ?),
	** object would be used for a compound key
	*/
	getModelById<T>( id: ID ): Observable<T | HttpErrorResponse | undefined>;
	/**
	** Get text service.
	** @param id global ID type
	** @returns text/string
	*/
	getText( id: ID ): Observable<string | never>;
	/**
	** Create (post) model.
	** @param model object of class T
	*/
	createModel<T>( model: T ): Observable<T | never>;
	/**
	** Update (put) model.
	** @param id needs to start with either / or ?
	*/
	updateModel<T>( id: ID, model: T ): Observable<T | never>;
	/**
	** Delete (delete) model with id.
	** @param id needs to start with either / or ?
	** @param model object of class T
	*/
	deleteModel<T>( id: ID ): Observable<T>;
	/**
	** Handle an error from this service.
	** 1) Log a console error log,
	** 2) Throw an exception up the chain of execution.
	*/
	baseSrvcErrorHandler( error: HttpErrorResponse | string ): Observable<never>;
	//
}
// ===========================================================================
