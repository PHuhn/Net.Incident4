// ===========================================================================
// File: base-serv.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
//
import { throwError } from 'rxjs';
//
import { environment } from '../../../environments/environment';
import { ConsoleLogService } from '../../global/console-log/console-log.service';
//
//
@Injectable({ providedIn: 'root' })
export class BaseServService {
	codeName: string = 'base-serv.service';
	base_url: string;
	/**
	** Add the generally needed services.
	*/
	constructor(
		// communicate to the web services
		protected _http: HttpClient,
		// to write console logs condition on environment log-level
		protected _console: ConsoleLogService
	) {
		this.base_url = environment.base_Url;
	}
	/**
	** General error handler
	*/
	baseServiceError( error: any ) {
		if ( error instanceof HttpErrorResponse ) {
			this._console.Error(
				`${this.codeName}.baseServiceError: ${ JSON.stringify( error ) }` );
			return throwError( `Code: ${error.status}, Message: ${error.message}` || 'Service error' );
		}
		this._console.Error(
			`${this.codeName}.baseServiceError: ${error.toString()}` );
		return throwError( error.toString() || 'Service error' );
	}
	//
}
// ===========================================================================
