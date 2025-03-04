// ===========================================================================
import { ComponentFixture, TestBed, inject, fakeAsync, tick, waitForAsync } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { of, throwError } from 'rxjs';
//
import { HttpResponse, HttpErrorResponse, provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
//
import { ConfirmationService } from 'primeng/api';
import { APP_MODULE_PRIMENG } from '../../APP.MODULE-PRIMENG';
//
import { BaseCompService } from '../../global/base-comp/base-comp.service';
import { AlertsService } from '../../global/alerts/alerts.service';
import { Message } from '../../global/alerts/message';
import { ConsoleLogService } from '../../global/console-log/console-log.service';
import { ServicesService } from '../services/services.service';
import { SelectItemClass, SelectItemExtra } from '../../global/primeng/select-item-class';
import { Incident } from '../incident';
import { NetworkIncident } from '../network-incident';
import { NetworkLog } from '../network-log';
import { ServerData } from '../server-data';
import { User } from '../user';
import { IIncidentNote, IncidentNote } from '../incident-note';
import { IncidentNoteDetailWindowComponent, IIncidentNoteWindowInput } from './incident-note-detail-window.component';
import { LoadingSpinnerComponent } from '../../global/primeng/loading-spinner/loading-spinner.component';
//
describe( 'IncidentNoteDetailWindowComponent', ( ) => {
	let sut: IncidentNoteDetailWindowComponent;
	let fixture: ComponentFixture<IncidentNoteDetailWindowComponent>;
	let alertService: AlertsService;
	let baseService: BaseCompService;
	let consoleService: ConsoleLogService;
	const incidentnoteServiceSpy = jasmine.createSpyObj(
		'IncidentNoteService', ['emptyIncidentNote', 'validate', 'getIncidentNote', 'createIncidentNote', 'updateIncidentNote']);
	const servicesServiceSpy = jasmine.createSpyObj(
		'ServicesService', ['getPing', 'getWhoIs']);
	const startDate: Date = new Date('2018-03-11T02:00:00');
	const endDate: Date = new Date('2018-11-04T02:00:00');
	const standardDate: Date = new Date('2018-03-10T23:00:00');
	const daylightDate: Date = new Date('2018-03-11T02:01:00');
	//
	const serverMock = new ServerData(
		1, 1, 'NSG', 'Srv 1', 'Members Web-site',
		'Web-site', 'Web-site address: www.nsg.com',
		'We are in Michigan, USA.', 'User Admin', 'Admin', 'ServerAdmin@yahoo.com',
		'EST (UTC-5)', true,  'EDT (UTC-4)', startDate, endDate
	);
	//
	const userMock = new User('e0-04','Head','Head','Admin','Head Admin','U','UN4@yahoo.com',
		true,'734-555-1212', true,1, [new SelectItemClass('srv 1','Server 1')],'srv 1',serverMock, ['admin']);
	//
	const ipAddr: string = '192.169.1.1';
	//
	let mockData: IncidentNote;
	//
	const mockDatum = [
		new IncidentNote( 1,1,'Ping','i 1',new Date( '2018-01-01T00:00:00' ), false ),
		new IncidentNote( 2,2,'Whois','i 2',new Date( '2018-01-02T00:00:00' ), false ),
		new IncidentNote( 3,3,'ISP Rpt','i 3',new Date( '2018-01-03T00:00:00' ), false ),
		new IncidentNote( 4,1,'Ping','i 4',new Date( '2018-01-04T00:00:00' ), false ),
		new IncidentNote( 5,2,'Whois','i 5',new Date( '2018-01-05T00:00:00' ), false ),
		new IncidentNote( 6,3,'ISP Rpt','i 6',new Date( '2018-01-06T00:00:00' ), false )
	];
	const noteTypes: SelectItemExtra[] = [
		new SelectItemExtra( 1, 'Ping', 'ping' ),
		new SelectItemExtra( 2, 'WhoIs', 'whois' ),
		new SelectItemExtra( 3, 'ISP Rpt', 'email' ),
		new SelectItemExtra( 4, 'ISP Addl', '' ),
		new SelectItemExtra( 5, 'ISP Resp', '' )
	];
	//
	const inc: Incident = new Incident( 4,1,ipAddr,'arin.net','PSG169',
		'dandy@psg.com','',false,false,false,'',new Date( '2018-04-01T01:00:00' ) );
	const emptyData: IncidentNote =
		new IncidentNote( 0,0,'','',new Date( '2010-01-01T00:00:00' ) );
	//
	const netInc = new NetworkIncident();
	let testWindowIncidentNoteInput: IIncidentNoteWindowInput;
	//
	beforeEach( waitForAsync( ( ) => {
		TestBed.configureTestingModule(  {
			imports: [
				FormsModule,
				APP_MODULE_PRIMENG,
				BrowserAnimationsModule
			],
			declarations: [
				IncidentNoteDetailWindowComponent,
				LoadingSpinnerComponent
			],
			providers: [
				BaseCompService,
				AlertsService,
				ConsoleLogService,
				ConfirmationService,
				provideHttpClient(),
				provideHttpClientTesting(),
				{ provide: ServicesService, useValue: servicesServiceSpy },
			]
		});
		// Setup injected pre service for each test
		baseService = TestBed.inject( BaseCompService );
		alertService = baseService._alerts;
		consoleService = baseService._console;
		// Setup sut
		TestBed.compileComponents();
	}));
	//
	beforeEach( inject( [AlertsService], ( srvc: AlertsService ) => {
		alertService = srvc;
	} ) );
	//
	beforeEach( ( ) => {
		// pretend that it was wired to something that supplied a row
		const nl = new NetworkLog(99, serverMock.ServerId, inc.IncidentId, ipAddr, new Date( '2018-01-19T01:00:00' ), 'Fake log', 3, 'Type 3', true);
		netInc.incident = inc.Clone();
		netInc.networkLogs = [ nl ];
		netInc.deletedLogs = [];
		netInc.deletedNotes = [];
		netInc.incidentNotes = [ ... mockDatum ];
		netInc.typeEmailTemplates = [];
		netInc.user = userMock;
		netInc.noteTypes = noteTypes;
		mockData = { ... mockDatum[3] };
		testWindowIncidentNoteInput = {
			model: { ... mockData },
			networkIncident: netInc,
			displayWin: true
		};
		//
		fixture = TestBed.createComponent( IncidentNoteDetailWindowComponent );
		sut = fixture.componentInstance;
		sut.incidentnote = undefined;
		fixture.detectChanges( ); // trigger initial data binding
		fixture.whenStable( );
	} );
	/*
	** Cleanup so no dialog window will still be open
	*/
	function windowCleanup( ) {
		sut.incidentnote = undefined;
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
	/*
	** Component is instantiated
	*/
	it( 'should be created ...', fakeAsync( ( ) => {
		console.log(
			'===================================\n' +
			'IncidentNoteDetailWindowComponent should create ...' );
		// given / when / then
		expect( sut ).toBeTruthy( );
		windowCleanup( );
	} ) );
	/*
	** Verify data is transmitted to model via @input statement
	*/
	it( 'should get the mock data...', fakeAsync( ( ) => {
		// given / when
		sut.incidentnote = { ... testWindowIncidentNoteInput };
		tickFakeWait( 10 );
		// then
		expect( sut.model.IncidentNoteId ).toEqual( mockData.IncidentNoteId );
		expect( sut.model.NoteTypeId ).toEqual( mockData.NoteTypeId );
		expect( sut.model.Note ).toEqual( mockData.Note );
		expect( sut.model.CreatedDate ).toEqual( mockData.CreatedDate );
		windowCleanup( );
		tickFakeWait( 10 );
	} ) );
	//
	it( 'incidentnote: should get the input data ...', fakeAsync( ( ) => {
		// given
		sut.incidentnote = { ... testWindowIncidentNoteInput };
		tickFakeWait( 10 );
		// when
		const _input: IIncidentNoteWindowInput = sut.incidentnote;
		// then
		expect( _input ).toEqual( testWindowIncidentNoteInput );
		windowCleanup( );
		tickFakeWait( 10 );
	} ) );
	/*
	** updateItem( ): void
	** Call updateItem directly with the default data.
	*/
	it('updateItem: should update class when updateItem called ...', fakeAsync( ( ) => {
		// given
		sut.incidentnote = { ... testWindowIncidentNoteInput };
		tickFakeWait( 10 );
		const response = new HttpResponse( { status: 201, statusText: 'OK' } );
		incidentnoteServiceSpy.updateIncidentNote.and.returnValue(of( response ));
		sut.emitCloseWin.subscribe( saved => {
			sut.displayWin = false;
			expect( saved ).toBe( true );
		} );
		// when
		sut.updateItem( );
		// then
		tick( 10 );
		windowCleanup( );
		//
	} ) );
	//
	it('updateItem: should fail when id not found ...', fakeAsync( ( ) => {
		// given
		sut.incidentnote = { ... testWindowIncidentNoteInput };
		sut.model.IncidentNoteId = 99;
		spyOn( alertService, 'setWhereWhatWarning' );
		tickFakeWait( 1 );
		// when
		sut.updateItem( );
		// then
		tick( 10 );
		expect( alertService.setWhereWhatWarning ).toHaveBeenCalled( );
		windowCleanup( );
		//
	} ) );
	//
	// Call directly createItem
	//
	it('should create class when createItem called ...', fakeAsync(() => {
		// given
		const response = new HttpResponse( { status: 201, statusText: 'OK' } );
		const testInput = { ... testWindowIncidentNoteInput };
		testInput.model = { ... emptyData };
		sut.incidentnote = testInput;
		tickFakeWait( 10 );
		sut.model.IncidentNoteId = 0;
		sut.model.NoteTypeId = 3;
		sut.model.NoteTypeShortDesc = 'ISP Rpt';
		sut.model.Note = 'i 5';
		sut.model.CreatedDate = new Date( '2000-01-01T00:00:00' );
		const count = sut.networkIncident.incidentNotes.length;
		incidentnoteServiceSpy.createIncidentNote.and.returnValue(of( response ));
		sut.emitCloseWin.subscribe( saved => {
			sut.displayWin = false;
			expect( saved ).toBe( true );
		} );
		// when
		sut.createItem( );
		// then
		tick( 10 );
		expect( sut.networkIncident.incidentNotes.length).toEqual( count + 1 );
		windowCleanup( );
		//
	} ) );
	//
	it('createItem: should fail if Id is not zero ...', fakeAsync(() => {
		// given
		sut.model = { ... emptyData };
		sut.model.IncidentNoteId = -1;
		spyOn( alertService, 'setWhereWhatWarning' );
		// when
		sut.createItem( );
		// then
		tick( 10 );
		expect( alertService.setWhereWhatWarning ).toHaveBeenCalled( );
		windowCleanup( );
		//
	} ) );
	/*
	** windowClose(saved: boolean)
	** Simulate a button clicked, by calling event method
	*/
	it('windowClose: should update class when save button clicked ...', fakeAsync(() => {
		// given
		const response = new HttpResponse( { status: 204, statusText: 'OK' } );
		incidentnoteServiceSpy.validate.and.returnValue(of( [] ));
		incidentnoteServiceSpy.updateIncidentNote.and.returnValue(of( response ));
		sut.incidentnote = { ... testWindowIncidentNoteInput };
		tickFakeWait( 10 );
		spyOn( sut.emitCloseWin, 'emit' );
		sut.emitCloseWin.subscribe( ( saved: boolean ) => {
			// then
			expect( saved ).toBe( true );
			windowCleanup( );
		} );
		// when
		sut.windowClose( true );
		// then
		tickFakeWait( 10 );
		expect( sut.emitCloseWin.emit ).toHaveBeenCalledWith( true );
	} ) );
	//
	it('windowClose: should create class when save button clicked ...', fakeAsync(() => {
		// given
		const response = new HttpResponse( { status: 204, statusText: 'OK' } );
		incidentnoteServiceSpy.validate.and.returnValue(of( [] ));
		incidentnoteServiceSpy.updateIncidentNote.and.returnValue(of( response ));
		const _input = { ... testWindowIncidentNoteInput };
		_input.model = new IncidentNote(0, 4, 'gen','-', new Date(), true);
		sut.incidentnote = _input;
		spyOn( sut.emitCloseWin, 'emit' );
		tickFakeWait( 10 );
		// when
		sut.windowClose( true );
		// then
		tickFakeWait( 10 );
		expect( sut.emitCloseWin.emit ).toHaveBeenCalledWith( true );
		//
	} ) );
	//
	it('updateItem: should fail if Id is not zero ...', fakeAsync(() => {
		// given
		sut.model = { ... emptyData };
		sut.model.IncidentNoteId = 0;
		sut.add === false 
		spyOn( alertService, 'setWhereWhatWarning' );
		// when
		sut.updateItem( );
		// then
		tick( 10 );
		expect( alertService.setWhereWhatWarning ).toHaveBeenCalled( );
		windowCleanup( );
		//
	} ) );
	/*
	** onTypeIdDropdownChanged( selected: number )
	** (change)='onTypeIdDropdownChanged( $event )'
	*/
	it('onTypeIdChanged: call the onChange event ...', fakeAsync(() => {
		// given
		sut.incidentnote = { ... testWindowIncidentNoteInput };
		tickFakeWait( 10 );
		const id: number = 4;
		// when
		sut.onTypeIdDropdownChanged(id);
		// then
		expect( sut.model.NoteTypeId ).toEqual( id );
		windowCleanup( );
		tickFakeWait( 10 );
		//
	} ) );
	//
	it('onTypeIdChanged: call the onChange event (ping) ...', fakeAsync(() => {
		// given
		sut.incidentnote = { ... testWindowIncidentNoteInput };
		sut.add = true;
		tickFakeWait( 10 );
		const id: number = 1;
		const resp: string = `Pinging USA.NET [${ipAddr}] with 32 bytes of data:`;
		servicesServiceSpy.getPing.and.returnValue(of( resp ));
		// when
		sut.onTypeIdDropdownChanged(id);
		// then
		expect( sut.model.NoteTypeId ).toEqual( id );
		expect( sut.model.Note ).toEqual( resp );
		windowCleanup( );
		tickFakeWait( 10 );
		//
	} ) );
	//
	// performIncidentType( id: number, noteScript: string ): void
	//
	it('performIncidentType: call the onChange event ...', fakeAsync(() => {
		// given
		const testInput = { ... testWindowIncidentNoteInput };
		testInput.model = { ... emptyData };
		sut.incidentnote = testInput;
		tickFakeWait( 10 );
		const id: number = 1;
		const resp: string = `Pinging USA.NET [${ipAddr}] with 32 bytes of data:`;
		servicesServiceSpy.getPing.and.returnValue(of( resp ));
		// when
		sut.performIncidentType(id, 'ping');
		// then
		tickFakeWait( 10 );
		expect( sut.model.Note ).toEqual( resp );
		windowCleanup( );
		//
	} ) );
	//
	it('performIncidentType: call whois event ...', fakeAsync(() => {
		// given
		const testInput = { ... testWindowIncidentNoteInput };
		testInput.model = { ... emptyData };
		sut.incidentnote = testInput;
		tickFakeWait( 10 );
		const id: number = 1;
		const resp: HttpErrorResponse = new HttpErrorResponse({
			error: {}, status: 500, url: 'http://localhost', statusText: 'Bad Request' });
		servicesServiceSpy.getWhoIs.and.returnValue( throwError( ( ) => resp ) );
		spyOn( alertService, 'setWhereWhatError' );
		// when
		sut.performIncidentType(id, 'whois');
		// then
		tickFakeWait( 10 );
		expect( alertService.setWhereWhatError ).toHaveBeenCalled( );
		windowCleanup( );
		//
	} ) );
	//
	it('performIncidentType: call ISP report event ...', fakeAsync(() => {
		// given
		const testInput = { ... testWindowIncidentNoteInput };
		testInput.model = { ... emptyData };
		testInput.networkIncident.incident.ServerId = 0;
		sut.incidentnote = testInput;
		tickFakeWait( 10 );
		const id: number = 1;
		const resp: HttpErrorResponse = new HttpErrorResponse({
			error: {}, status: 500, url: 'http://localhost', statusText: 'Bad Request' });
		servicesServiceSpy.getWhoIs.and.returnValue( throwError( resp ));
		spyOn( alertService, 'warningSet' );
		// when
		sut.performIncidentType(id, 'email');
		// then
		tickFakeWait( 10 );
		expect( alertService.warningSet ).toHaveBeenCalled( );
		windowCleanup( );
		//
	} ) );
	//
	it('performIncidentType: alert called ...', fakeAsync(() => {
		// given
		spyOn( alertService, 'setWhereWhatWarning' );
		// when
		sut.performIncidentType( -99, 'bad type');
		// then
		tickFakeWait( 10 );
		expect( alertService.setWhereWhatWarning ).toHaveBeenCalled( );
		//
	} ) );
	//
	// Invoke the getPing server service
	//
	it('getPing: call the getPing server services ...', fakeAsync(() => {
		// given
		const testInput = { ... testWindowIncidentNoteInput };
		testInput.model = { ... emptyData };
		sut.incidentnote = testInput;
		tickFakeWait( 10 );
		const resp: string = `Pinging USA.NET [${ipAddr}] with 32 bytes of data:`;
		servicesServiceSpy.getPing.and.returnValue(of( resp ));
		// when
		sut.getPing();
		// then
		tickFakeWait( 10 );
		expect( sut.model.Note ).toEqual( resp );
		windowCleanup( );
		//
	} ) );
	//
	// Invoke the getPing server service
	//
	it('getPing: call the getPing server services error ...', fakeAsync(() => {
		// given
		sut.incidentnote = { ... testWindowIncidentNoteInput };
		tickFakeWait( 10 );
		const resp: HttpErrorResponse = new HttpErrorResponse({
			error: {}, status: 500, url: 'http://localhost', statusText: 'Bad Request' });
		servicesServiceSpy.getPing.and.returnValue( throwError( ( ) => resp ) );
		spyOn( alertService, 'setWhereWhatError' );
		// when
		sut.getPing();
		// then
		expect( alertService.setWhereWhatError ).toHaveBeenCalled( );
		tickFakeWait( 10 );
		//
	} ) );
	//
	// Invoke the getPing server service
	//
	it('getWhoIs: call the getWhoIs server services ...', fakeAsync(() => {
		// given
		const testInput = { ... testWindowIncidentNoteInput };
		testInput.model = { ... emptyData };
		sut.incidentnote = testInput;
		tickFakeWait( 10 );
		const resp: string = `Abuse contact for '${ipAddr} - ${ipAddr}' is 'noc@usa.net'`;
		servicesServiceSpy.getWhoIs.and.returnValue(of( resp ));
		// when
		sut.getWhoIs();
		// then
		tickFakeWait( 10 );
		expect( sut.model.Note ).toEqual( resp );
		windowCleanup( );
		//
	} ) );
	//
	// Invoke the getPing server service
	//
	it('getReport: call getReport for current network incident ...', fakeAsync(() => {
		// given
		sut.incidentnote = { ... testWindowIncidentNoteInput };
		tickFakeWait( 10 );
		const resp = 'Email Template error: not found.';
		// when
		sut.getReport();
		// then
		tickFakeWait( 10 );
		expect( sut.model.Note ).toEqual( resp );
		windowCleanup( );
		//
	} ) );
	/*
	** validate( ): boolean
	** validate, check against a common set of validation rules.
	*/
	it('validate: should fail and display alert warnings ...', fakeAsync( ( ) => {
		//
		sut.model = emptyData;
		sut.add = true;
		spyOn( alertService, 'warningSet' );
		// when
		const ret = sut.validate();
		// then
		expect( ret ).toEqual( false );
		expect( alertService.warningSet ).toHaveBeenCalled( );
		sut.windowClose( false );
		//
	} ) );
	//
	// validateNote, check against a common set of validation rules.
	//
	it('validateNote should succeed ...', fakeAsync( ( ) => {
		//
		const data = mockDatum[0];
		const errMsgs: Message[] = sut.validateNote(data, false);
		expect( errMsgs.length ).toEqual( 0 );
		windowCleanup( );
		//
	} ) );
	//
	it( 'validateNote: should handle a validation Incident Note Id required error...', ( ) => {
		// given
		// IncidentNoteId, NoteTypeId, NoteTypeShortDesc, Note, CreatedDate, IsChanged
		const incidentnoteidBad: any = undefined;
		const model: IIncidentNote = new IncidentNote(
			5, 5, 'i 5', '-note-', new Date( '2000-01-01T00:00:00' ), false );
		model.IncidentNoteId = incidentnoteidBad;
		// when
		const ret: Message[] = sut.validateNote( model, true );
		// then (is required.)
		expect( ret.length ).toEqual( 1 );
		expect( ret[0].message.indexOf( 'required' ) !== -1 ).toBe( true );
	} );
	//
	it( 'validateNote: should handle a validation Note Type Id required error...', ( ) => {
		// given
		const notetypeidBad: any = undefined;
		const model: IIncidentNote = new IncidentNote(
			5, 5, 'i 5', '-note-', new Date( '2000-01-01T00:00:00' ), false );
		model.NoteTypeId = notetypeidBad;
		// when
		const ret: Message[] = sut.validateNote( model, true );
		// then (is required.)
		expect( ret.length ).toEqual( 1 );
		expect( ret[0].message.indexOf( 'required' ) !== -1 ).toBe( true );
	} );
	//
	it( 'validateNote: should handle a validation Note Type Id too large error...', ( ) => {
		// given
		const notetypeidBad: number = 2147483648;
		const model: IIncidentNote = new IncidentNote(
			5, 5, 'i 5', '-note-', new Date( '2000-01-01T00:00:00' ), false );
		model.NoteTypeId = notetypeidBad;
		// when
		const ret: Message[] = sut.validateNote( model, true );
		// then (is too large, over: #)
		expect( ret.length ).toEqual( 1 );
		expect( ret[0].message.indexOf( 'too large' ) !== -1 ).toBe( true );
	} );
	//
	it( 'validateNote: should handle a validation Note required error...', ( ) => {
		// given
		const noteBad: string = '';
		const model: IIncidentNote = new IncidentNote(
			5, 5, 'i 5', '-note-', new Date( '2000-01-01T00:00:00' ), false );
		model.Note = noteBad;
		// when
		const ret: Message[] = sut.validateNote( model, true );
		// then (is required.)
		expect( ret.length ).toEqual( 1 );
		expect( ret[0].message.indexOf( 'required' ) !== -1 ).toBe( true );
		console.log(
			'End of IncidentNoteDetailWindowComponent.\n' +
			'==================================================' );
	} );
	//
} );
// ===========================================================================
