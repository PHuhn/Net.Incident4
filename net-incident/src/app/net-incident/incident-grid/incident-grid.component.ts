// File: incident-grid.component.ts
import { Component, OnInit, OnDestroy, Input, ViewChild, EventEmitter } from '@angular/core';
import { NgForm } from '@angular/forms';
//
import { Table } from 'primeng/table';
import { ConfirmDialog } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';
import { SelectItem } from 'primeng/api';
import { FilterMetadata } from 'primeng/api';
import { LazyLoadEvent } from 'primeng/api';
//
import { ILazyResults } from '../../global/base-srvc/ibase-srvc';
import { AlertsService } from '../../global/alerts/alerts.service';
import { ConsoleLogService } from '../../global/console-log/console-log.service';
import { BaseCompService } from '../../global/base-comp/base-comp.service';
import { BaseComponent } from '../../global/base-comp/base-comp.component';
import { DetailWindowInput } from '../DetailWindowInput';
import { IUser, User } from '../user';
import { UserService } from '../services/user.service';
import { IncidentService } from '../services/incident.service';
import { IIncident, Incident } from '../incident';
import { LazyLoadEvent2 } from '../../global/LazyLoadEvent2';
import { IncidentDetailWindowComponent } from '../incident-detail-window/incident-detail-window.component';
import { ServerSelectionWindowComponent } from '../server-selection-window/server-selection-window.component';
import { AppComponent } from '../../app.component';
//
@Component({
	selector: 'app-incident-grid',
	templateUrl: './incident-grid.component.html',
	styleUrls: ['./incident-grid.component.css']
})
export class IncidentGridComponent extends BaseComponent implements OnInit {
	//
	// --------------------------------------------------------------------
	// Data declaration.
	//
	// Window/dialog communication (also see onClose event)
	windowDisplay: boolean = false;
	selectItemsWindow: SelectItem[] = [];
	displayServersWindow: boolean = false;
	detailWindow: DetailWindowInput | undefined;
	// Local variables
	incidents: Incident[] = [];
	totalRecords: number = 0;
	lastTableLazyLoadEvent: LazyLoadEvent2;
	id: number = -1;
	loading: boolean = false;
	@ViewChild('dt', { static: true }) dt: Table | undefined;
	//
	mailed: boolean = false;
	closed: boolean = false;
	special: boolean = false;
	// communicate to the AlertComponent
	protected _alerts: AlertsService;
	// to write console logs condition on environment log-level
	protected _console: ConsoleLogService;
	// PrimeNG's Ok/Cancel confirmation dialog service
	protected _confirmationService: ConfirmationService;
	//
	// --------------------------------------------------------------------
	// Inputs and emitted outputs
	// 	inputs: login user
	// 	outputs: onClose
	//
	@Input() user: User;
	//
	constructor(
		// inject the base components services
		private _baseSrvc: BaseCompService,
		private _data: IncidentService,
		private _user: UserService,
	) {
		super( _baseSrvc );
		// get the needed services from the base component
		this._alerts = _baseSrvc._alerts;
		this._console = _baseSrvc._console;
		this._confirmationService = _baseSrvc._confirmationService;
		this.codeName = 'incident-grid-component';
		//
		this.user = User.empty( );
		this.detailWindow = undefined;
		//
	}
	//
	// On component initialization, get all data from the data service.
	//
	ngOnInit() {
		this._console.Debug(
			`${this.codeName}.ngOnInit: Entering ...` );
		this.loading = true;
	}
	//
	// --------------------------------------------------------------------
	// Events:
	// addItemClicked, editItemClicked, deleteItemClicked, onClose
	// Add button clicked, launch edit detail window.
	//
	addItemClicked( ) {
		this._console.Debug(
			`${this.codeName}.addItemClicked: Entering ...` );
		this._console.Debug( JSON.stringify( this.user ) );
		if( AppComponent.securityManager !== undefined ) {
			if( AppComponent.securityManager.isValidIncidentDetail( ) ) {
				const empty: Incident = this._data.emptyIncident( );
				empty.ServerId = this.user.Server.ServerId;
				this.editItemClicked( empty );
			} else {
				this._alerts.setWhereWhatWarning( this.codeName, 'Not authorized' );
			}
		}		
	}
	//
	// Edit button clicked, launch edit detail window.
	//
	editItemClicked( item: Incident ) {
		//
		if( AppComponent.securityManager !== undefined ) {
			if( AppComponent.securityManager.isValidIncidentDetail( ) ) {
				this.id = item.IncidentId;
				this._console.Information(
					`${this.codeName}.editItemClicked: Entering, id: ${this.id}` );
				this.detailWindow = new DetailWindowInput( this.user, item );
				this.windowDisplay = true;
				this._console.Information(
					`${this.codeName}.editItemClicked: ${this.windowDisplay}` );
			} else {
				this._alerts.setWhereWhatWarning( this.codeName, 'Not authorized' );
			}
		}		
	}
	//
	// Confirm component (delete)
	//
	deleteItemClicked( item: Incident ): boolean {
		if( AppComponent.securityManager !== undefined ) {
			if( AppComponent.securityManager.isValidIncidentDetail( ) ) {
				this.id = item.IncidentId;
				this._console.Information(
					`${this.codeName}.deleteItemClicked: Entering, id: ${this.id}` );
				// the p-confirmDialog in in app.component
				return this.baseDeleteConfirm<number>( this.id, (ident: number): boolean => {
					return this.deleteItem( ident );
				}, '' );
			} else {
				this._alerts.setWhereWhatWarning(
					this.codeName, 'Not authorized' );
			}
		} else {
			this._alerts.setWhereWhatError(
				this.codeName, 'deleteItemClicked', 'securityManager undefined' );
		}
		return false;
	}
	//
	// on edit window closed
	//
	onClose( saved: boolean ) {
		this._console.Debug(
			`${this.codeName}.onClose: entering: ${saved}` );
		if( saved === true ) {
			this._console.Information(
				`${this.codeName}.onClose: Refreshing...` );
			this.refreshWithLastEvent();
		}
		this.windowDisplay = false;
		this.detailWindow = undefined;
		this.incidents = [ ... this.incidents ];
	}
	//
	// onChangeServer( "server" )
	// Launch server selection window
	//
	onChangeServer( event: any ) {
		this._console.Debug(
			`${this.codeName}.onChangeServer: entering: ${event}` );
		this.selectItemsWindow = this.user.ServerShortNames;
		this.displayServersWindow = true;
		this._console.Information(
			`${this.codeName}.onChangeServer: ${event.value}` );
	}
	//
	// onServerSelected($event)
	//
	onServerSelected( event: any ) {
		this._console.Debug(
			`${this.codeName}.onServerSelected: entering: ${event}` );
		this.getUserServer( this.user.UserName, event );
	}
	/**
	** Refresh by resending the last lazy loading event
	** to the load method.
	*/
	refreshWithLastEvent(): void {
		this._console.Debug(
			`${this.codeName}.refreshWithLastEvent: entered` );
		this.loadIncidentsLazy(this.lastTableLazyLoadEvent);
	}
	//
	// --------------------------------------------------------------------
	// File access
	// get user with user service
	// getAll & delete
	//
	getUserServer( userName: string, serverShortName: string ) {
		//
		this._console.Debug( `${this.codeName}.getUserServer: user: ${userName}, short: ${serverShortName}` );
		const srvcParam: any = { id: userName, serverShortName: serverShortName };
		this._user.getModelById<User>( srvcParam ).subscribe({
			next: ( userData: User ) => {
				this._console.Information(
					`${this.codeName}.authUser: user: ${userData.UserName}` );
				if( userData.ServerShortName !== ''
					&& userData.ServerShortName.toLowerCase()
							=== serverShortName.toLowerCase() ) {
						const changed: boolean = ( userData.ServerShortName.toLowerCase() !== this.user.ServerShortName.toLocaleLowerCase() );
						this.user = userData;
						this.dt.filter( this.user.Server.ServerId, 'ServerId', 'equals' );
						this.displayServersWindow = false;
				} else {
					this._console.Information(
						`${this.codeName}.getUserServer, Returned: ${userData.ServerShortName}` );
					this.selectItemsWindow = this.user.ServerShortNames;
					this.displayServersWindow = true;
				}
			},
			error: (error) => {
				this.baseErrorHandler(
					this.codeName, `User not found: ${userName}`, error );
			},
			complete: () => { }
		});
		//
	}
	/**
	** event latout as follows:
	** {first: 3, rows: 3, sortField: "AbuseEmailAddress", sortOrder: 1,
	** filters: }, globalFilter: null, multiSortMeta: undefined}
	** make a remote request to load data using state metadata from event
	** event.first = First row offset
	** event.rows = Number of rows per page
	** event.sortField = Field name to sort with
	** event.sortOrder = Sort order as number, 1 for asc and -1 for dec
	** filters: FilterMetadata[] object having field as key and filter value, filter matchMode as value
	** @param event LazyLoadEvent but with array of FilterMetadat
	*/
	loadIncidentsLazy( event: LazyLoadEvent2 ) {
		setTimeout( ( ) => {
			this.loading = true;
			event.globalFilter = '';
			event.sortField = '';
			// The following 6 fields are defined by p-columnFilter:
			// IncidentId, IPAddress, NIC, NetworkName, AbuseEmailAddress, ISPTicketNumber
			// Manually apply the caption filters, to force the filter.
			const ev: LazyLoadEvent2 = { ... event };
			ev.filters = { ... event.filters, 
				'ServerId': [{ value: this.user.Server.ServerId, matchMode: 'equals' }],
				'Mailed': [{ value: this.mailed, matchMode: 'equals' }],
				'Closed': [{ value: this.closed, matchMode: 'equals' }],
				'Special': [{ value: this.special, matchMode: 'equals' }],
			};
			this.lastTableLazyLoadEvent = ev;
			//
			this._data.postJsonBody<ILazyResults>( ev ).subscribe({
				next: (paginationData: ILazyResults ) => {
					this._console.Debug( `${this.codeName}.loadIncidentsLazy: # records: ${paginationData.results.length} totalRecords: ${paginationData.totalRecords}` );
					// console.warn( JSON.stringify( paginationData ) );
					this.loading = false;
					this.incidents = paginationData.results as IIncident[];
					this.totalRecords = paginationData.totalRecords;
				},
				error: (error) => {
					this.loading = false;
					this.baseErrorHandler(
						this.codeName, `loadIncidentsLazy`, error );
				}
			});
		}, 0 );
	}
	/**
	** Call delete data service,
	** if successful then delete the row from array
	** @param delId 
	** @returns 
	*/
	deleteItem( delId: number ): boolean {
		if( delId !== 0 ) {
			this._data.deleteModel<Incident>( delId ).subscribe({
				next: ( ) => {
					this.incidents = this.incidents.filter(function(el) {
						return el.IncidentId !== delId;
						});
					this._alerts.setWhereWhatSuccess(
						'Incident-Grid', 'Deleted:' + delId);
				},
				error: (error) => {
					this.baseErrorHandler(
						this.codeName, 'Incident-Grid Delete', error )
				},
				complete: () => { }
			});
		}
		return false;
	}
	//
}
// End of incident-grid.component.ts
