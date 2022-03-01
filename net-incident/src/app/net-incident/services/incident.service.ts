// ===========================================================================
// File: Incident.service.ts
// Service for Incident class
//
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams, HttpErrorResponse } from '@angular/common/http';
//
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { LazyLoadEvent } from 'primeng/api';
//
import { environment } from '../../../environments/environment';
import { Message } from '../../global/alerts/message';
import { IIncident, Incident } from '../incident';
import { IncidentPaginationData } from '../incident-pagination-data';
import { ConsoleLogService } from '../../global/console-log/console-log.service';
import { BaseServService } from '../../common/base-serv/base-serv.service';
//
@Injectable( { providedIn: 'root' } )
export class IncidentService extends BaseServService {
	//
	// --------------------------------------------------------------------
	// Data declaration.
	//
	codeName: string;
	url: string;
	//
	// Service constructor, inject http service.
	//
	constructor(
		protected http: HttpClient,
		protected _console: ConsoleLogService ) {
			super( http, _console );
			this.codeName = 'incident-service';
			this.url = environment.base_Url + 'Incidents';
	}
	//
	// Single place to create a new Incident.
	//
	emptyIncident( ): IIncident {
		return Incident.empty( );
	}
	//
	// CRUD (Create/Read/Update/Delete)
	// Read (get) page of Incidents, that are filtered and sorted.
	//
	getIncidentsLazy( event: LazyLoadEvent ): Observable<IncidentPaginationData> {
		const urlPath: string = this.url + '/';
		const headerJSON = new HttpHeaders().set('content-type', 'application/json' as const);
		const options = { headers: headerJSON };
		this._console.Information(
			`${this.codeName}.getIncidentsLazy: ${urlPath}` );
		return this.http.post<IncidentPaginationData>( urlPath, event, options )
			.pipe( tap( ( response: IncidentPaginationData ) => {
				return response;
			},( err: any ) => {
				this.baseServiceError( err );
			} ) );
	}
	//
	// Delete (delete) Incident with id
	//
	deleteIncident( IncidentId: number ) {
		const urlPath: string = this.url + '/' + String( IncidentId );
		this._console.Information(
			`${this.codeName}.deleteIncident: ${urlPath}` );
		return this.http.delete<IIncident>( urlPath )
			.pipe( catchError( this.baseServiceError.bind(this) ) );
	}
	//
}
// ===========================================================================
