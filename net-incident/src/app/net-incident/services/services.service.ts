// ===========================================================================
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
//
import { Observable } from 'rxjs';
//
import { environment } from '../../../environments/environment';
import { BaseSrvcService } from '../../common/base-srvc/base-srvc.service';
import { ConsoleLogService } from '../../global/console-log/console-log.service';
//
@Injectable( { providedIn: 'root' } )
export class ServicesService extends BaseSrvcService {
	/* --------------------------------------------------------------------
	** Local variables
	*/
	/**
	** Service constructor, inject http service.
	*/
	constructor(
		protected _http: HttpClient,
		protected _console: ConsoleLogService ) {
			super( _http, _console,
				environment.base_Url + 'services', 'Services' );
			this.codeName = 'services-service';
	}
	/**
	** Get ping for ip address
	** @param ipAddress 
	** @returns string/text
	*/
	getPing( ipAddress: string ): Observable<string | never> {
		return this.getText( {service: 'ping', ipaddress: ipAddress} );
	}
	/**
	** Get whois for ip address
	** @param ipAddress 
	** @returns string/text
	*/
	getWhoIs( ipAddress: string ): Observable<string | never> {
		return this.getText( {service: 'whois', ipaddress: ipAddress} );
	}
	//
}
// ===========================================================================
