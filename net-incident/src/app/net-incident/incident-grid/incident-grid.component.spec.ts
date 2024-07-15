// ===========================================================================
import { ComponentFixture, TestBed, fakeAsync, tick, waitForAsync } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { of, throwError } from 'rxjs';
import { By } from '@angular/platform-browser';
import { HttpResponse } from '@angular/common/http';
//
import { LazyLoadMeta, FilterMetadata, ConfirmationService, Confirmation, SelectItem } from 'primeng/api';
import { APP_MODULE_PRIMENG } from '../../APP.MODULE-PRIMENG';
//
import { APP_COMPONENTS } from '../../APP.COMPONENTS';
import { APP_GLOBAL_COMPONENTS } from '../../global/APP.GLOBAL';
import { AlertsService } from '../../global/alerts/alerts.service';
import { ConsoleLogService } from '../../global/console-log/console-log.service';
import { BaseCompService } from '../../global/base-comp/base-comp.service';
import { ServicesService } from '../services/services.service';
import { UserService } from '../../net-incident/services/user.service';
import { IncidentService } from '../services/incident.service';
import { NetworkIncidentService } from '../services/network-incident.service';
//
import { DetailWindowInput } from '../DetailWindowInput';
import { User } from '../user';
import { ServerData } from '../server-data';
import { SelectItemClass } from '../../global/primeng/select-item-class';
import { IIncident, Incident } from '../incident';
import { NetworkLog } from '../network-log';
import { NetworkIncident } from '../network-incident';
import { ILazyResults } from '../../global/base-srvc/ilazy-results';
import { IncidentGridComponent } from './incident-grid.component';
import { LazyLoadingMock } from '../services/mocks/lazy-loading.mock';
//
import { AppComponent } from '../../app.component';
import { Security } from '../security';
//
describe( 'IncidentGridComponent', ( ) => {
	let sut: IncidentGridComponent;
	let fixture: ComponentFixture<IncidentGridComponent>;
	const lazyLoading: LazyLoadingMock = new LazyLoadingMock();
	let alertService: AlertsService;
	let baseService: BaseCompService;
	let consoleService: ConsoleLogService;
	const selectionWindowTitleSelector: string =
		'#serverSelectionWindow > div > div > div > span.p-dialog-title > p-header';
	// document.querySelector('#serverSelectionWindow > div > div > div > span.p-dialog-title > p-header')
	const userServiceSpy = jasmine.createSpyObj('UserService',
			['emptyUser', 'getModelById']);
	const servicesServiceSpy = jasmine.createSpyObj('ServicesService',
		['getText']);
	const incidentServiceSpy = jasmine.createSpyObj(
		'IncidentService', ['emptyIncident', 'getModelLazy', 'deleteModel']);
	let confirmService: ConfirmationService;
	const networkIncidentServiceSpy = jasmine.createSpyObj(
		'NetworkIncidentService', ['validateIncident', 'validateNetworkLog',
		'validateNetworkLogs', 'getModelById', 'createIncident', 'updateIncident']);
	const windowTitleSelector: string =
		'app-incident-detail-window > p-dialog > div > div.p-dialog-titlebar > span > p-header > div';
	const testDate: Date = new Date('2000-01-01T00:00:00');
	const startDate: Date = new Date('2018-03-11T02:00:00');
	const endDate: Date = new Date('2018-11-04T02:00:00');
	const server = new ServerData(
		1, 1, 'NSG', 'Srv 1', 'Members Web-site',
		'Web-site', 'Web-site address: www.nsg.com',
		'We are in Michigan, USA.', 'Phil Huhn', 'Phil', 'PhilHuhn@yahoo.com',
		'EST (UTC-5)', true,  'EDT (UTC-4)', startDate, endDate
	);
	const selectItemsWindow: SelectItem[] = [new SelectItemClass(1, 'srv 1'), new SelectItemClass( 2, 'srv 2')];
	const user: User = new User('e0-01','U1','U','N','U N','U','UN1@yahoo.com',true,'734-555-1212', true,1,
		selectItemsWindow, 'srv 1', server,['admin']);
	//
	const mockDatum = [
		new Incident( 1,1,'193.1','arin.net','i 1','i1@1.com','',true,true,true,'',testDate ),
		new Incident( 2,1,'193.2','ripe.net','i 2','i2@2.com','',false,false,false,'Note 2',testDate ),
		new Incident( 3,1,'193.3','ripe.net','i 3','i3@3.com','',true,true,true,'',testDate ),
		new Incident( 4,1,'193.4','ripe.net','i 4','i4@4.com','',false,false,false,'Note 4',testDate ),
		new Incident( 5,1,'193.5','ripe.net','i 5','i5@5.com','',true,true,true,'',testDate ),
		new Incident( 6,2,'193.6','arin.net','i 6','i6@6.com','',false,false,false,'',testDate ),
		new Incident( 7,1,'193.7','arin.net','i 7','i7@7.com','',false,false,false,'',testDate )
	];
	const mockLogs = [
		new NetworkLog( 1,1,null,'193.1',new Date( '2018-02-27T00:00:00' ),'Log 1',1, 'SQL', false ),
		new NetworkLog( 2,1,2,'193.2',new Date( '2018-02-27T00:00:00' ),'Log 2',1, 'SQL', false ),
		new NetworkLog( 3,1,null,'193.3',new Date( '2018-02-27T00:00:00' ),'Log 3',1, 'SQL', false ),
		new NetworkLog( 4,1,4,'193.4',new Date( '2018-02-27T00:00:00' ),'Log 4',1, 'SQL', false ),
		new NetworkLog( 5,1,null,'193.5',new Date( '2018-02-27T00:00:00' ),'Log 5',1, 'SQL', false ),
		new NetworkLog( 6,1,null,'193.5',new Date( '2018-02-27T00:00:00' ),'Log 6',1, 'SQL', false )
	];
	let pageData: ILazyResults<IIncident> = { results: [], totalRecords: 0, loadEvent: '', message: ''};
	//
	beforeEach( waitForAsync( ( ) => {
		TestBed.configureTestingModule( {
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
				{ provide: UserService, useValue: userServiceSpy },
				{ provide: ServicesService, useValue: servicesServiceSpy },
				{ provide: IncidentService, useValue: incidentServiceSpy },
				{ provide: NetworkIncidentService, useValue: networkIncidentServiceSpy },
			]
		});
		baseService = TestBed.inject( BaseCompService );
		alertService = baseService._alerts;
		consoleService = baseService._console;
		confirmService = baseService._confirmationService;
		TestBed.compileComponents( );
	}));
	//
	beforeEach( fakeAsync( ( ) => {
		// clone the array with slice( 0 )
		const page: ILazyResults<IIncident> = pageData;
		const temp: IIncident[] = mockDatum.filter( inc => inc.ServerId === 1 ).sort( ( n1: IIncident, n2: IIncident ) => (n1.IncidentId < n2.IncidentId ? 1: -1) );
		page.totalRecords = temp.length;
		page.results = temp.slice(0, 3);
		incidentServiceSpy.getModelLazy.and.returnValue( of( page ) );
		userServiceSpy.getModelById.and.returnValue(of( user ));
		AppComponent.securityManager = new Security( user );
		//
		fixture = TestBed.createComponent( IncidentGridComponent );
		sut = fixture.componentInstance;
		// push in the @Input
		sut.user = user;
		pageData = page;
		tickFakeWait( 1000 );
		tickFakeWait( 1000 );
	} ) );
	//
	function createEvent( page: number, size: number): LazyLoadMeta {
		const event: LazyLoadMeta = {'first':page, 'rows':size, 'sortField':'IncidentId','sortOrder':-1,
			'filters':{'ServerId':[{'value':1,'matchMode':'equals'}],'Mailed':[{'value':false,'matchMode':'equals'}],'Closed':[{'value':false,'matchMode':'equals'}],'Special':[{'value':false,'matchMode':'equals'}]},'globalFilter':null};
		return event;
	}
	//
	function newPage( ): ILazyResults<IIncident> {
		const event: LazyLoadMeta = createEvent( 0, 5);
		const page: ILazyResults<IIncident> =
				lazyLoading.LazyLoading2( [ ... mockDatum ], event );
		return page;
	}
	//
	function newNetworkIncident( incident: Incident ): NetworkIncident {
		const _ni = new NetworkIncident( );
		_ni.incident = incident.Clone();
		_ni.incidentNotes = [];
		_ni.deletedNotes = [];
		_ni.networkLogs = [ ... mockLogs ];
		_ni.networkLogs.forEach( (row) => {
			if( _ni.incident.IncidentId === row.IncidentId ) {
				row.Selected = true;
			} else {
				row.Selected = false;
			}
		});
		_ni.deletedLogs = [];
		_ni.typeEmailTemplates = [];
		_ni.NICs = [];
		_ni.incidentTypes = [];
		_ni.noteTypes = [];
		_ni.message = '';
		_ni.user = { ... user };
		return _ni;
	}
	/*
	** Cleanup so no dialog window will still be open
	*/
	function windowCleanup( ) {
		sut.windowDisplay = false;
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
	it( 'should be created ...', waitForAsync( () => {
		console.log(
			'===================================\n' +
			'IncidentGridComponent should create ...' );
		expect( sut ).toBeTruthy( );
	} ) );
	/*
	** loadIncidentsLazy( event: LazyLoadMeta )
	*/
	it('loadIncidentsLazy: should handle service error ...', fakeAsync(() => {
		// given
		const response = new HttpResponse( { status: 500, statusText: 'Fake Error' } );
		incidentServiceSpy.getModelLazy.and.returnValue( throwError( ( ) => response ) );
		const event: LazyLoadMeta = createEvent( 1, 5);
		spyOn( alertService, 'setWhereWhatError' );
		// when
		sut.loadIncidentsLazy( event );
		// then
		tickFakeWait( 1 ); // wait 1 second task to get done
		expect( alertService.setWhereWhatError ).toHaveBeenCalled( );
		expect( sut.loading ).toEqual( false)
		// cleanup
		windowCleanup( );
		//
	}));
	//
	// getIncidents( ) : Observable<IIncident[]>
	//
	it('should initialize with all data...', fakeAsync( () => {
		// given/when/then
		// console.warn( `should initialize with all data ${sut.incidents.length} ...` );
		expect( sut.incidents.length ).toBe( 3 );
		//
		expect( sut.incidents[ 0 ].IncidentId ).toEqual( 7 );
		expect( sut.incidents[ 1 ].IncidentId ).toEqual( 5 );
		expect( sut.incidents[ 2 ].IncidentId ).toEqual( 4 );
		//
	}));
	/*
	** test of getUserServer, succeed
	*/
	it('getUserServer select a different server ...', fakeAsync( ( ) => {
		// given
		// console.warn( `getUserServer select a different server, loading: ${sut.loading}` );
		let serverShortName = selectItemsWindow[1].label;
		// setup response to _user.getUserServer service call
		const user2 = { ... user };
		user2.Server.ServerName = 'Server 2';
		user2.Server.ServerShortName = serverShortName;
		user2.ServerShortName = serverShortName;
		userServiceSpy.getModelById.and.returnValue(of( user2 ));
		tickFakeWait(10); // wait 1 second task to get done
		// when
		sut.getUserServer( sut.user.UserName, serverShortName );
		// then
		tickFakeWait(1000); // wait 1 second task to get done
		// tickFakeWait(1000); // wait 1 second task to get done
		//
		expect( sut.user.ServerShortName ).toEqual( serverShortName );
		// reset values
		serverShortName = selectItemsWindow[0].label;
		sut.user.Server.ServerName = 'Server 1';
		sut.user.Server.ServerShortName = serverShortName;
		sut.user.ServerShortName = serverShortName;
		//
	} ) );
	//
	it('getUserServer select if short name not set ...', fakeAsync( ( ) => {
		// given
		// console.warn( `getUserServer select a different server, loading: ${sut.loading}` );
		let serverShortName = selectItemsWindow[1].label;
		// setup response to _user.getUserServer service call
		const user2 = { ... user };
		user2.Server.ServerName = 'Server 2';
		user2.Server.ServerShortName = serverShortName;
		user2.ServerShortName = '';
		userServiceSpy.getModelById.and.returnValue(of( user2 ));
		tickFakeWait(10); // wait 1 second task to get done
		// when
		sut.getUserServer( sut.user.UserName, serverShortName );
		// then
		tickFakeWait(1000); // wait 1 second task to get done
		// tickFakeWait(1000); // wait 1 second task to get done
		//
		expect( sut.user.ServerShortName ).toEqual( 'srv 1' );
		expect( consoleService.lastMessage ).toEqual( 'Info: Server-Selection-Component.displayWin setter: win: true' );
		tickFakeWait(10);
		//
	} ) );
	//
	it('getUserServer: should fail when service throws an error ...', fakeAsync( ( ) => {
		// given
		const response = new HttpResponse( { status: 500, statusText: 'Fake Error' } );
		userServiceSpy.getModelById.and.returnValue(throwError( ( ) => response ));
		spyOn( alertService, 'setWhereWhatError' );
		// when
		sut.getUserServer( sut.user.UserName, '' );
		// then
		tickFakeWait( 1 );
		expect( alertService.setWhereWhatError ).toHaveBeenCalled( );
	}));
	/*
	** onServerSelected( event: any )
	** test of getUserServer, succeed
	*/
	it('onServerSelected select a different server ...', fakeAsync( ( ) => {
		// given
		// console.warn( 'onServerSelected select a different server ...' );
		let serverShortName = 'srv 2';
		// setup response to _user.getUserServer service call
		const user2 = { ... user };
		user2.Server.ServerName = 'Server 2';
		user2.Server.ServerShortName = serverShortName;
		user2.ServerShortName = serverShortName;
		userServiceSpy.getModelById.and.returnValue(of( user2 ));
		// when
		sut.onServerSelected( serverShortName );
		// then
		tickFakeWait(1000); // wait 1 second task to get done
		tickFakeWait(1000); // wait 1 second task to get done
		//
		expect( sut.user.ServerShortName ).toEqual( serverShortName );
		// reset values
		serverShortName = selectItemsWindow[0].label;
		sut.user.Server.ServerName = 'Server 1';
		sut.user.Server.ServerShortName = serverShortName;
		sut.user.ServerShortName = serverShortName;
		//
	} ) );
	/*
	** onChangeServer( ), launch server selection window
	*/
	it('onChangeServer: should launch server selection window ...', fakeAsync( () => {
		// given / when
		// console.warn( 'onChangeServer: should launch server selection window ...' );
		sut.onChangeServer( 'testing' );
		// then
		tickFakeWait(10); // wait 100 second task to get done
		//
		expect( sut.displayServersWindow ).toEqual( true );
		const title: HTMLDivElement = fixture.debugElement.query(By.css(
			selectionWindowTitleSelector )).nativeElement;
		expect( title.innerText.trim( ) ).toEqual( `Select a server` );
		tickFakeWait(10);
	}));
	/*
	** addItemClicked( )
	*/
	it('addItemClicked: should launch detail window ...', fakeAsync( () => {
		// given
		// console.warn( 'addItemClicked: should launch detail window ...' );
		const inc = new Incident( 0,0,'','','','','',false,false,false,'',testDate );
		incidentServiceSpy.emptyIncident.and.returnValue(of( inc ));
		const response: NetworkIncident = newNetworkIncident(
			inc
		);
		const id = response.incident.IncidentId; // for title
		const ip = response.incident.IPAddress;
		networkIncidentServiceSpy.getModelById.and.returnValue( of( response ) );
		// when
		sut.addItemClicked( );
		// then
		tickFakeWait(1000); // wait 1 second task to get done
		tickFakeWait(1000); // wait 1 second task to get done
		//
		expect( sut.windowDisplay ).toEqual( true );
		const title: HTMLDivElement = fixture.debugElement.query(By.css(
			'#IncidentDetailWindowHeader' )).nativeElement;
		expect( title.innerText.trim( ) ).toEqual( `Incident Detail: 0, IP Address:` );
		windowCleanup( );
	}));
	//
	it('addItemClicked: should fail to launch create window when security fails ...', fakeAsync( ( ) => {
		// given
		// console.warn( 'addItemClicked: should fail to launch create window when security fails ...' );
		const incident: Incident = pageData.results[ 2 ].Clone();
		AppComponent.securityManager = new Security( undefined );
		spyOn( alertService, 'setWhereWhatWarning' );
		// when
		sut.addItemClicked( );
		// then
		tickFakeWait( 1 );
		expect( alertService.setWhereWhatWarning ).toHaveBeenCalled( );
	}));
	/*
	** editItemClicked( item: Incident )
	*/
	it('editItemClicked: should launch detail window ...', fakeAsync( ( ) => {
		// given
		// console.warn( 'editItemClicked: should launch detail window ...' );
		const incident: Incident = pageData.results[ 1 ].Clone();
		const response: NetworkIncident = newNetworkIncident( incident );
		const id = response.incident.IncidentId; // for title
		const ip = response.incident.IPAddress;
		networkIncidentServiceSpy.getModelById.and.returnValue(of( response ));
		// when
		sut.editItemClicked( incident );
		// then
		expect( sut.windowDisplay ).toEqual( true );
		tickFakeWait(1000); // wait 1 second task to get done
		tickFakeWait(1000); // wait 1 second task to get done
		//
		const title: HTMLDivElement = fixture.debugElement.query(By.css(
			'#IncidentDetailWindowHeader' )).nativeElement;
		expect( title.innerText.trim( ) ).toEqual( `Incident Detail: ${id}, IP Address: ${ip}` );
		windowCleanup( );
	}));
	//
	it('editItemClicked: should fail to launch detail window when security fails ...', fakeAsync( ( ) => {
		// given
		// console.warn( 'editItemClicked: should fail to launch detail window when security fails ...' );
		const incident: Incident = pageData.results[ 2 ].Clone();
		AppComponent.securityManager = new Security( undefined );
		spyOn( alertService, 'setWhereWhatWarning' );
		// when
		sut.editItemClicked( incident );
		// then
		tickFakeWait( 1 );
		expect( alertService.setWhereWhatWarning ).toHaveBeenCalled( );
	}));
	/*
	** deleteItem( ) :boolean
	*/
	it( 'deleteItem: should handle a service error ...', fakeAsync( () => {
		// given
		// console.warn( 'deleteItem: should handle an error ...' );
		const response = new HttpResponse( { status: 500, statusText: 'Fake Error' } );
		incidentServiceSpy.deleteModel.and.returnValue( throwError( ( ) => response ) );
		const incident: Incident = pageData.results[ 1 ].Clone();
		const id = incident.IncidentId;
		spyOn( alertService, 'setWhereWhatError' );
		// when
		sut.deleteItem( id );
		// then
		expect( alertService.setWhereWhatError ).toHaveBeenCalled( );
		//
	}));
	/*
	** deleteItemClicked( item: Incident ) :boolean
	*/
	it('deleteItemClicked: should delete row when event called and OK is clicked...', fakeAsync(() => {
		// given
		// console.warn( 'deleteItemClicked: should delete row when event called and OK is clicked...' );
		const delRow: Incident = pageData.results[ 2 ].Clone();
		const delId: number = delRow.IncidentId;
		const expected: number = sut.incidents.length - 1;
		incidentServiceSpy.deleteModel.and.returnValue(of( 1 ));
		spyOn(confirmService, 'confirm').and.callFake(
			(confirmation: Confirmation) => {
				return confirmation.accept();
			});
		// when
		const ret: boolean = sut.deleteItemClicked( delRow );
		// then
		expect( ret ).toEqual( false );
		tickFakeWait( 1000 ); // give it small amount of time
		expect( sut.incidents.length ).toBe( expected );
		windowCleanup( );
		//
	}));
	//
	it('deleteItemClicked: should fail to delete when security fails ...', fakeAsync( ( ) => {
		// given
		// console.warn( 'deleteItemClicked: should fail to delete when security fails ...' );
		const incident: Incident = pageData.results[ 1 ].Clone();
		AppComponent.securityManager = new Security( undefined );
		spyOn( alertService, 'setWhereWhatWarning' );
		// when
		sut.deleteItemClicked( incident );
		tickFakeWait( 1 );
		expect( alertService.setWhereWhatWarning ).toHaveBeenCalled( );
	}));
	//
	it('deleteItemClicked: should fail when security manager undefined ...', fakeAsync( ( ) => {
		// given
		const incident: Incident = pageData.results[ 1 ].Clone();
		AppComponent.securityManager = undefined;
		spyOn( alertService, 'setWhereWhatError' );
		// when
		sut.deleteItemClicked( incident );
		tickFakeWait( 1 );
		expect( alertService.setWhereWhatError ).toHaveBeenCalled( );
	}));
	/*
	** closeWin( saved: boolean )
	*/
	it('onClose: should set window display off ...', fakeAsync(() => {
		// given
		// console.warn( 'onClose: should set window display off ...' );
		const incident: Incident = pageData.results[ 1 ].Clone();
		const response: NetworkIncident = newNetworkIncident( incident );
		const id = response.incident.IncidentId; // for title
		const ip = response.incident.IPAddress;
		networkIncidentServiceSpy.getModelById.and.returnValue(of( response ));
		//
		sut.detailWindow = new DetailWindowInput( {... user }, sut.incidents[ 1 ] );
		sut.windowDisplay = true;
		tickFakeWait( 1000 ); // wait 1 second task to get done
		tickFakeWait( 1000 ); // wait 1 second task to get done
		// when
		sut.onClose( false );
		// then
		expect( sut.windowDisplay ).toEqual( false );
		expect( sut.detailWindow ).toBeUndefined( );
		// cleanup
		windowCleanup( );
		//
	}));
	//
	it('onClose: should refresh grid ...', fakeAsync(() => {
		// given
		const page = newPage( );
		incidentServiceSpy.getModelLazy.and.returnValue( of( page ) );
		sut.lastTableLazyLoadMeta = 
			{'first':0,'rows':5,'sortField':'IncidentId','sortOrder':-1,'filters':{'ServerId':[{'value':1,'matchMode':'equals'}],'Mailed':[{'value':false,'matchMode':'equals'}],'Closed':[{'value':false,'matchMode':'equals'}],'Special':[{'value':false,'matchMode':'equals'}]},'globalFilter':null} as LazyLoadMeta;
		//
		tickFakeWait( 1 ); // wait 1 second task to get done
		// when
		sut.onClose( true );
		// then
		tickFakeWait( 1 ); // wait 1 millisecond task to get done
		expect( sut.windowDisplay ).toEqual( false );
		expect( sut.detailWindow ).toBeUndefined( );
		// cleanup
		windowCleanup( );
		//
	}));
	//
	it('fixFieldNames: should translater NIC to NIC_Id ...', () => {
		const filters: { [s: string]: FilterMetadata[] } = {
			'ServerId': [{ value: 4, matchMode: 'equals' }],
			'Mailed': [{ value: true, matchMode: 'equals' }],
			'Closed': [{ value: null, matchMode: 'equals' }],
			'NIC': [{ value: 'ripe.net', matchMode: 'equals' }],
		};
		const actual = sut.fixFieldNames(filters);
		// then
		const keys: string[] = [];
		for( const item in actual ) {
			keys.push( item );
		}
		expect( keys.length ).toEqual( 3 );
		expect( keys.find( f => f === 'NIC_Id') ).toEqual('NIC_Id');
	});
	//
	it('subscribe: should understand next/error/complete ...', () => {
		const values = [1,2,3];
		of(values).subscribe({
			next: (val) => {
				console.log(`subscribe: next: ${val}`);
				expect(val).toEqual(values);
			},
			error: (error) => {
				fail();
				console.error(`subscribe: error: ${error}`);
			},
			complete: () => {
				console.info(`subscribe: complete`);
			}
		});
		console.log(
			'End of IncidentGridComponent ...\n' +
			'===================================' );
	});
	//
} );
// ===========================================================================
