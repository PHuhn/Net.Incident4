// ===========================================================================
import { ComponentFixture, TestBed, fakeAsync, tick, waitForAsync } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';
//
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
//
import { ConfirmationService, Confirmation } from 'primeng/api';
import { APP_MODULE_PRIMENG } from '../../APP.MODULE-PRIMENG';
//
import { APP_GLOBAL_COMPONENTS } from '../../global/APP.GLOBAL';
import { APP_COMPONENTS } from '../../APP.COMPONENTS';
//
import { AlertsService } from '../../global/alerts/alerts.service';
import { ServicesService } from '../services/services.service';
import { BaseCompService } from '../../global/base-comp/base-comp.service';
//
import { SelectItemExtra } from '../../global/primeng/select-item-class';
import { User } from '../user';
import { Incident } from '../incident';
import { INetworkIncident, NetworkIncident } from '../network-incident';
import { IncidentNote } from '../incident-note';
import { IIncidentNoteWindowInput } from '../incident-note-detail-window/incident-note-detail-window.component';
//
import { IncidentNoteGridComponent } from './incident-note-grid.component';
//
describe( 'IncidentNoteGridComponent', ( ) => {
	// IncidentNoteGridComponent services:
	// private _alerts: AlertsService,
	// private _confirmationService: ConfirmationService
	// IncidentNoteGridComponent loads: IncidentNoteDetailWindowComponent
	// which uses dropdown, ServicesService, http
	let sut: IncidentNoteGridComponent;
	let fixture: ComponentFixture<IncidentNoteGridComponent>;
	let alertService: AlertsService;
	let baseService: BaseCompService;
	let confirmService: ConfirmationService;
	let windowNoteInput: IIncidentNoteWindowInput;
	//
	const servicesServiceSpy = jasmine.createSpyObj('ServicesService',
		['getPing', 'getWhoIs', 'getService']);
	const expectedWindowTitle: string = 'Incident Note Detail: ';
	const windowTitleSelector: string =
		'app-incidentnote-detail-window > p-dialog > div > div.p-dialog-titlebar > span > p-header > div';
	const ipAddr: string = '192.169.1.1';
	//
	const mockDatum = [
		new IncidentNote( 1,1,'Ping','i 1',new Date( '2018-01-01T00:00:00' ), false ),
		new IncidentNote( 2,2,'Whois','i 2',new Date( '2018-01-02T00:00:00' ), false ),
		new IncidentNote( 3,3,'ISP Rpt','i 3',new Date( '2018-01-03T00:00:00' ), false ),
		new IncidentNote( 4,1,'Ping','i 4',new Date( '2018-01-04T00:00:00' ), false ),
		new IncidentNote( 5,2,'Whois','i 5',new Date( '2018-01-05T00:00:00' ), false ),
		new IncidentNote( 6,3,'ISP Rpt','i 6',new Date( '2018-01-06T00:00:00' ), false )
	];
	//
	const inc: Incident = new Incident( 4,1,ipAddr,'arin.net','PSG169',
		'dandy@psg.com','',false,false,false,'',new Date( '2018-04-01T01:00:00' ) );
	const mockNoteTypes: SelectItemExtra[] = [
		new SelectItemExtra( 1, 'Ping', 'ping' ),
		new SelectItemExtra( 2, 'WhoIs', 'whois' ),
		new SelectItemExtra( 3, 'ISP Rpt', 'email' ),
		new SelectItemExtra( 4, 'ISP Addl', '' ),
		new SelectItemExtra( 5, 'ISP Resp', '' )
	];
	//
	beforeEach( waitForAsync( ( ) => {
		TestBed.configureTestingModule(  {
			imports: [
				FormsModule,
				APP_MODULE_PRIMENG,
			],
			declarations: [
				APP_GLOBAL_COMPONENTS,
				APP_COMPONENTS,
			],
			providers: [
				BaseCompService,
				AlertsService,
				ConfirmationService,
				provideHttpClient(),
				provideHttpClientTesting(),
				{ provide: ServicesService, useValue: servicesServiceSpy },
			]
		});
		// Setup injected pre service for each test
		baseService = TestBed.inject( BaseCompService );
		alertService = baseService._alerts;
		confirmService = baseService._confirmationService;
		TestBed.compileComponents( );
	}));
	//
	beforeEach( ( ) => {
		windowNoteInput = {
			model: { ... mockDatum[2] },
			networkIncident: createNetworkIncident( ),
			displayWin: true
		};
		fixture = TestBed.createComponent( IncidentNoteGridComponent );
		sut = fixture.componentInstance;
		// clone the array with slice( 0 )
		sut.networkIncident = createNetworkIncident( );
		sut.ngAfterViewInit( );
		fixture.detectChanges( ); // trigger initial data binding
		fixture.whenStable( );
	} );
	//
	function createNetworkIncident( ): INetworkIncident {
		const networkIncident: INetworkIncident = new NetworkIncident();
		networkIncident.incident = inc.Clone();
		networkIncident.noteTypes = [ ... mockNoteTypes ];
		networkIncident.deletedLogs = [];
		networkIncident.deletedNotes = [];
		networkIncident.incidentNotes = [ ... mockDatum ];
		networkIncident.typeEmailTemplates = [];
		networkIncident.user = User.empty( );
		return networkIncident;
	}
	/*
	** Cleanup so no dialog window will still be open
	*/
	function windowCleanup( ) {
		sut.windowIncidentNoteInput = undefined;
		tickFakeWait( 1 );
	}
	/*
	** Pause for events to process.
	*/
	function tickFakeWait( ticks: number ) {
		tick( ticks );
		fixture.detectChanges( ); // trigger initial data binding
		fixture.whenStable( );
	}
	//
	// Component instantiates
	//
	it( 'should be created ...', ( ) => {
		console.log(
			'===================================\n' +
			'IncidentNoteGridComponent should create ...' );
		expect( sut ).toBeTruthy( );
	} );
	//
	// getIncidentNotes( ) : Observable<IIncidentNote[]>
	//
	it('should initialize with all data ...', fakeAsync( () => {
		expect( sut.networkIncident.incidentNotes.length ).toBe( 6 );
		//
		expect( sut.networkIncident.incidentNotes[ 0 ].IncidentNoteId ).toEqual( 1 );
		expect( sut.networkIncident.incidentNotes[ 1 ].IncidentNoteId ).toEqual( 2 );
		expect( sut.networkIncident.incidentNotes[ 2 ].IncidentNoteId ).toEqual( 3 );
		expect( sut.networkIncident.incidentNotes[ 3 ].IncidentNoteId ).toEqual( 4 );
		expect( sut.networkIncident.incidentNotes[ 4 ].IncidentNoteId ).toEqual( 5 );
		expect( sut.networkIncident.incidentNotes[ 5 ].IncidentNoteId ).toEqual( 6 );
	}));
	//
	// addItemClicked( )
	//
	it('should launch detail window when addItemClicked is called ...', fakeAsync( () => {
		sut.addItemClicked( );
		//
		tickFakeWait( 10 );
		//
		const title: HTMLDivElement = fixture.debugElement.query(By.css(
			'#incidentNoteDetailWindow > p-dialog > div > div > div.p-dialog-header' )).nativeElement;
		expect( title.innerText ).toEqual( expectedWindowTitle + '0' );
		windowCleanup( );
		tickFakeWait( 10 );
	}));
	//
	// editItemClicked( )
	//
	it('should launch detail window when editItemClicked is called ...', fakeAsync( () => {
		const incidentNote: IncidentNote = sut.networkIncident.incidentNotes[ 3 ];
		sut.editItemClicked( incidentNote );
		//
		tickFakeWait( 10 );
		//
		const title: HTMLDivElement = fixture.debugElement.query(By.css(
			'#incidentNoteDetailWindow > p-dialog > div > div > div.p-dialog-header' )).nativeElement;
		// console.warn( title );
		expect( title.innerText ).toEqual( expectedWindowTitle + incidentNote.IncidentNoteId );
		windowCleanup( );
		tickFakeWait( 10 );
	}));
	/*
	** deleteItemClicked( item: IncidentNote ) :boolean
	*/
	it('should delete row when event called and OK is clicked...', fakeAsync(() => {
		// given
		spyOn(confirmService, 'confirm').and.callFake(
			(confirmation: Confirmation) => {
				console.log(confirmation.message);
				if( confirmation.accept !== undefined ) {
					return confirmation.accept();
				}
				return true;
			});
		// when
		const ret: Boolean =
				sut.deleteItemClicked( sut.networkIncident.incidentNotes[ 2 ] );
		// then
		expect( ret ).toEqual( false );
		tickFakeWait( 1 );
		// give it very small amount of time
		expect( sut.networkIncident.incidentNotes.length ).toBe( 5 );
		expect( sut.networkIncident.deletedNotes.length ).toBe( 1 );
		//
	}));
	//
	it('should not delete if mailed ...', fakeAsync(() => {
		// given
		sut.networkIncident.incident.Mailed = true;
		sut.ngAfterViewInit( );
		spyOn( alertService, 'setWhereWhatWarning' );
		tickFakeWait( 1 );
		// when
		const ret: Boolean =
				sut.deleteItemClicked( sut.networkIncident.incidentNotes[ 2 ] );
		// then
		expect( ret ).toEqual( false );
		expect( sut.DisableDelete ).toEqual( true );
		tickFakeWait( 1 );
		expect( alertService.setWhereWhatWarning ).toHaveBeenCalled( );
		//
	}));
	//
	it('should not delete if not found ...', fakeAsync(() => {
		// given
		spyOn( alertService, 'setWhereWhatWarning' );
		// when
		const ret: Boolean = sut.deleteItem( -99 );
		// then
		expect( ret ).toEqual( false );
		expect( alertService.setWhereWhatWarning ).toHaveBeenCalled( );
		//
	}));
	//
	it('deleteItem: should delete if found ...', fakeAsync(() => {
		// given
		sut.networkIncident.incidentNotes = [
			new IncidentNote( 1,1,'Ping','i 1',new Date( '2018-01-01T00:00:00' ), false ),
			new IncidentNote( 2,4,'ISP Addl','Blah blah',new Date( '2018-01-02T10:00:00' ), false ),
			new IncidentNote( 3,4,'ISP Addl','Blah blah',new Date( '2018-01-02T10:02:00' ), false ),
			new IncidentNote( 4,4,'ISP Addl','Blah blah',new Date( '2018-01-02T10:04:00' ), false ),
		];
		// when
		const ret4: Boolean = sut.deleteItem( 4 );
		const ret3: Boolean = sut.deleteItem( 3 );
		// then
		expect( ret4 ).toEqual( false );
		expect( ret3 ).toEqual( false );
		expect( sut.networkIncident.incidentNotes.length ).toBe( 2 );
		expect( sut.networkIncident.deletedNotes.length ).toBe( 2 );
		console.log( `deletedItem: ${sut.networkIncident.deletedNotes[0].toString()}` )
		console.log( `deletedItem: ${sut.networkIncident.deletedNotes[1].toString()}` )
		//
	}));
	//
	// onClose( saved: boolean ): void
	//
	it('onCloseWin: on save should cleanup ...', fakeAsync(() => {
		// given
		sut.windowIncidentNoteInput = windowNoteInput;
		tickFakeWait( 1 );
		// when
		sut.onCloseWin( true );
		// then
		tickFakeWait( 1 );
		expect( sut.windowIncidentNoteInput ).toBeUndefined( );
		tickFakeWait( 100 );
		//
	}));
	//
} );
// ===========================================================================
