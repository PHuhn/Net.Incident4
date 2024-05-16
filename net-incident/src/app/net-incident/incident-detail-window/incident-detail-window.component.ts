// ===========================================================================
// File: Incident-detail-window.component.ts
import { Component, OnInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';
//
import { Dialog } from 'primeng/dialog';
import { environment } from '../../../environments/environment';
//
import { AlertsService } from '../../global/alerts/alerts.service';
import { ServicesService } from '../services/services.service';
import { NetworkIncidentService } from '../services/network-incident.service';
import { ConsoleLogService } from '../../global/console-log/console-log.service';
import { BaseCompService } from '../../global/base-comp/base-comp.service';
import { BaseComponent } from '../../global/base-comp/base-comp.component';
import { Message } from '../../global/alerts/message';
import { DetailWindowInput } from '../DetailWindowInput';
import { IUser, User } from '../user';
import { IIncident, Incident } from '../incident';
import { SelectItemClass } from '../../global/select-item-class';
import { NetworkIncident } from '../network-incident';
import { NetworkIncidentSave } from '../network-incident-save';
import { IWhoIsAbuse, WhoIsAbuse } from '../whois-abuse';
import { AbuseEmailReport } from '../abuse-email-report';
import { SelectItemExtra } from '../../global/select-item-class';
import { IncidentNote } from '../incident-note';
//
import { NetworkLogGridComponent } from '../network-log-grid/network-log-grid.component';
import { IncidentNoteGridComponent } from '../incident-note-grid/incident-note-grid.component';
//
@Component({
	selector: 'app-incident-detail-window',
	templateUrl: './incident-detail-window.component.html'
})
export class IncidentDetailWindowComponent extends BaseComponent implements OnDestroy {
	//
	// --------------------------------------------------------------------
	// Data declaration.
	//
	private add: boolean = false;
	private disableEdit: boolean = false;
	id: number = -1;
	ip: string = '';
	private serverId: number = -1;
	private displayWinTimeout: any;
	private paramsSubscription: Subscription | undefined;
	private httpSubscription: Subscription | undefined;
	private httpCreateSubscription: Subscription | undefined;
	private httpUpdateSubscription: Subscription | undefined;
	detailWindow: DetailWindowInput;
	networkIncident: NetworkIncident | undefined;
	networkIncidentSave: NetworkIncidentSave | undefined;
	user: User;
	displayWindow: boolean = false;
	// communicate to the AlertComponent
	protected _alerts: AlertsService;
	// to write console logs condition on environment log-level
	protected _console: ConsoleLogService;
	/**
	** --------------------------------------------------------------------
	** Inputs and emitted outputs
	** 	inputs: incident and displayWin
	** 	outputs: emitClose
	** setter/getter for incident & user
	*/
	@Input() set detailWindowInput( detailInput: DetailWindowInput ) {
		this.detailWindow = detailInput;
		if( detailInput === undefined ) {
			this.id = 0;
			return;
		}
		this.disableEdit = false;
		this.user = detailInput.user;
		this.serverId = this.user.Server.ServerId;
		this.id = detailInput.incident.IncidentId;
		this.ip = detailInput.incident.IPAddress;
		this.add = ( detailInput.incident.IncidentId < 1 ? true : false );
		this.networkIncident = undefined;
		this.getNetIncident( this.id, this.serverId );
		this._console.Information(
			`${this.codeName}: Editing: ${this.id}, win: ${this.displayWin}` );
	}
	get detailWindowInput(): DetailWindowInput { return this.detailWindow; }
	// setter/getter for displayWin
	@Input() set displayWin( displayWin: boolean ) {
		if( displayWin === true ) {
			this.displayWinTimeout = setTimeout(() => {
				this._console.Information(
					`${this.codeName}: displayWin: ${displayWin}` );
				// The set displayWindow in getNetIncidents should activate the window
				// this set of displayWindow is the last resort
				this.displayWindow = displayWin;
			},800);
		} else {
			this.displayWindow = displayWin;
		}
	}
	get displayWin(): boolean { return this.displayWindow; }
	//
	@Output() emitClose = new EventEmitter<boolean>();
	windowClose(saved: boolean) {
		if( saved === false ) {
			this.emitClose.emit( saved ); // cancel
			return;
		}
		this.NetIncidentSave( false );
	}
	/**
	** The Mailed checkbox was changed
	** @param event 
	** @returns ture if success, false if error
	*/
	MailedCheckboxEvent( event: Event ): boolean {
		const _mailed: Boolean = (event.target as HTMLInputElement).checked;
		if( _mailed ) {
			const model: Incident = this.networkIncident.incident;
			if( model.IncidentId === undefined || model.IncidentId === null || model.IncidentId === 0 
				|| model.IPAddress.length === 0 || model.IPAddress === undefined
				|| model.NIC.length === 0 || model.NIC === undefined
				|| model.AbuseEmailAddress.length === 0
				|| model.NetworkName.length === 0)  {
				const errMsgs: Message[] = [];
				//
				if( model.IncidentId === undefined || model.IncidentId === null || model.IncidentId === 0  ) {
					errMsgs.push( new Message( 'IncidentId-1', `Incident not saved, 'Id' is zero` ) );
				}
				if( model.IPAddress.length === 0 || model.IPAddress === undefined ) {
					errMsgs.push( new Message( 'IPAddress-1', `'IP Address' is required.` ) );
				}
				if( model.NIC.length === 0 || model.NIC === undefined ) {
					errMsgs.push( new Message( 'NIC-1', `'NIC' not set.` ) );
				}
				if( model.NetworkName.length === 0 ) {
					errMsgs.push( new Message( 'NetworkName-1', `'Network Name' not set.` ) );
				}
				if( model.AbuseEmailAddress.length === 0 ) {
					errMsgs.push( new Message( 'AbuseEmailAddress-1', `'Abuse Email Address'  not set.` ) );
				}
				this._alerts.warningSet( errMsgs );
				return false;
			} else {
				const _noteType: SelectItemExtra = this.networkIncident.noteTypes.find( nt => nt.extra === 'email');
				const _noteTypeId: number = _noteType.value;
				this._alerts.setWhereWhatInfo(this.codeName, `Email note type id: + ${_noteTypeId}` );
				if( this.networkIncident.incidentNotes.filter( _in => _in.NoteTypeId === _noteTypeId ).length === 0 ) {
					// Create the abuse email message
					const abuseReport: AbuseEmailReport = new AbuseEmailReport( this.networkIncident );
					if( abuseReport.IsValid() ) {
						const _id = this.newNoteId();
						const _incidentNote: IncidentNote = new IncidentNote( _id, _noteTypeId,
							_noteType.label, abuseReport.ComposeEmail( ).replace(/\\n/g, '\n'),
							new Date(), true);
						this.networkIncident.incidentNotes = [ ...this.networkIncident.incidentNotes, _incidentNote ];
					} else {
						this._console.Warning( JSON.stringify( abuseReport.errMsgs ) );
						this._alerts.warningSet( abuseReport.errMsgs );
					}
				}
			}
		}
		return true;
	}
	/**
	** Allow save and close of save and stay.
	*/
	NetIncidentSave( stay: boolean ): void {
		//
		this.networkIncidentSave = new NetworkIncidentSave();
		if( this.networkIncident !== undefined ) {
			this.networkIncidentSave.incident = this.networkIncident.incident;
			this.networkIncidentSave.incidentNotes =
				this.networkIncident.incidentNotes.filter( nt => nt.IsChanged === true );
			this.networkIncidentSave.deletedNotes = this.networkIncident.deletedNotes;
			this.networkIncidentSave.networkLogs =
				this.networkIncident.networkLogs.filter( nl => nl.IsChanged === true ||
					nl.Selected === true );
			this.networkIncidentSave.deletedLogs = this.networkIncident.deletedLogs;
			this.networkIncidentSave.user = this.networkIncident.user;
			this.networkIncidentSave.message = this.networkIncident.message;
			if( this.add === false ) {
				this.networkIncident.incident.IncidentId = this.id;
			}
		}
		//
		this._console.Information(
			JSON.stringify( this.networkIncidentSave ) );
		if( this.validate( ) ) {
			if( this.add === true ) {
				this.createItem( stay );
			} else {
				this.updateItem( stay );
			}
		}
		//
	}
	/**
	** Constructor used to inject services.
	*/
	constructor(
		// inject the base components services
		private _baseSrvc: BaseCompService,
		private _netIncident: NetworkIncidentService,
		private _services: ServicesService
	) {
		super( _baseSrvc );
		// get the needed services from the base component
		this._alerts = _baseSrvc._alerts;
		this._console = _baseSrvc._console;
		this.codeName = 'Incident-Detail-Window';
		//
		this.user = User.empty();
		this.detailWindow = new DetailWindowInput( this.user, Incident.empty())
		this.networkIncident = new NetworkIncident( );
		//
	}
	/**
	** Cleanup
	** * Stop interval timers (clearTimeout/clearInterval).
	** * Unsubscribe Observables.
	** * Detach event handlers (addEventListener > removeEventListener).
	** * Free resources that will not be garbage collected automatically.
	** * Unregister all callbacks.
	*/
	ngOnDestroy() {
		if( this.displayWinTimeout ) {
			clearTimeout( this.displayWinTimeout );
		}
		if( this.httpSubscription ) {
			this.httpSubscription.unsubscribe( );
		}
		if( this.httpCreateSubscription ) {
			this.httpCreateSubscription.unsubscribe( );
		}
		if( this.httpUpdateSubscription ) {
			this.httpUpdateSubscription.unsubscribe( );
		}
	}
	/**
	** get the complete requested incident (unit-of-work).
	** incident # is zero then get empty for server #.
	** api/NetworkIncident/1
	** or
	** api/NetworkIncident?action=empty?serverId=1
	** @param incidentId 
	** @param serverId 
	*/
	getNetIncident( incidentId: number, serverId: number ): void {
		let srvcParam: any = '';
		if( incidentId === 0 ) {
			srvcParam = { action: 'empty', serverId: serverId }
		} else {
			srvcParam = incidentId;
		}
		this.httpSubscription =
			this._netIncident.getModelById<NetworkIncidentSave>( srvcParam ).subscribe({
				next: ( netIncidentData: NetworkIncident ) => {
					this.moveNetIncidentDetail( netIncidentData, true );
					// once the data is loaded now display it.
					this.displayWindow = true;
					clearTimeout( this.displayWinTimeout );
				},
				error: (error) => {
					this.baseErrorHandler( this.codeName, `Get Net Incident`, error );
				},
				complete: () => { }
		});
	}
	/**
	** Move detail data to local data.
	*/
	moveNetIncidentDetail( netIncidentData: NetworkIncident, stay: boolean ): void {
		this._console.Information(
			`${this.codeName}.moveNetIncidentDetail, Entering: ${new Date().toISOString()}` );
		if( !stay ) {
			this.displayWin = false;
			this.networkIncident.user = undefined;
			this.emitClose.emit( true );
		} else {
			this._console.Information(
				JSON.stringify( netIncidentData ) );
			this.networkIncident = netIncidentData;
			this.networkIncident.user = this.user;
			if( this.networkIncident !== undefined ) {
				this.id = this.networkIncident.incident.IncidentId;
			}
			if( this.networkIncident.incident.Mailed === true || this.networkIncident.incident.Closed === true ) {
				this.disableEdit = true;
			}
			this.networkIncidentSave = undefined;
		}
		this._console.Information( 
			`${this.codeName}.moveNetIncidentDetail, Exiting: ${new Date().toISOString()}` );
	}
	/**
	** Check against a common set of validation rules.
	*/
	validate( ): boolean {
		if( this.networkIncidentSave === undefined ) {
			const msg = new Message( 'err1', 'Incident undefined.');
			this._console.Warning( `${this.codeName}.validate, ${msg}` );
			this._alerts.warningSet( [msg] );
			return false;
		}
		this.initialize( this.networkIncidentSave.incident );
		const errMsgs: Message[] = this._netIncident.validateIncident(
			this.networkIncidentSave.incident, this.add );
		// need at least one log selected
		this._netIncident.validateNetworkLogs( errMsgs, this.networkIncidentSave.networkLogs );
		this.validateUser( errMsgs, this.networkIncidentSave.user );
		//
		if( errMsgs.length > 0 ) {
			this._console.Warning( `${this.codeName}.validate, ${JSON.stringify( errMsgs )}` );
			this._alerts.warningSet( errMsgs );
			return false;
		}
		return true;
	}
	/**
	** Set any undefined string to empty string
	*/
	initialize( model: IIncident ): void {
		if( model.IPAddress === undefined || model.IPAddress === null ) {
			model.IPAddress = '';
		}
		if( model.NIC === undefined || model.NIC === null ) {
			model.NIC = '';
		}
		if( model.NetworkName === undefined || model.NetworkName === null ) {
			model.NetworkName = '';
		}
		if( model.AbuseEmailAddress === undefined || model.AbuseEmailAddress === null ) {
			model.AbuseEmailAddress = '';
		}
		if( model.ISPTicketNumber === undefined || model.ISPTicketNumber === null ) {
			model.ISPTicketNumber = '';
		}
		if( model.Notes === undefined || model.Notes === null ) {
			model.Notes = '';
		}
	}
	/**
	** Class validation rules.
	*/
	validateUser( errMsgs: Message[], model: IUser ): void {
		//
		if( model.UserName === undefined || model.UserName === '' ) {
			errMsgs.push( new Message( 'UserName-1', `From User, 'User Name' is required.` ) );
		}
		if( model.UserNicName === undefined || model.UserNicName === '' ) {
			errMsgs.push( new Message( 'UserNicName-1', `From User, 'User Nic Name' is required.` ) );
		}
		if( model.Email === undefined || model.Email === '' ) {
			errMsgs.push( new Message( 'Email-1', `From User, 'User Email Address' is required.` ) );
		}
		//
	}
	/**
	** --------------------------------------------------------------------
	** (ipChanged)='ipChanged($event)'
	** get whois data for the ip-address and parse it for:
	** * nic,
	** * network-name,
	** * abuse e-mail address.
	** Update the incident record with this information.  If the abuse
	** e-mail address is not found or invalid then save the whois data to
	** the notes.
	*/
	ipChanged( ipAddress: string ): void {
		this._console.Information(
			`${this.codeName}.ipChanged, IP address: ${ipAddress}` );
		if( this.networkIncident === undefined ) {
			const msg = new Message( 'err1', 'Incident undefined.');
			this._console.Warning( `${this.codeName}.ipChanged, ${msg}` );
			this._alerts.warningSet( [msg] );
			return;
		}
		if( this.networkIncident.incident.IPAddress !== ipAddress ) {
			this.networkIncident.incident.IPAddress = ipAddress;
			this.ip = ipAddress;
			if( ipAddress === '' ) { return; }
			this._console.Verbose(
				`${this.codeName}.ipChanged: calling whois with ${ipAddress}` );
			this._services.getWhoIs( ipAddress ).subscribe({
				next: ( whoisData: string ) => {
					if( whoisData !== '' && this.networkIncident !== undefined ) {
						// instanciate WhoIsAbuse class
						const whois: WhoIsAbuse = new WhoIsAbuse();
						whois.GetWhoIsAbuse( whoisData );
						const cnt: number = this.networkIncident.NICs.reduce( (count, el) => {
							return count + (el.value === whois.nic ? 1 : 0); }, 0 );
						if( cnt > 0 ) {
							this.networkIncident.incident.NIC = whois.nic;
						} else {
							this.networkIncident.incident.NIC = 'other';
						}
						this.networkIncident.incident.AbuseEmailAddress = whois.abuse;
						this.networkIncident.incident.NetworkName = whois.net;
						this._console.Verbose( `${this.codeName}.ipChanged: WhoIs: ${ipAddress}, ${whois.nic}, ${whois.net}, ${whois.abuse}` );
						if( whois.BadAbuseEmail( ) ) {
							const newNote: IncidentNote = new IncidentNote(
								this.newNoteId(),2,'WhoIs',whoisData,new Date( Date.now() ), true );
							this.networkIncident.incidentNotes = [ ...this.networkIncident.incidentNotes, newNote ];
						}
					} else {
						this._alerts.setWhereWhatError( `${this.codeName}: getWhoIs`,
							'Services-Service failed.', 'Returned no data' );
					}
				},
				error: (error) => {
					this._alerts.setWhereWhatError( `${this.codeName}: getWhoIs`,
						'Services-Service failed.', error || 'Server error');
				},
				complete: () => { }
			});
		} else {
			this._console.Verbose( `${this.codeName}.ipChanged: Addresses are the same ${this.networkIncident.incident.IPAddress}` );
		}
	}
	/**
	** a new note id needs to be -2 or less
	** @returns number
	*/
	newNoteId(): number {
		let inId: number = -9999;
		if( this.networkIncident !== undefined ) {
			inId = Math.min.apply(Math,this.networkIncident.incidentNotes.map( (n) => n.IncidentNoteId )) - 1;
			if( inId > -2 ) {
				inId = -2;
			}
		}
		return inId;
	}
	/*
	** --------------------------------------------------------------------
	** File access
	** create & update
	*/
	/**
	** Call create data service,
	** if successful then emit to parent form success.
	** @param stay boolean
	*/
	createItem( stay: boolean ): void {
		this._console.Information( `${this.codeName}.createItem, Entering: ${stay}` );
		console.warn( `createItem ${stay}` );
		if( this.networkIncidentSave !== undefined ) {
			console.warn( `createItem defined` );
			this.httpCreateSubscription = this._netIncident.createModel<NetworkIncidentSave>( this.networkIncidentSave ).subscribe({
				next: ( netIncidentData: NetworkIncident ) => {
					this._console.Verbose( `${this.codeName}.createItem, netIncidentData` );
					if( netIncidentData.message !== '' ) {
						this._alerts.setWhereWhatError(
							this.codeName, `createItem`, `${netIncidentData.message}`);
					} else {
						this._alerts.setWhereWhatSuccess( this.codeName,
							`Created: ${this.networkIncident.incident.IncidentId}` );
					}
					this.moveNetIncidentDetail( netIncidentData, stay );
					this.add = false;
					this._console.Verbose(
						`${this.codeName}.createItem, Exiting` );
				},
				error: (error) => {
					this.baseErrorHandler( this.codeName, `Create`, error );
				},
				complete: () => { }
			});
		}
	}
	/**
	** Call update data service,
	** if successful then emit to parent form success.
	** @param stay boolean
	*/
	updateItem( stay: boolean ): void {
		this.httpUpdateSubscription = this._netIncident.updateModel<NetworkIncidentSave>( this.id, this.networkIncidentSave ).subscribe({
			next: ( netIncidentData: NetworkIncident ) => {
				this._console.Verbose( `${this.codeName}.updateItem, netIncidentData` );
				if( netIncidentData.message !== '' ) {
					this._alerts.setWhereWhatError(
						this.codeName, `updateItem`, `${netIncidentData.message}`);
				} else
					this._alerts.setWhereWhatSuccess(
						this.codeName, `Updated: + ${this.id}`);
				this.moveNetIncidentDetail( netIncidentData, stay );
			},
			error: (error) => {
				this.baseErrorHandler( this.codeName, `Update`, error );
			},
			complete: () => { }
		});
	}
	//
}
// End of: Incident-detail-window.component.ts
// ===========================================================================
