import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest, HttpHandler, HttpEvent, HttpResponse, HttpErrorResponse } from '@angular/common/http';
//
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
//
import { environment } from '../../../environments/environment';
import { Message } from '../../global/alerts/message';
import { SelectItemClass } from '../../global/select-item-class';
import { NetworkIncident } from '../network-incident';
import { NetworkIncidentSave } from '../network-incident-save';
import { IIncident, Incident } from '../incident';
import { INetworkLog, NetworkLog } from '../network-log';
import { BaseSrvcService } from '../../common/base-srvc/base-srvc.service';
import { ConsoleLogService } from '../../global/console-log/console-log.service';
//
@Injectable( { providedIn: 'root' } )
export class NetworkIncidentService extends BaseSrvcService {
	//
	codeName = 'network-incident-service';
	url: string;
	/**
	** Service constructor, inject http service.
	*/
	constructor(
		protected _http: HttpClient,
		protected _console: ConsoleLogService ) {
			super( _http, _console,
				environment.base_Url + 'NetworkIncidents', 'NetworkIncidents' );
			this.url = environment.base_Url + 'NetworkIncidents';
	}
	/**
	** Class validation rules.
	*/
	validateIncident( model: IIncident, add: boolean ): Message[] {
		const errMsgs: Message[] = [];
		//
		if( model.IncidentId === undefined || model.IncidentId === null ) {
			errMsgs.push( new Message( 'IncidentId-1', `'Incident Id' is required.` ) );
		}
		if( model.ServerId === undefined || model.ServerId === null ) {
			errMsgs.push( new Message( 'ServerId-1', `'Server Id' is required.` ) );
		}
		if( model.ServerId > 2147483647 ) {
			errMsgs.push( new Message( 'ServerId-2', `'Server Id' is too large, over: 2147483647` ) );
		}
		if( model.IPAddress.length === 0 || model.IPAddress === undefined ) {
			errMsgs.push( new Message( 'IPAddress-1', `'IP Address' is required.` ) );
		}
		if( model.IPAddress.length > 50 ) {
			errMsgs.push( new Message( 'IPAddress-2', `'IP Address' max length of 50.` ) );
		}
		if( model.NIC.length === 0 || model.NIC === undefined ) {
			errMsgs.push( new Message( 'NIC-1', `'NIC' is required.` ) );
		}
		if( model.NIC.length > 16 ) {
			errMsgs.push( new Message( 'NIC-2', `'NIC' max length of 50.` ) );
		}
		if( model.NetworkName.length > 255 ) {
			errMsgs.push( new Message( 'NetworkName-2', `'Network Name' max length of 255.` ) );
		}
		if( model.AbuseEmailAddress.length > 255 ) {
			errMsgs.push( new Message( 'AbuseEmailAddress-2', `'Abuse Email Address' max length of 255.` ) );
		}
		if( model.ISPTicketNumber.length > 50 ) {
			errMsgs.push( new Message( 'ISPTicketNumber-2', `'ISP Ticket Number' max length of 50.` ) );
		}
		if( model.Mailed === undefined || model.Mailed === null ) {
			errMsgs.push( new Message( 'Mailed-1', `'Mailed' is required.` ) );
		}
		if( model.Closed === undefined || model.Closed === null ) {
			errMsgs.push( new Message( 'Closed-1', `'Closed' is required.` ) );
		}
		if( model.Special === undefined || model.Special === null ) {
			errMsgs.push( new Message( 'Special-1', `'Special' is required.` ) );
		}
		//
		return errMsgs;
	}
	/**
	** Class validation rules.
	*/
	validateNetworkLog( model: INetworkLog, add: boolean, incidentTypes: SelectItemClass[] ): Message[] {
		const errMsgs: Message[] = [];
		//
		if( model.NetworkLogId === undefined || model.NetworkLogId === null ) {
			errMsgs.push( new Message( 'NetworkLogId-1', `'Network Log Id' is required.` ) );
		}
		if( model.ServerId === undefined || model.ServerId === null ) {
			errMsgs.push( new Message( 'ServerId-1', `'Server Id' is required.` ) );
		}
		if( model.ServerId > 2147483647 ) {
			errMsgs.push( new Message( 'ServerId-2', `'Server Id' is too large, over: 2147483647` ) );
		}
		if( model.IPAddress.length === 0 || model.IPAddress === undefined ) {
			errMsgs.push( new Message( 'IPAddress-1', `'IP Address' is required.` ) );
		}
		if( model.IPAddress.length > 50 ) {
			errMsgs.push( new Message( 'IPAddress-2', `'IP Address' max length of 50.` ) );
		}
		if( model.NetworkLogDate === undefined || model.NetworkLogDate === null ) {
			errMsgs.push( new Message( 'NetworkLogDate-1', `'Network Log Date' is required.` ) );
		}
		if( model.Log.length === 0 || model.Log === undefined ) {
			errMsgs.push( new Message( 'Log-1', `'Log' is required.` ) );
		}
		if( model.IncidentTypeId === undefined || model.IncidentTypeId === null ) {
			errMsgs.push( new Message( 'IncidentTypeId-1', `'Log Type Id' is required.` ) );
		} else {
			const type = incidentTypes.find( (el) => el.value === model.IncidentTypeId );
			if( type === undefined ) {
				errMsgs.push( new Message( 'IncidentTypeId-2', `'Log Type' is not found.` ) );
			}
		}
		//
		return errMsgs;
	}
	/**
	** Class validation rules.
	*/
	validateNetworkLogs( errMsgs: Message[], model: INetworkLog[] ): void {
		//
		const cnt: number = model.reduce( (count, el) => {
			return count + (el.Selected === true ? 1 : 0); }, 0 );
		if( cnt === 0 ) {
			errMsgs.push( new Message( 'NetworkLog-1', `'Network Log' at least one needs to be selected.` ) );
		}
		//
	}
	//
}
// ===========================================================================
