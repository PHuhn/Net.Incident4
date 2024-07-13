import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
//
import { environment } from '../../../environments/environment';
import { Message } from '../../global/alerts/message';
import { AlertLevel } from '../../global/alerts/alert-level.enum';
import { SelectItemClass } from '../../global/select-item-class';
import { IIncident } from '../incident';
import { INetworkLog } from '../network-log';
import { BaseSrvcService } from '../../global/base-srvc/base-srvc.service';
import { ConsoleLogService } from '../../global/console-log/console-log.service';
//
@Injectable( { providedIn: 'root' } )
export class NetworkIncidentService extends BaseSrvcService {
	//
	codeName = 'network-incident-service';
	/**
	** Service constructor, inject http service.
	*/
	constructor(
		protected _http: HttpClient,
		protected _console: ConsoleLogService ) {
			super( _http, _console );
			this.baseUrl = environment.base_Url + 'NetworkIncidents';
			this.codeName = 'NetworkIncidents';
	}
	/**
	** Class validation rules.
	*/
	validateIncident( model: IIncident, add: boolean ): Message[] {
		const errMsgs: Message[] = [];
		//
		if( model.IncidentId === undefined || model.IncidentId === null ) {
			errMsgs.push( new Message( 'IncidentId-1', AlertLevel.Warning, `'Incident Id' is required.` ) );
		}
		if( model.ServerId === undefined || model.ServerId === null ) {
			errMsgs.push( new Message( 'ServerId-1', AlertLevel.Warning, `'Server Id' is required.` ) );
		}
		if( model.ServerId > 2147483647 ) {
			errMsgs.push( new Message( 'ServerId-2', AlertLevel.Warning, `'Server Id' is too large, over: 2147483647` ) );
		}
		if( model.IPAddress.length === 0 || model.IPAddress === undefined ) {
			errMsgs.push( new Message( 'IPAddress-1', AlertLevel.Warning, `'IP Address' is required.` ) );
		}
		if( model.IPAddress.length > 50 ) {
			errMsgs.push( new Message( 'IPAddress-2', AlertLevel.Warning, `'IP Address' max length of 50.` ) );
		}
		if( model.NIC.length === 0 || model.NIC === undefined ) {
			errMsgs.push( new Message( 'NIC-1', AlertLevel.Warning, `'NIC' is required.` ) );
		}
		if( model.NIC.length > 16 ) {
			errMsgs.push( new Message( 'NIC-2', AlertLevel.Warning, `'NIC' max length of 50.` ) );
		}
		if( model.NetworkName.length > 255 ) {
			errMsgs.push( new Message( 'NetworkName-2', AlertLevel.Warning, `'Network Name' max length of 255.` ) );
		}
		if( model.AbuseEmailAddress.length > 255 ) {
			errMsgs.push( new Message( 'AbuseEmailAddress-2', AlertLevel.Warning, `'Abuse Email Address' max length of 255.` ) );
		}
		if( model.ISPTicketNumber.length > 50 ) {
			errMsgs.push( new Message( 'ISPTicketNumber-2', AlertLevel.Warning, `'ISP Ticket Number' max length of 50.` ) );
		}
		if( model.Mailed === undefined || model.Mailed === null ) {
			errMsgs.push( new Message( 'Mailed-1', AlertLevel.Warning, `'Mailed' is required.` ) );
		}
		if( model.Closed === undefined || model.Closed === null ) {
			errMsgs.push( new Message( 'Closed-1', AlertLevel.Warning, `'Closed' is required.` ) );
		}
		if( model.Special === undefined || model.Special === null ) {
			errMsgs.push( new Message( 'Special-1', AlertLevel.Warning, `'Special' is required.` ) );
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
			errMsgs.push( new Message( 'NetworkLogId-1', AlertLevel.Warning, `'Network Log Id' is required.` ) );
		}
		if( model.ServerId === undefined || model.ServerId === null ) {
			errMsgs.push( new Message( 'ServerId-1', AlertLevel.Warning, `'Server Id' is required.` ) );
		}
		if( model.ServerId > 2147483647 ) {
			errMsgs.push( new Message( 'ServerId-2', AlertLevel.Warning, `'Server Id' is too large, over: 2147483647` ) );
		}
		if( model.IPAddress.length === 0 || model.IPAddress === undefined ) {
			errMsgs.push( new Message( 'IPAddress-1', AlertLevel.Warning, `'IP Address' is required.` ) );
		}
		if( model.IPAddress.length > 50 ) {
			errMsgs.push( new Message( 'IPAddress-2', AlertLevel.Warning, `'IP Address' max length of 50.` ) );
		}
		if( model.NetworkLogDate === undefined || model.NetworkLogDate === null ) {
			errMsgs.push( new Message( 'NetworkLogDate-1', AlertLevel.Warning, `'Network Log Date' is required.` ) );
		}
		if( model.Log.length === 0 || model.Log === undefined ) {
			errMsgs.push( new Message( 'Log-1', AlertLevel.Warning, `'Log' is required.` ) );
		}
		if( model.IncidentTypeId === undefined || model.IncidentTypeId === null ) {
			errMsgs.push( new Message( 'IncidentTypeId-1', AlertLevel.Warning, `'Log Type Id' is required.` ) );
		} else {
			const type = incidentTypes.find( (el) => el.value === model.IncidentTypeId );
			if( type === undefined ) {
				errMsgs.push( new Message( 'IncidentTypeId-2', AlertLevel.Warning, `'Log Type' is not found.` ) );
			}
		}
		//
		return errMsgs;
	}
	/**
	** Class validation rules.
	*/
	validateNetworkLogs( errMsgs: Message[], model: INetworkLog[] ): number {
		//
		const cnt: number = model.reduce( (count, el) => {
			return count + (el.Selected === true ? 1 : 0); }, 0 );
		if( cnt === 0 ) {
			errMsgs.push( new Message( 'NetworkLog-1', AlertLevel.Warning, `'Network Log' at least one needs to be selected.` ) );
		}
		return cnt;
		//
	}
	//
}
// ===========================================================================
