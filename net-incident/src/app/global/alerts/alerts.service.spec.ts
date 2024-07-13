// ===========================================================================
// file: alerts.service.spects
import { TestBed, inject, fakeAsync  } from '@angular/core/testing';
//
import { AlertsService } from './alerts.service';
import { AlertLevel } from './alert-level.enum';
import { Message } from './message';
import { Alerts } from './alerts';
//
describe('AlertsService', () => {
	let service: AlertsService;
	//
	beforeEach(() => {
		TestBed.configureTestingModule({
			providers: [AlertsService]
		});
	});
	//
	beforeEach(inject([AlertsService], (srvc: AlertsService) => {
		service = srvc;
	}));
	//
	it('should create ...', () => {
		console.log(
			'===================================\n' +
			'AlertsService should create ...' );
		expect(service).toBeTruthy();
	});
	//
	it('should take alerts message ...', () => {
		const msg = 'Hello World';
		service.getAlerts().subscribe({
			next: (alertMsg: Alerts) => {
				// then
				expect(alertMsg.level).toBe(AlertLevel.Info);
				expect(alertMsg.messages.length).toEqual(1);
				expect(alertMsg.messages[0].message).toBe(msg);
			},
			error: (error) => {
				fail( 'get by id error, failed: ' + error );
			}
		});
		// when
		service.setAlerts(AlertLevel.Info, [new Message('1', AlertLevel.Info, msg)]);
	});
	//
	it('should take WhereWhat info Message ...', () => {
		service.getAlerts().subscribe({
			next: (alertMsg: Alerts) => {
				// then
				expect(alertMsg.level).toBe(AlertLevel.Info);
				expect(alertMsg.messages.length).toEqual(2);
				expect(alertMsg.messages[0].message).toBe('where');
				expect(alertMsg.messages[1].message).toBe('what');
			},
			error: (error) => {
				fail( 'get by id error, failed: ' + error );
			}
		});
		// when
		service.setWhereWhatInfo('where','what');
	});
	//
	it('should take WhereWhat Success Message ...', () => {
		service.getAlerts().subscribe({
			next: (alertMsg: Alerts) => {
				// then
				expect(alertMsg.level).toBe(AlertLevel.Success);
				expect(alertMsg.messages.length).toEqual(2);
				expect(alertMsg.messages[0].message).toBe('where');
				expect(alertMsg.messages[1].message).toBe('what');
			},
			error: (error) => {
				fail( 'get by id error, failed: ' + error );
			}
		});
		// when
		service.setWhereWhatSuccess('where','what');
	});
	//
	it('should take WhereWhatWarning message ...', () => {
		service.getAlerts().subscribe({
			next: (alertMsg: Alerts) => {
				// then
				expect(alertMsg.level).toBe(AlertLevel.Warning);
				expect(alertMsg.messages.length).toEqual(2);
				expect(alertMsg.messages[0].message).toBe('where');
				expect(alertMsg.messages[1].message).toBe('what');
			},
			error: (error) => {
				fail( 'get by id error, failed: ' + error );
			}
		});
		// when
		service.setWhereWhatWarning('where','what');
	});
	//
	it('should take WhereWhatError message ...', () => {
		service.getAlerts().subscribe({
			next: (alertMsg: Alerts) => {
				// then
				expect(alertMsg.level).toBe(AlertLevel.Error);
				expect(alertMsg.messages.length).toEqual(3);
				expect(alertMsg.messages[0].message).toBe('where');
				expect(alertMsg.messages[1].message).toBe('what');
				expect(alertMsg.messages[2].message).toBe('error');
			},
			error: (error) => {
				fail( 'get by id error, failed: ' + error );
			}
		});
		// when
		service.setWhereWhatError('where','what','error');
	});
	//
	it('should take WhereWhatError empty Message ...', () => {
		service.getAlerts().subscribe({
			next: (alertMsg: Alerts) => {
				// then
				expect(alertMsg.level).toBe(AlertLevel.Error);
				expect(alertMsg.messages.length).toEqual(3);
				expect(alertMsg.messages[0].message).toBe('-');
				expect(alertMsg.messages[1].message).toBe('-');
				expect(alertMsg.messages[2].message).toBe('-');
			},
			error: (error) => {
				fail( 'get by id error, failed: ' + error );
			}
		});
		// when
		service.setWhereWhatError('', '', '');
	});
	//
	it('should take warningSet message ...', () => {
		const msg: string = 'Is required.';
		const msgs: Message[] = [new Message( '1', AlertLevel.Info, msg )];
		service.getAlerts().subscribe({
			next: (alertMsg: Alerts) => {
				// then
				expect(alertMsg.level).toBe(AlertLevel.Warning);
				expect(alertMsg.messages.length).toEqual(1);
				expect(alertMsg.messages[0].message).toBe( msg );
			},
			error: (error) => {
				fail( 'get by id error, failed: ' + error );
			}
		});
		// when
		const ret = service.warningSet( msgs );
		expect( ret ).toEqual( true );
	});
	it('warningSet should fail if empty message array ...', () => {
		const ret = service.warningSet( [] );
		expect( ret ).toEqual( false );
	});
	//
	it('should take warningInit/Add/Post ...', () => {
		const msg: string = 'Is required.';
		service.warningInit( );
		const retAdd = service.warningAdd( msg );
		service.getAlerts().subscribe({
			next: (alertMsg: Alerts) => {
				// then
				expect(alertMsg.level).toBe(AlertLevel.Warning);
				expect(alertMsg.messages.length).toEqual(1);
				expect(alertMsg.messages[0].message).toBe( msg );
			},
			error: (error) => {
				fail( 'get by id error, failed: ' + error );
			}
		});
		// when
		const retPost = service.warningPost( );
		expect( retAdd ).toEqual( true );
		expect( retPost ).toEqual( true );
	});
	it('warningAdd should fail when empty message ...', () => {
		const retAdd = service.warningAdd( '' );
		expect( retAdd ).toEqual( false );
	});
	it('warningPost should fail when un-initialize ...', () => {
		const retPost = service.warningPost( );
		expect( retPost ).toEqual( false );
	});
	it('warningCount should return 0 after init ...', () => {
		service.warningInit( );
		expect( service.warningCount( ) ).toEqual( 0 );
	});
	/*
	**
	*/
	it('Error Alert should add a message ...', () => {
		// given
		const msg = 'Error message';
		ExpectMessage(1, AlertLevel.Error, msg);
		// when
		const id: string = service.errorAlert( msg );
		// then
		expect( id ).toEqual( '001' );
	});
	it('Warn Alertshould add a message ...', () => {
		// given
		const msg = 'Warn message';
		ExpectMessage(1, AlertLevel.Warning, msg);
		const id: string = service.warnAlert( msg );
		expect( id ).toEqual( '001' );
	});
	it('Success Alert should add a message ...', () => {
		// given
		const msg = 'Success message';
		ExpectMessage(1, AlertLevel.Success, msg);
		const id: string = service.successAlert( msg );
		expect( id ).toEqual( '001' );
	});
	it('Info Alert should add a message ...', fakeAsync( () => {
		// given
		const msg = 'Information  message';
		ExpectMessage(1, AlertLevel.Info, msg);
		const id: string = service.infoAlert( msg );
		expect( id ).toEqual( '001' );
	}));
	it('Err/Wrn/Suc/Inf Alert should add 4 messages ...', fakeAsync( () => {
		const idErr: string = service.errorAlert( 'Error message' );
		const idWrn: string = service.warnAlert( 'Warn message' );
		const idSuc: string = service.successAlert( 'Success message' );
		const idInf: string = service.infoAlert( 'Information message' );
		expect( idErr ).toEqual( '001' );
		expect( idWrn ).toEqual( '002' );
		expect( idSuc ).toEqual( '003' );
		expect( idInf ).toEqual( '004' );
	}));
	it('message archive ...', fakeAsync( () => {
		// given
		service.setWhereWhatInfo( 'Test', 'information' );
		service.setWhereWhatSuccess( 'Test', 'success' );
		const msgs = service.alertMessages;
		const msg = service.lastAlertMessage;
		// console.warn( msgs );
		expect( msgs.length ).toEqual( 10 );
		expect( msgs[0].message ).toEqual( 'success' );
		expect( msgs[1].message ).toEqual( 'Test' );
		expect( msgs[2].message ).toEqual( 'information' );
		expect( msgs[3].message ).toEqual( 'Test' );
		// console.warn( msg );
		expect( msg.label ).toEqual( AlertLevel.Success );
		expect( msg.message ).toEqual( 'success' );
	}));
	// helper
	function ExpectMessage(len: number, level: AlertLevel, message: string): void {
		service.getAlerts().subscribe({
			next: (alerts) => {
				// then
				const msgs = alerts.messages
				// console.warn( msgs );
				expect( msgs.length ).toEqual( len );
				expect( msgs[0].label ).toEqual( level );
				expect( msgs[0].message ).toEqual( message );
			},
			error: (error) => {
				fail( 'get by id error, failed: ' + error );
			}
		});
	}
	//
});
// ===========================================================================
