// ===========================================================================
// File: Incident.service.ts
// Service for Incident class
//
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
//
import { environment } from '../../../environments/environment';
import { IIncident, Incident } from '../incident';
import { ConsoleLogService } from '../../global/console-log/console-log.service';
import { BaseSrvcService } from '../../global/base-srvc/base-srvc.service';
//
@Injectable( { providedIn: 'root' } )
export class IncidentService extends BaseSrvcService {
	//
	// Service constructor, inject http service.
	//
	constructor(
		protected http: HttpClient,
		protected _console: ConsoleLogService ) {
			super( http, _console );
			this.baseUrl = environment.base_Url + 'Incidents';
			this.codeName = 'incident-service';
			this.baseClassName = 'Incident';
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
