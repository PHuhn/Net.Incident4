// ===========================================================================
import { ComponentFixture, TestBed, fakeAsync, tick, waitForAsync } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpResponse, HttpErrorResponse } from '@angular/common/http';
//
import { of, throwError } from 'rxjs';
//
import { ConfirmationService, SelectItem } from 'primeng/api';
import { APP_MODULE_PRIMENG } from '../../APP.MODULE-PRIMENG';
//
import { APP_GLOBAL_COMPONENTS } from '../../global/APP.GLOBAL';
import { APP_COMPONENTS } from '../../APP.COMPONENTS';
import { BaseCompService } from '../../global/base-comp/base-comp.service';
import { Message } from '../../global/alerts/message';
import { AlertLevel } from '../../global/alerts/alert-level.enum';
import { AlertsService } from '../../global/alerts/alerts.service';
import { ConsoleLogService } from '../../global/console-log/console-log.service';
import { ServicesService } from '../services/services.service';
import { NetworkIncidentService } from '../services/network-incident.service';
//
import { DetailWindowInput } from '../DetailWindowInput';
import { IIncident, Incident } from '../incident';
import { NetworkIncident } from '../network-incident';
import { NetworkLog } from '../network-log';
import { User } from '../user';
import { ServerData } from '../server-data';
import { SelectItemClass } from '../../global/primeng/select-item-class';
import { NetworkIncidentSave } from '../network-incident-save';
import { IncidentNote } from '../incident-note';
//
import { IncidentDetailWindowComponent } from './incident-detail-window.component';
import { LogLevel } from 'src/app/global/console-log/log-level.enum';
//
describe( 'IncidentDetailWindowComponent', ( ) => {
	let sut: IncidentDetailWindowComponent;
	let fixture: ComponentFixture<IncidentDetailWindowComponent>;
	let alertService: AlertsService;
	let baseService: BaseCompService;
	let consoleService: ConsoleLogService;
	let detailWindow: DetailWindowInput;
	//
	const networkIncidentServiceSpy = jasmine.createSpyObj(
		'NetworkIncidentService', [ 'validateIncident', 'validateNetworkLogs',
		'getModelById', 'createModel', 'updateModel' ]);
	const servicesServiceSpy = jasmine.createSpyObj(
		'ServicesService', ['getPing', 'getWhoIs']);
	const whoisMockData_17_142_171_7: string =
		`[Querying whois.arin.net]\r\n[whois.arin.net]\r\n\r\n#\r\nNetRange:       17.0.0.0 - 17.255.255.255\r\nCIDR:           17.0.0.0/8\r\nNetName:        APPLE-WWNET\r\nOrganization:   Apple Inc. (APPLEC-1-Z)\r\n \r\nOrgName:        Apple Inc.\r\nOrgId:          APPLEC-1-Z\r\nOrgAbuseHandle: APPLE11-ARIN\r\nOrgAbuseName:   Apple Abuse\r\nOrgAbusePhone:  +1-408-974-7777 \r\nOrgAbuseEmail:  abuse@apple.com\r\n#`;
	//
	const testDate: Date = new Date('2000-01-01T00:00:00-05:00');
	//
	const ip: string = '192.199.1.1';
	const startDate: Date = new Date('2018-03-11T02:00:00-05:00');
	const endDate: Date = new Date('2018-11-04T02:00:00-05:00');
	const server = new ServerData(
		1, 1, 'NSG', 'Srv 1', 'Members Web-site',
		'Web-site', 'Web-site address: www.nsg.com',
		'We are in Michigan, USA.', 'Phil Huhn', 'Phil', 'PhilHuhn@yahoo.com',
		'EST (UTC-5)', true,  'EDT (UTC-4)', startDate, endDate
	);
	const user: User = new User('e0-01','U1','U','N','U N','U','UN1@yahoo.com',true,'734-555-1212', true,1,
		[new SelectItemClass('srv 1','Server 1'), new SelectItemClass('srv 2','Server 2')],'srv 1', server, ['admin']);
	//
	const mockData: Incident =
		new Incident( 4,1,ip,'arin.net','i-4-net','a@1.com','',false,false,false,'i 4',testDate );
	//
	const mockNetLogs = [
		new NetworkLog( 1,1,null,'192.1',new Date( '2018-02-27T00:00:00-05:00' ),'Log 1',1, 'SQL', false ),
		new NetworkLog( 2,1,null,'192.2',new Date( '2018-02-27T00:00:00-05:00' ),'Log 2',1, 'SQL', false ),
		new NetworkLog( 3,1,null,'192.3',new Date( '2018-02-27T00:00:00-05:00' ),'Log 3',1, 'SQL', false ),
		new NetworkLog( 4,1,null,ip,new Date( '2018-02-27T00:00:00-05:00' ),'Log 4',1, 'SQL', true ),
		new NetworkLog( 5,1,null,'192.5',new Date( '2018-02-27T00:00:00-05:00' ),'Log 5',1, 'SQL', false ),
		new NetworkLog( 6,1,null,'192.5',new Date( '2018-02-27T00:00:00-05:00' ),'Log 6',1, 'SQL', false )
	];
	const mockNICs: SelectItem[] = [
		{ value: 'afrinic.net' }, { value: 'apnic.net' },
		{ value: 'arin.net' }, { value: 'hostwinds.com' },
		{ value: 'jpnic.net' }, { value: 'lacnic.net' },
		{ value: 'nic.br' }, { value: 'other' },
		{ value: 'ripe.net' }, { value: 'twnic.net' },
		{ value: 'unknown' }
	];
	//
	beforeEach( waitForAsync( ( ) => {
		TestBed.configureTestingModule(  {
			imports: [
				FormsModule,
				APP_MODULE_PRIMENG,
				BrowserAnimationsModule
			],
			declarations: [
				APP_GLOBAL_COMPONENTS,
				APP_COMPONENTS
			],
			providers: [
				BaseCompService,
				AlertsService,
				ConsoleLogService,
				ConfirmationService,
				{ provide: ServicesService, useValue: servicesServiceSpy },
				{ provide: NetworkIncidentService, useValue: networkIncidentServiceSpy },
			]
		});
		// Setup injected pre service for each test
		baseService = TestBed.inject( BaseCompService );
		alertService = baseService._alerts;
		consoleService = baseService._console;
		// netIncidentService  = TestBed.get( NetworkIncidentService );
		TestBed.compileComponents( );
	}));
	//
	beforeEach( fakeAsync( ( ) => {
		fixture = TestBed.createComponent( IncidentDetailWindowComponent );
		sut = fixture.componentInstance;
		//
		const response: NetworkIncident = newNetworkIncident( mockData );
		// netIncidentService.mockGet = response;
		networkIncidentServiceSpy.getModelById.and.returnValue( of( response ) );
		// supply the input data
		detailWindow = new DetailWindowInput( user, mockData );
		sut.detailWindowInput = detailWindow;
		sut.displayWin = true;
		//
		tickFakeWait( 200 );
		tickFakeWait( 200 );
		tickFakeWait( 200 );
		tickFakeWait( 200 );
		tickFakeWait( 1 );
	} ) );
	//
	function newNetworkIncident( incident: Incident ): NetworkIncident {
		const _ni = new NetworkIncident( );
		_ni.incident = JSON.parse( JSON.stringify( incident ) );
		_ni.incidentNotes = [];
		_ni.deletedNotes = [];
		_ni.networkLogs = JSON.parse( JSON.stringify( mockNetLogs ) );
		_ni.deletedLogs = [];
		_ni.typeEmailTemplates = [];
		_ni.NICs = mockNICs;
		_ni.incidentTypes = [];
		_ni.noteTypes = [];
		_ni.message = '';
		_ni.user = undefined;
		return _ni;
	}
	/*
	** Cleanup so no dialog window will still be open
	*/
	function windowCleanup( ) {
		sut.detailWindow = undefined;
		sut.displayWindow = false;
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
	// Component is instantiated
	//
	it( 'should be created ...', fakeAsync( ( ) => {
		console.log(
			'===================================\n' +
			'IncidentDetailWindowComponent should create ...' );
		// given / when / then
		expect( sut ).toBeTruthy( );
		windowCleanup( );
	} ) );
	// get detailWindowInput(): DetailWindowInput { return this.detailWindow; }
	it( 'detailWindowInput: should return input property ...', fakeAsync( ( ) => {
		// given / when
		const _detailWindowInput: DetailWindowInput = sut.detailWindowInput;
		// then
		expect( _detailWindowInput.user ).toEqual( user );
		expect( _detailWindowInput.incident ).toEqual( mockData );
		windowCleanup( );
	} ) );
	//
	// Verify data is transmitted to model via @input statement
	//
	it( 'should get the mock data...', fakeAsync( ( ) => {
		//
		// given / when
		tickFakeWait( 1 );
		fixture.whenStable().then(() => {
			// then
			expect( sut.networkIncident.incident.IncidentId ).toEqual( mockData.IncidentId );
			expect( sut.networkIncident.incident.ServerId ).toEqual( mockData.ServerId );
			expect( sut.networkIncident.incident.NIC ).toEqual( mockData.NIC );
			expect( sut.networkIncident.incident.NetworkName ).toEqual( mockData.NetworkName );
			expect( sut.networkIncident.incident.AbuseEmailAddress ).toEqual( mockData.AbuseEmailAddress );
			expect( sut.networkIncident.incident.ISPTicketNumber ).toEqual( mockData.ISPTicketNumber );
			expect( sut.networkIncident.incident.Mailed ).toEqual( mockData.Mailed );
			expect( sut.networkIncident.incident.Closed ).toEqual( mockData.Closed );
			expect( sut.networkIncident.incident.Special ).toEqual( mockData.Special );
			expect( sut.networkIncident.incident.Notes ).toEqual( mockData.Notes );
			windowCleanup( );
		});
		//
	} ) );
	//
	// initialize( model: IIncident ): void
	//
	it('initialize: should set undefined string fields ...', fakeAsync( ( ) => {
		//
		// given
		const incident: IIncident =
			new Incident( 76, 1, undefined, undefined, undefined, undefined, undefined, false, false, false, undefined, new Date( '2021-06-06' ) );
		// when
		sut.initialize( incident );
		// then
		expect( incident.IPAddress ).toEqual( '' );
		expect( incident.NIC ).toEqual( '' );
		expect( incident.NetworkName ).toEqual( '' );
		expect( incident.AbuseEmailAddress ).toEqual( '' );
		expect( incident.ISPTicketNumber ).toEqual( '' );
		expect( incident.Notes ).toEqual( '' );
		windowCleanup( );
		//
	} ) );
	//
	// validate( ): boolean
	//
	it('validate: should display alert when invalid ...', fakeAsync( ( ) => {
		// given
		const save = new NetworkIncidentSave();
		save.incident =
			new Incident( 76, 1, undefined, undefined, undefined, undefined, undefined, false, false, false, undefined, new Date( '2021-06-06' ) );
		save.incidentNotes = [];
		save.deletedNotes = [];
		save.networkLogs = [];
		save.deletedLogs = [];
		save.user = { ... user };
		save.message = '';
		sut.networkIncidentSave = save;
		spyOn( alertService, 'warningSet' );
		networkIncidentServiceSpy.validateIncident.and.returnValue( [ new Message( 'id', AlertLevel.Warning, 'message' ) ] );
		networkIncidentServiceSpy.validateNetworkLogs.and.returnValue( [] );
		// when
		sut.validate( );
		// then
		expect( alertService.warningSet ).toHaveBeenCalled( );
		windowCleanup( );
		//
	} ) );
	//
	it('validate: should display alert when invalid netIncidentSave ...', fakeAsync( ( ) => {
		// given
		sut.networkIncidentSave = undefined;
		spyOn( alertService, 'warningSet' );
		// when
		sut.validate( );
		// then
		expect( alertService.warningSet ).toHaveBeenCalled( );
		windowCleanup( );
		//
	} ) );
	//
	// validateUser( errMsgs: Message[], model: IUser ): void
	//
	it('validateUser: should display alert when invalid ...', fakeAsync( ( ) => {
		// given
		const errMsgs: Message[] = [];
		const badUser = { ... user };
		badUser.UserName = '';
		badUser.UserNicName = '';
		badUser.Email = '';
		// when
		sut.validateUser( errMsgs, badUser );
		// then
		expect( errMsgs.length ).toEqual( 3 );
		windowCleanup( );
		//
	} ) );
	/*
	** ipChanged( ipAddress: string ): void
	*/
	it('ipChanged: should lookup the new ip address ...', fakeAsync( ( ) => {
		// given
		const testData: Incident = mockData.Clone();
		sut.networkIncident = newNetworkIncident( testData );
		servicesServiceSpy.getWhoIs.and.returnValue( of( whoisMockData_17_142_171_7 ) );
		// when
		sut.ipChanged( '17.142.171.7' );
		// then
		tickFakeWait( 1 );
		expect( sut.networkIncident.incident.NIC ).toEqual( 'arin.net' );
		expect( sut.networkIncident.incident.AbuseEmailAddress ).toEqual( 'abuse@apple.com' );
		expect( sut.networkIncident.incident.NetworkName ).toEqual( 'APPLE-WWNET' );
		windowCleanup( );	// window launched in beforeEach
		tickFakeWait( 1000 );
		//
	} ) );
	//
	it('ipChanged: should save whois record if lookup could not extract IP address ...', fakeAsync( ( ) => {
		// given
		const whoisResponse: string =
			`[Querying whois.net]\r\n[whois.net]\r\n\r\n#\r\nNot found\r\n#`;
		const testData: Incident = mockData.Clone();
		const noteCount: number = sut.networkIncident.incidentNotes.length
		sut.networkIncident = newNetworkIncident( testData );
		servicesServiceSpy.getWhoIs.and.returnValue( of( whoisResponse ) );
		// when
		sut.ipChanged( '17.142.171.7' );
		// then
		tickFakeWait( 1 );
		expect( sut.networkIncident.incident.NIC ).toEqual( 'other' );
		expect( sut.networkIncident.incidentNotes.length ).toEqual( noteCount + 1 );
		windowCleanup( );	// window launched in beforeEach
		tickFakeWait( 1000 );
		//
	} ) );
	//
	it('ipChanged: should fail if invalid ip address ...', fakeAsync( ( ) => {
		// given
		sut.networkIncident = undefined;
		spyOn( alertService, 'warningSet' );
		// when
		sut.ipChanged( '17.142.171.7' );
		// then
		expect( alertService.warningSet ).toHaveBeenCalled( );
		windowCleanup( );	// window launched in beforeEach
		//
	} ) );
	//
	it('ipChanged: should handle http whois error ...', fakeAsync( ( ) => {
		// given
		const testData: Incident = mockData.Clone();
		sut.networkIncident = newNetworkIncident( testData );
		const resp: HttpErrorResponse = new HttpErrorResponse({
			error: {}, status: 500, url: 'http://localhost', statusText: 'Bad Request' });
		servicesServiceSpy.getWhoIs.and.returnValue( throwError( ( ) => resp ) );
		spyOn( alertService, 'setWhereWhatError' );
		// when
		sut.ipChanged( '17.142.171.7' );
		// then
		tickFakeWait( 10 );
		expect( alertService.setWhereWhatError ).toHaveBeenCalled( );
		windowCleanup( );	// window launched in beforeEach
		tickFakeWait( 1000 );
		//
	} ) );
	//
	it('ipChanged: should handle no whois data ...', fakeAsync( ( ) => {
		// given
		const testData: Incident = mockData.Clone();
		sut.networkIncident = newNetworkIncident( testData );
		const resp: HttpErrorResponse = new HttpErrorResponse({
			error: {}, status: 500, url: 'http://localhost', statusText: 'Bad Request' });
		servicesServiceSpy.getWhoIs.and.returnValue( of( '' ) );
		spyOn( alertService, 'setWhereWhatError' );
		// when
		sut.ipChanged( '17.142.171.7' );
		// then
		tickFakeWait( 10 );
		expect( alertService.setWhereWhatError ).toHaveBeenCalled( );
		windowCleanup( );	// window launched in beforeEach
		tickFakeWait( 1000 );
		//
	} ) );
	/*
	** newNoteId(): number
	*/
	it('newNoteId: should create new negative id ...', fakeAsync( ( ) => {
		// given / when
		const ret: number = sut.newNoteId( );
		// then
		expect( ret ).toEqual( -2 );
		windowCleanup( );	// window launched in beforeEach
		//
	} ) );
	//
	it('newNoteId: should create new negative id again ...', fakeAsync( ( ) => {
		// given
		sut.networkIncident.incidentNotes.push( new IncidentNote( -2, 1, 'ping', 'ping', new Date(), true ) );
		// when
		const ret: number = sut.newNoteId( );
		// then
		expect( ret ).toEqual( -3 );
		windowCleanup( );	// window launched in beforeEach
		sut.networkIncident.incidentNotes = [];
		//
	} ) );
	/*
	** getNetIncident( incidentId: number, serverId: number )
	*/
	it( 'getNetIncident: should handle an error ...', fakeAsync( () => {
		// given
		const response = new HttpErrorResponse(
			{ error: {}, status: 500, url: 'http://localhost', statusText: 'Fake Error' });
		networkIncidentServiceSpy.getModelById.and.returnValue( throwError( ( ) => response ) );
		const incId: number = 0;
		const srvId: number = 1;
		spyOn( alertService, 'setWhereWhatError' );
		// when
		sut.getNetIncident( incId, srvId ); // close window
		// then
		expect( alertService.setWhereWhatError ).toHaveBeenCalled( );
		windowCleanup( );
		//
	}));
	/*
	** Simulate a button clicked, call windowClose directly with the default data.
	*/
	it('should update class when updateItem called ...', fakeAsync( ( ) => {
		// given
		const response = new HttpResponse(
				{ status: 201, statusText: 'OK' } );
		networkIncidentServiceSpy.updateModel.and.returnValue( of( response ) );
		networkIncidentServiceSpy.validateIncident.and.returnValue( [] );
		networkIncidentServiceSpy.validateNetworkLogs.and.returnValue( [] );
		sut.emitClose.subscribe( saved => {
			// when / then
			sut.displayWin = false;
			expect( saved ).toBe( true );
		} );
		sut.windowClose( true );
		windowCleanup( );
		//
	} ) );
	//
	it( 'updateItem: should handle an error ...', fakeAsync( () => {
		// given
		const response = new HttpErrorResponse(
			{ error: {}, status: 500, url: 'http://localhost', statusText: 'Fake Error' });
		networkIncidentServiceSpy.updateModel.and.returnValue( throwError( ( ) => response ) );
		const incId: number = 0;
		const srvId: number = 1;
		spyOn( alertService, 'setWhereWhatError' );
		// when
		sut.updateItem( false ); // close window
		// then
		expect( alertService.setWhereWhatError ).toHaveBeenCalled( );
		windowCleanup( );
		//
	}));
	//
	// Simulate a button clicked, call directly windowClose for createItem
	//
	it('should create NetworkIncident class when createItem called ...', fakeAsync( () => {
		// given
		const id = 0;
		const empty: Incident = new Incident( 0,1,'','','','','',false,false,false,'',testDate );
		sut.detailWindowInput = new DetailWindowInput( user, empty );
		//
		const createIncident = new Incident(
			id, // IncidentId: number,
			1, // ServerId: number,
			'10.1.1.1',  // IPAddress: string,
			'ripe.net', // NIC: string,
			'ru.net', // NetworkName: string,
			'abuse@ru.net', // AbuseEmailAddress: string,
			'', // ISPTicketNumber: string,
			false, // Mailed: boolean,
			false, // Closed: boolean,
			false, // Special: boolean,
			'Create', // Notes: string,
			new Date(Date.now()) // CreatedDate: Date,
		);
		const response: NetworkIncident = newNetworkIncident( createIncident );
		networkIncidentServiceSpy.getModelById.and.returnValue( of( response ) );
		networkIncidentServiceSpy.createModel.and.returnValue( of( response ) );
		networkIncidentServiceSpy.validateIncident.and.returnValue( [] );
		networkIncidentServiceSpy.validateNetworkLogs.and.returnValue( [] );
		createIncident.IncidentId = 0;
		sut.networkIncident.incident = createIncident;
		// when
		sut.createItem( false );
		tickFakeWait( 1 );
		// then
		sut.windowClose( true );
		tickFakeWait( 1 );
		//
		expect( sut.networkIncident.incident.IncidentId ).toBe( id );
		sut.displayWin = false;
		windowCleanup( );
		//
	} ) );
	//
	it( 'createItem: should handle an error on create...', fakeAsync( () => {
		// given
		const response = new HttpResponse( { status: 500, statusText: 'Fake Error' } );
		sut.networkIncidentSave = new NetworkIncidentSave( );
		networkIncidentServiceSpy.createModel.and.returnValue( throwError( ( ) => response ) );
		spyOn( alertService, 'setWhereWhatError' );
		// when
		sut.createItem( false ); // close window
		// then
		expect( alertService.setWhereWhatError ).toHaveBeenCalled( );
		windowCleanup( );
		//
	}));
	//
	// Simulate a cancel button clicked, call directly windowClose
	//
	it('should emit when canceled ...', fakeAsync( () => {
		// given
		sut.emitClose.subscribe( saved => {
			sut.displayWin = false;
			// then
			expect( saved ).toBe( false );
			//
			windowCleanup( );
			console.log(
				'End of IncidentDetailWindowComponent.spec\n' +
				'===================================' );
		} );
		// when
		sut.windowClose( false );
	} ) );
	//
} );
// ===========================================================================
