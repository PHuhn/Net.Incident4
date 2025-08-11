// ===========================================================================
import { ComponentFixture, TestBed, fakeAsync, tick, waitForAsync } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { By } from '@angular/platform-browser';
//
import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { ConfirmationService, Confirmation } from 'primeng/api';
//
import { AlertsService } from '../../global/alerts/alerts.service';
import { BaseCompService } from '../../global/base-comp/base-comp.service';
import { ConsoleLogService } from '../../global/console-log/console-log.service';
import { Incident } from '../incident';
import { INetworkIncident, NetworkIncident } from '../network-incident';
import { INetworkLog, NetworkLog } from '../network-log';
import { NetworkLogGridComponent } from './network-log-grid.component';
import { TruncatePipe } from '../../global/truncate.pipe';
import { User } from '../user';
//
describe('NetworkLogGridComponent', () => {
	let sut: NetworkLogGridComponent;
	let fixture: ComponentFixture<NetworkLogGridComponent>;
	let alertService: AlertsService;
	let baseService: BaseCompService;
	let confirmService: ConfirmationService;
	let consoleService: ConsoleLogService;
	//
	const numRowsSelector = '#netLogTable > div > table > tbody > tr';
	const ipAddr: string = '192.169.1.1';
	//
	const inc: Incident = new Incident( 4,1,'','arin.net','PSG169',
		'dandy@psg.com','',false,false,false,'',new Date( '2018-04-01T01:00:00' ) );
	//
	const mockDatum = [
		new NetworkLog( 1,1,null,'192.169.101.1',new Date( '2018-02-27T00:00:00' ),'Log 1',1, 'SQL', false ),
		new NetworkLog( 2,1,null,'192.169.101.2',new Date( '2018-02-27T00:00:00' ),'Log 2',1, 'SQL', false ),
		new NetworkLog( 3,1,null,'192.169.101.3',new Date( '2018-02-27T00:00:00' ),'Log 3',1, 'SQL', false ),
		new NetworkLog( 4,1,null,'192.169.101.4',new Date( '2018-02-27T00:00:00' ),'Log 4',1, 'SQL', false ),
		new NetworkLog( 5,1,null,'192.169.101.5',new Date( '2018-02-27T00:00:00' ),'Log 5',1, 'SQL', false ),
		new NetworkLog( 6,1,null,'192.169.101.5',new Date( '2018-02-27T00:00:00' ),'Log 6',1, 'SQL', false )
	];
	//
	beforeEach( waitForAsync( ( ) => {
		TestBed.configureTestingModule(  {
			imports: [
				FormsModule,
				TableModule,
				ButtonModule,
				DialogModule,
				BrowserAnimationsModule
			],
			declarations: [
				NetworkLogGridComponent,
				TruncatePipe
			],
			providers: [
				BaseCompService,
				AlertsService,
				ConsoleLogService,
				ConfirmationService,
			]
		});
		// Setup injected pre service for each test
		baseService = TestBed.inject( BaseCompService );
		alertService = baseService._alerts;
		consoleService = baseService._console;
		confirmService = baseService._confirmationService;
		TestBed.compileComponents( );
	}));
	//
	beforeEach( ( ) => {
		//
		fixture = TestBed.createComponent( NetworkLogGridComponent );
		sut = fixture.componentInstance;
		//
	} );
	//
	function createNetworkIncident( ): INetworkIncident {
		const netInc = new NetworkIncident();
		netInc.incident = inc.Clone();
		netInc.deletedLogs = [];
		netInc.deletedNotes = [];
		netInc.incidentNotes = [];
		netInc.networkLogs = [ ... mockDatum ];
		netInc.networkLogs.forEach( el => el.Selected = false );
		netInc.typeEmailTemplates = [];
		netInc.user = User.empty();
		netInc.user.Id = '1234-ABCD-0987'
		netInc.noteTypes = undefined;
		return netInc;
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
	it('should be created ...', fakeAsync( () => {
		sut.networkIncident = createNetworkIncident( );
		tickFakeWait( 10 );
		tickFakeWait( 400 );
		//
		console.log(
			'===================================\n' +
			'NetworkLogGridComponent should create ...' );
		expect( sut ).toBeTruthy();
	} ) );
	/*
	** Initial columns displayed
	**   IP Address		Date   Type  Log
	** > 17.56.48.13	05/07  DoS   [DoS attack: RST Sca...
	** or:
	**   IP Address		Date   Type  Log	
	** > 54.183.209.144	11/28  VS    Fake log 4, Fake log...  Del
	*/
	it('default data should have all columns ...', fakeAsync( () => {
		sut.networkIncident = createNetworkIncident( );
		tickFakeWait( 10 );
		tickFakeWait( 200 );
		tickFakeWait( 200 );
		//
		const numCols: number = 7;
		console.log( sut.networkIncident.incident );
		console.log( sut.networkIncident.networkLogs );
		console.log( fixture.debugElement.queryAll(By.css( '#netLogTable > div > table' )) );
		const netLogBodyCols = fixture.debugElement.queryAll(By.css(
			'#netLogTable > div > table > tbody > tr:nth-child(1) > td' ));
			// '#netLogTable > div > div > table > tbody > tr:nth-child(1) > td' ));
		expect( netLogBodyCols.length ).toBe( numCols );
	} ) );
	/*
	** Initial rows displayed
	*/
	it('default data should have all rows ...', fakeAsync( () => {
		sut.networkIncident = createNetworkIncident( );
		tickFakeWait( 10 );
		tickFakeWait( 200 );
		tickFakeWait( 200 );
		//
		const numRows: number = mockDatum.length;
		const netLogBodyRows = fixture.debugElement.queryAll(By.css(
			'#netLogTable > div > table > tbody > tr' ));
		expect( netLogBodyRows.length ).toBe( numRows );
	} ) );
	//
	it('mailed incident should have no selection column ...', fakeAsync( () => {
		const testNetInc: INetworkIncident= createNetworkIncident( );
		testNetInc.incident.Mailed = true;
		testNetInc.networkLogs[3].Selected = true;
		testNetInc.incident.IPAddress = testNetInc.networkLogs[3].IPAddress;
		sut.networkIncident = testNetInc;
		sut.dt.globalFilterFields = ['IPAddress'];
		sut.ngAfterContentInit( );
		tickFakeWait( 10 );
		tickFakeWait( 200 );
		tickFakeWait( 200 );
		const loop = [1,2,3,4];
		loop.forEach( i => {
			if ( sut.disabled === undefined ) {
				tick( 10000 );
			}
		});
		tickFakeWait( 1 );
		const numCols: number = 5;
		const netLogBodyCols = fixture.debugElement.queryAll(By.css(
			'#netLogTable > div > table > tbody > tr:nth-child(1) > td' ));
		expect( netLogBodyCols.length ).toBe( numCols );
	}));
	//
	it('setTableFilter: should filter log rows ...', fakeAsync( () => {
		// given
		const ipAddr = mockDatum[1].IPAddress;
		sut.networkIncident = createNetworkIncident( );
		tickFakeWait( 10 );
		tickFakeWait( 400 );
		// when
		const ret = sut.setTableFilter( ipAddr );
		// then
		tickFakeWait( 1000 );
		expect( ret ).toEqual( true );
	} ) );
	//
	it('setTableFilter: should not filter if view problems ...', fakeAsync( () => {
		// given
		const ipAddr = mockDatum[1].IPAddress;
		sut.networkIncident = createNetworkIncident( );
		sut.dt = undefined;
		tickFakeWait( 10 );
		tickFakeWait( 400 );
		// when
		const ret = sut.setTableFilter( ipAddr );
		// then
		tickFakeWait( 1000 );
		expect( ret ).toEqual( false );
	} ) );
	//
	it('incident should have only selected rows ...', fakeAsync( () => {
		const testNetInc: INetworkIncident= createNetworkIncident( );
		testNetInc.networkLogs[4].Selected = true;
		testNetInc.networkLogs[5].Selected = true;
		testNetInc.networkLogs[4].IncidentId = testNetInc.incident.IncidentId;
		testNetInc.networkLogs[5].IncidentId = testNetInc.incident.IncidentId;
		testNetInc.incident.IPAddress = testNetInc.networkLogs[4].IPAddress;
		sut.networkIncident = testNetInc;
		sut.ngAfterContentInit( );
		tickFakeWait( 1 );
		tickFakeWait( 2000 );
		fixture.detectChanges( ); // trigger final data binding
		fixture.whenStable( );
		const numRows: number = 2;
		const netLogBodyRows = fixture.debugElement.queryAll(By.css(
			numRowsSelector ));
		expect( netLogBodyRows.length ).toBe( numRows );
	}));
	//
	it('should filter on ip-address when selected ...', fakeAsync( () => {
		sut.networkIncident = createNetworkIncident( );
		sut.ngAfterContentInit( );
		tickFakeWait( 1000 );
		tickFakeWait( 1000 );
		//
		// console.warn( fixture.debugElement.query(By.css(
		// 	'#netLogTable > div > table > tbody > tr:nth-child(5) > td:nth-child(2) > p-tablecheckbox > p-checkbox > input' )) );
		const netLogCheckbox: HTMLInputElement = fixture.debugElement.query(By.css(
			'#netLogTable > div > table > tbody > tr:nth-child(5) > td:nth-child(2) > p-tablecheckbox > p-checkbox > input' )).nativeElement;
		netLogCheckbox.click();
		tickFakeWait( 5000 );
		const numRows: number = 2;
		const netLogBodyRows = fixture.debugElement.queryAll(By.css(
			numRowsSelector ));
		expect( netLogBodyRows.length ).toBe( numRows );
	}));
	//
	it('should have all rows when unselected ...', fakeAsync( () => {
		sut.networkIncident = createNetworkIncident( );
		sut.ngAfterContentInit( );
		tickFakeWait( 100 );
		tickFakeWait( 100 );
		//
		let netLogCheckbox: HTMLInputElement = fixture.debugElement.query(By.css(
			'#netLogTable > div > table > tbody > tr:nth-child(4) > td:nth-child(2) > p-tablecheckbox > p-checkbox > input' )).nativeElement;
		netLogCheckbox.click();
		tickFakeWait( 5000 );
		let numRows: number = 1;
		let netLogBodyRows = fixture.debugElement.queryAll(By.css(
			numRowsSelector ));
		expect( netLogBodyRows.length ).toBe( numRows );
		netLogCheckbox = fixture.debugElement.query(By.css(
			'#netLogTable > div > table > tbody > tr:nth-child(1) > td:nth-child(2) > p-tablecheckbox > p-checkbox > input' )).nativeElement;
		netLogCheckbox.click( );
		tickFakeWait( 2000 );
		numRows = 6;
		netLogBodyRows = fixture.debugElement.queryAll(By.css(
			numRowsSelector ));
		expect( netLogBodyRows.length ).toBe( numRows );
	}));
	/*
	** afterViewInit( complete: boolean ): boolean
	*/
	it('afterViewInit: should fail ...', fakeAsync(() => {
		// given
		sut.networkIncident = null;
		// when
		const ret: boolean = sut.afterViewInit( false );
		// then
		expect( ret ).toEqual( false );
		//
	}));
	/*
	** viewInitIPAddress( ): string
	*/
	it('viewInitIPAddress: should emit change ...', fakeAsync(() => {
		// given
		const _ipAddr = mockDatum[1].IPAddress;
		sut.networkIncident = createNetworkIncident( );
		sut.networkIncident.networkLogs[2].Selected = true;
		tickFakeWait( 10 );
		tickFakeWait( 400 );
		sut.networkIncident.incident.IPAddress = _ipAddr;
		// when
		const ret: string = sut.viewInitIPAddress( );
		// then
		expect( ret ).toEqual( sut.selectedLogs[0].IPAddress );
		//
	}));
	/*
	** deleteItem( delId: number ): boolean
	*/
	it('deleteItem: should delete row from in memory networkLogs and move to deletedLogs ...', fakeAsync( () => {
		sut.networkIncident = createNetworkIncident( );
		sut.ngAfterContentInit( );
		tickFakeWait( 1 );
		tickFakeWait( 2000 );
		expect( sut.networkIncident.deletedLogs.length ).toBe( 0 );
		expect( sut.networkIncident.networkLogs.length ).toBe( mockDatum.length );
		//
		const delId = 3;
		sut.deleteItem( delId );
		tickFakeWait( 50 );
		//
		expect( sut.networkIncident.deletedLogs.length ).toBe( 1 );
		expect( sut.networkIncident.networkLogs.length ).toBe( mockDatum.length - 1 );
		expect( sut.networkIncident.deletedLogs[0].NetworkLogId ).toBe( delId );
	}));
	//
	it('deleteItem: should delete row shold fail ...', fakeAsync( () => {
		// given
		sut.networkIncident = createNetworkIncident( );
		sut.ngAfterContentInit( );
		tickFakeWait( 1 );
		tickFakeWait( 2000 );
		expect( sut.networkIncident.deletedLogs.length ).toBe( 0 );
		expect( sut.networkIncident.networkLogs.length ).toBe( mockDatum.length );
		spyOn( alertService, 'setWhereWhatWarning' );
		// when
		sut.deleteItem( 9999999 );
		tickFakeWait( 1 );
		// then
		expect( alertService.setWhereWhatWarning ).toHaveBeenCalled( );
	}));
	/*
	** deleteItemClicked( item: Incident ) :boolean
	*/
	it('deleteItemClicked: should delete row when event called and OK is clicked...', fakeAsync(() => {
		// given
		const delRow: INetworkLog = { ... mockDatum[ 4 ] };
		const delId: number = delRow.NetworkLogId;
		sut.networkIncident.networkLogs = [ ... mockDatum ];
		const expected: number = sut.networkIncident.networkLogs.length - 1;
		spyOn(confirmService, 'confirm').and.callFake(
			(confirmation: Confirmation) => {
				return confirmation.accept();
			});
		// when
		const ret: boolean = sut.deleteItemClicked( delRow );
		// then
		expect( ret ).toEqual( false );
		tickFakeWait( 10 );
		tickFakeWait( 900 );
		expect( sut.networkIncident.networkLogs.length ).toBe( expected );
		//
	}));
	//
});
// ===========================================================================
