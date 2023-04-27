// ===========================================================================
// File: register.component.spec.ts
import { ComponentFixture, TestBed, inject, fakeAsync, tick, waitForAsync } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule, NgForm } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { of, throwError } from 'rxjs';
//
import { ConfirmationService, SelectItem } from 'primeng/api';
import { Dialog } from 'primeng/dialog';
import { Header, Footer } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
//
import { AlertsService } from '../../global/alerts/alerts.service';
import { Alerts } from '../../global/alerts/alerts';
import { AlertLevel } from '../../global/alerts/alert-level.enum';
import { IAuthResponse, AuthResponse } from './iauth-response';
import { User } from '../../net-incident/user';
import { UserService } from '../../net-incident/services/user.service';
import { AuthService } from '../../net-incident/services/auth.service';
import { ConsoleLogService } from '../../global/console-log/console-log.service';
import { BaseCompService } from '../../global/base-comp/base-comp.service';
import { LoginComponent } from './login.component';
import { ServerSelectionWindowComponent } from '../../net-incident/server-selection-window/server-selection-window.component';
//
describe('LoginComponent', () => {
	let sut: LoginComponent;
	let fixture: ComponentFixture<LoginComponent>;
	let alertService: AlertsService;
	let baseService: BaseCompService;
	let consoleService: ConsoleLogService;
	// Create a fake TwainService object with a `getQuote()` spy
	const authServiceSpy = jasmine.createSpyObj('AuthService',
				['authenticate', 'logout', 'isLoggedIn', 'isLoggedOut']);
	const userServiceSpy = jasmine.createSpyObj('UserService',
			['emptyUser', 'getModelById']);
	//
	beforeEach(waitForAsync(() => {
		TestBed.configureTestingModule({
			imports: [
				FormsModule,
				ButtonModule,
				BrowserAnimationsModule
			],
			declarations: [
				LoginComponent,
				Dialog,
				Header,
				Footer,
				ServerSelectionWindowComponent
			],
			providers: [
				BaseCompService,
				AlertsService,
				ConsoleLogService,
				ConfirmationService,
				{ provide: AuthService, useValue: authServiceSpy },
				{ provide: UserService, useValue: userServiceSpy }
			]
		} );
		baseService = TestBed.inject( BaseCompService );
		alertService = baseService._alerts;
		consoleService = baseService._console;
		TestBed.compileComponents();
	}));
	//
	beforeEach(() => {
		fixture = TestBed.createComponent(LoginComponent);
		sut = fixture.componentInstance;
		fixture.detectChanges();
	});
	//
	afterEach(() => {
	});
	/*
	** Pause for events to process.
	*/
	function tickFakeWait( ticks: number ) {
		tick( ticks );
		fixture.detectChanges( ); // trigger initial data binding
		fixture.whenStable( );
	}
	//
	it('should be created ...', () => {
		console.log(
			'=================================\n' +
			'LoginComponent: should create ...' );
		expect( sut ).toBeTruthy();
	});
	//
	// test of loginUser, validation failure
	//
	it('loginUser should fail login when invalid password ...', fakeAsync( ( ) => {
		//
		sut.model.UserName = 'Xyz';
		sut.model.Password = '';
		sut.model.ServerShortName = 'XyzServer';
		const subscription = alertService.getAlerts().subscribe({
			next: (msg: Alerts) => {
				expect( msg ).toBeTruthy( );
				expect( msg.level ).toBe( AlertLevel.Error );
			},
			error: (error) => {
				fail( 'loginUser, failed: ' + error );
			}
		});
		const ret: number = sut.loginUser();
		expect( ret ).toBe( -1 );
		//
	} ) );
	//
	// test of loginUser, fail with bad username or password
	//
	it('loginUser should fail login when bad username or password ...', fakeAsync( ( ) => {
		//
		sut.model.UserName = 'Xyz';
		sut.model.Password = 'Xyz';
		sut.model.ServerShortName = 'XyzServer';
		const errMsg: string = 'Fake Service error';
		const mockErrorResponse: HttpErrorResponse = new HttpErrorResponse({
			error: {}, status: 500, url: 'http://localhost', statusText: errMsg });
		authServiceSpy.authenticate.and.returnValue(throwError( mockErrorResponse ));
		alertService.getAlerts( ).subscribe({
			next: ( msg: Alerts ) => {
				expect( msg ).toBeTruthy( );
				expect( msg.level ).toBe( AlertLevel.Error );
			},
			error: (error) => {
				console.error( error );
			},
			complete: () => { }
		});
		const ret: number = sut.loginUser();
		tickFakeWait( 10 );
		expect( ret ).toBe( 0 );
		//
	} ) );
	//
	// test of loginUser, succeed
	//
	it('loginUser should succeed to login user ...', fakeAsync( ( ) => {
		//
		sut.model.UserName = 'Xyz';
		sut.model.Password = 'Xyz';
		sut.model.ServerShortName = 'XyzServer';
		const expiresAt: number = Date.now() + 10000;
		const tokenResp: AuthResponse = new AuthResponse(
				'1111', new Date(expiresAt).toUTCString()
			);
		authServiceSpy.authenticate.and.returnValue(of( tokenResp ));
		const emptyUser: User = new User(
			'','','','','','','',false,'',false,0,[],'',undefined, []);
		emptyUser.ServerShortName = sut.model.ServerShortName;
		userServiceSpy.getModelById.and.returnValue(of( emptyUser ));
		sut.emitClose.subscribe( user => {
			expect( user ).toBe( emptyUser );
		} );
		const ret: number = sut.loginUser();
		expect( ret ).toBe( 0 );
		//
	} ) );
	//
	// test of getUserServer, succeed
	//
	it('getUserServer should get login user ...', fakeAsync( ( ) => {
		//
		sut.model.UserName = 'Xyz';
		sut.model.ServerShortName = 'XyzServer';
		const emptyUser: User = new User(
			'','','','','','','',false,'',false,0,[],'',undefined, []);
		emptyUser.ServerShortName = sut.model.ServerShortName;
		userServiceSpy.getModelById.and.returnValue(of( emptyUser ));
		sut.emitClose.subscribe( user => {
			expect( user ).toBe( emptyUser );
		} );
		sut.getUserServer( sut.model.UserName, sut.model.ServerShortName );
		//
	} ) );
	//
	it('getUserServer should get login user no short name...', fakeAsync( ( ) => {
		// given
		const _srvrs = [ {value: 'XYZ Server', label: 'XYZ Server'} ];
		sut.displayServersWindow = false;
		sut.model.UserName = 'Xyz';
		sut.model.ServerShortName = '';
		const emptyUser: User = new User(
			'','','','','','','',false,'',false,0,_srvrs,'',undefined, []);
		emptyUser.ServerShortName = sut.model.ServerShortName;
		userServiceSpy.getModelById.and.returnValue(of( emptyUser ));
		// when
		sut.getUserServer( sut.model.UserName, sut.model.ServerShortName );
		// then
		expect( sut.displayServersWindow ).toEqual( true );
		expect( sut.selectItemsWindow ).toEqual( _srvrs );
	} ) );
	//
	it('getUserServer should handle get user error  ...', fakeAsync( ( ) => {
		// given
		const errMsg: string = 'Fake Service error';
		const response: HttpErrorResponse = new HttpErrorResponse({
			error: {}, status: 500, url: 'http://localhost', statusText: errMsg });
		userServiceSpy.getModelById.and.returnValue( throwError( ( ) => response ) );
		const subscription = alertService.getAlerts().subscribe({
			next: (msg: Alerts) => {
				console.warn( 'getUserServer should handle get user error' );
				expect( msg.level ).toBe( AlertLevel.Error );
			},
			error: (error) => {
				fail( 'loginUser, failed: ' + error );
			},
			complete: () => { }
		});
		spyOn( alertService, 'setWhereWhatError' );
		// when
		sut.getUserServer( sut.model.UserName, sut.model.ServerShortName );
		// then
		expect( alertService.setWhereWhatError ).toHaveBeenCalled( );
		//
	} ) );
	//
	// test of onServerSelected, succeed
	//
	it('onServerSelected should get login user ...', fakeAsync( ( ) => {
		//
		const serverShortName = 'XyzServer';
		sut.model.UserName = 'Xyz';
		sut.model.Password = 'Xyz';
		sut.model.ServerShortName = '';
		const emptyUser: User = new User(
			'','','','','','','',false,'',false,0,[],'',undefined, []);
		emptyUser.ServerShortName = serverShortName;
		userServiceSpy.getModelById.and.returnValue(of( emptyUser ));
		sut.emitClose.subscribe( user => {
			expect( sut.model.ServerShortName ).toEqual( serverShortName );
		} );
		sut.onServerSelected( serverShortName );
		//
	} ) );
	//
});
// ===========================================================================
