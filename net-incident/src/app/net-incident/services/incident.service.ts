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
import { ConsoleLogService } from '../../global/console-log/console-log.service';
import { BaseSrvcService } from '../../common/base-srvc/base-srvc.service';
import { ILazyResults } from '../../common/base-srvc/ibase-srvc';
//
@Injectable( { providedIn: 'root' } )
export class IncidentService extends BaseSrvcService {
	//
	// --------------------------------------------------------------------
	// Data declaration.
	//
	codeName: string;
	//
	// Service constructor, inject http service.
	//
	constructor(
		protected http: HttpClient,
		protected _console: ConsoleLogService ) {
			super( http, _console,
				environment.base_Url + 'Incidents', 'Incident' );
			this.codeName = 'incident-service';
	}
	//
	// Single place to create a new Incident.
	//
	emptyIncident( ): IIncident {
		return Incident.empty( );
	}
	//
}
// ===========================================================================
