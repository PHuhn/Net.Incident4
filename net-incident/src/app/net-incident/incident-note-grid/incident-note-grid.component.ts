// File: incidentnote-grid.component.ts
import { Component, Input, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { NgForm } from '@angular/forms';
//
import { SelectItem } from 'primeng/api';
import { TableModule } from 'primeng/table';
import { ConfirmationService } from 'primeng/api';
//
import { ConsoleLogService } from '../../global/console-log/console-log.service';
import { AlertsService } from '../../global/alerts/alerts.service';
import { BaseCompService } from '../../global/base-comp/base-comp.service';
import { BaseComponent } from '../../global/base-comp/base-comp.component';
import { IIncident, Incident } from '../incident';
import { IIncidentNote, IncidentNote } from '../incident-note';
import { NetworkIncident } from '../network-incident';
import { IIncidentNoteWindowInput } from '../incident-note-detail-window/incident-note-detail-window.component';
//
@Component({
    selector: 'app-incident-note-grid',
    templateUrl: './incident-note-grid.component.html',
    standalone: false
})
export class IncidentNoteGridComponent extends BaseComponent implements AfterViewInit {
	/*
	** --------------------------------------------------------------------
	** Data declaration.
	** Window/dialog communication (also see onClose event)
	** Local variables
	*/
	totalRecords: number = 0;
	private id: number = -1;
	private disableDelete: boolean = false;
	public get DisableDelete( ): boolean {
		return this.disableDelete;
	}
	// xfer to detail window
	windowIncidentNoteInput: IIncidentNoteWindowInput | undefined;
	// communicate to the AlertComponent
	protected _alerts: AlertsService;
	// to write console logs condition on environment log-level
	protected _console: ConsoleLogService;
	// PrimeNG's Ok/Cancel confirmation dialog service
	protected _confirmationService: ConfirmationService;
	/*
	** --------------------------------------------------------------------
	** Inputs and emitted outputs
	** inputs: networklogs: NetworkLog[];
	** outputs:
	*/
	@Input() networkIncident: NetworkIncident;
	//
	constructor(
		// inject the base components services
		private _baseSrvc: BaseCompService,
	) {
		super( _baseSrvc );
		// get the needed services from the base component
		this._alerts = _baseSrvc._alerts;
		this._console = _baseSrvc._console;
		this._confirmationService = _baseSrvc._confirmationService;
		this.codeName = 'Incident-Note-Grid';
		//
		this.networkIncident = new NetworkIncident( );
		//
	}
	/**
	** Disable deletes if incudent is mailed
	*/
	ngAfterViewInit() {
		// all records are passed in via @Input
		this._console.Information( `${this.codeName}.ngAfterViewInit: ...` );
		if( this.networkIncident.incident.Mailed === true || this.networkIncident.incident.Closed === true ) {
			this.totalRecords = this.networkIncident.incidentNotes.length;
			this.disableDelete = true;
		}
	}
	// --------------------------------------------------------------------
	/**
	** Events:
	** addItemClicked, editItemClicked, deleteItemClicked, onClose
	** Add button clicked, launch edit detail window.
	*/
	addItemClicked( ) {
		this.windowIncidentNoteInput = {
			model: IncidentNote.empty( ),
			networkIncident: this.networkIncident,
			displayWin: true
		};
		this.id = this.windowIncidentNoteInput.model.IncidentNoteId;
		this._console.Information( `${this.codeName}.addItemClicked: Add item clicked` );
	}
	/**
	** Edit button clicked, launch edit detail window.
	*/
	editItemClicked( item: IncidentNote ) {
		this.id = item.IncidentNoteId;
		this.windowIncidentNoteInput = {
			model: item,
			networkIncident: this.networkIncident,
			displayWin: true
		};
		this._console.Information( `${this.codeName}.editItemClicked: open dialog: ${this.id}` );
		this._console.Information( JSON.stringify( item ) );
	}
	/**
	** Confirm component (delete)
	*/
	deleteItemClicked( item: IncidentNote ): boolean {
		this.id = item.IncidentNoteId;
		if( this.disableDelete === true && this.id > -1 ) {
			this._alerts.setWhereWhatWarning(
				this.codeName, `Locked, cannot delete id: ${this.id}`);
			return false;
		}
		this._console.Information( `${this.codeName}.deleteItemClicked: Id: ${this.id}` );
		// the p-confirmDialog in in app.component
		return this.baseDeleteConfirm<number>( this.id, (ident: number): boolean => {
			return this.deleteItem( ident );
		} );
	}
	/**
	** on edit window closed
	*/
	onCloseWin( saved: boolean ): void {
		this._console.Information( `${this.codeName}.onClose: Entering: on close with: ${saved}` );
		if( saved === true ) {
			this._console.Information( `${this.codeName}.onClose: Refreshing...` );
			this._console.Information( JSON.stringify( this.networkIncident.incidentNotes ) );
			this.totalRecords = this.networkIncident.incidentNotes.length;
		}
		this.windowIncidentNoteInput = undefined;
	}
	// --------------------------------------------------------------------
	// File access
	/**
	** delete
	** move row to deleted array
	*/
	deleteItem( delId: number ): boolean {
		if( this.id !== 0 ) {
			const note = this.networkIncident.incidentNotes.find(
				(el) => el.IncidentNoteId === delId );
			if( note !== undefined ) {
				this.networkIncident.deletedNotes.push( note );
				this.networkIncident.incidentNotes = this.networkIncident.incidentNotes.filter( (el) => {
					this._console.Information( `${this.codeName}.deleteItem: id: ${el.NoteTypeId}` );
					return el.IncidentNoteId !== delId;
				});
				this.totalRecords = this.networkIncident.incidentNotes.length;
				this._console.Information( `${this.codeName}.deleteItem: length: ${this.networkIncident.incidentNotes.length}` );
				this._alerts.setWhereWhatSuccess(
					this.codeName, `Deleted: ${delId}`);
			} else {
				this._alerts.setWhereWhatWarning(
					this.codeName, `Delete failed for: ${delId}` );
			}
		}
		return false;
	}
	//
}
// End of incidentnote-grid.component.ts
