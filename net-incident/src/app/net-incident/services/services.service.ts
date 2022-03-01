// ===========================================================================
import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest, HttpErrorResponse } from '@angular/common/http';
//
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
//
import { BaseServService } from '../../common/base-serv/base-serv.service';
import { ConsoleLogService } from '../../global/console-log/console-log.service';
//
@Injectable( { providedIn: 'root' } )
export class ServicesService extends BaseServService {
	// --------------------------------------------------------------------
	// Local variables
	//
	url: string;
	/**
	** Service constructor, inject http service.
	*/
	constructor(
		protected http: HttpClient,
		protected _console: ConsoleLogService ) {
			super( http, _console );
			this.url = this.base_url;
			this.codeName = 'services-service';
	}
	/**
	** Get ping for ip address
	*/
	getPing( ipAddress: string ): Observable<string> {
		return this.getService( `${this.url}services/ping/${ipAddress}` );
	}
	/**
	** Get ping for ip address
	*/
	getWhoIs( ipAddress: string ): Observable<string> {
		return this.getService( `${this.url}services/whois/${ipAddress}` );
	}
	/**
	** Get service for ip address
	*/
	getService( urlPath: string ): Observable<string> {
		this._console.Information(
			`${this.codeName}.getService: ${urlPath}` );
		return this.http.get( urlPath, {responseType: 'text'} )
			.pipe( catchError( this.baseServiceError.bind(this) ) );
	}
	//
}
// ===========================================================================
