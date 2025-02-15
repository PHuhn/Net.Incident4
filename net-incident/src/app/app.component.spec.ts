// ===========================================================================
// File: app.component.spec.ts
import { TestBed, ComponentFixture, waitForAsync } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { By } from '@angular/platform-browser';
import { AppComponent } from './app.component';
//
import { HttpClient, provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
//
import { ConfirmationService } from 'primeng/api';
import { ConfirmDialog } from 'primeng/confirmdialog';
import { $dt } from '@primeng/themes';
//
import { APP_GLOBAL_COMPONENTS } from './global/APP.GLOBAL';
import { APP_COMPONENTS } from './APP.COMPONENTS';
import { APP_MODULE_PRIMENG } from './APP.MODULE-PRIMENG';
import { BaseCompService } from './global/base-comp/base-comp.service';
import { AlertsService } from './global/alerts/alerts.service';
import { AuthService } from './net-incident/services/auth.service';
import { User } from './net-incident/user';
import { AlertsComponent } from './global/alerts/alerts.component';
import { FormsModule } from '@angular/forms';
//
describe('AppComponent', () => {
	let sut: AppComponent;
	let http: HttpClient;
	let backend: HttpTestingController;
	let fixture: ComponentFixture< AppComponent >;
	let baseService: BaseCompService;
	let alertService: AlertsService;
	const authServiceSpy =
		jasmine.createSpyObj( 'AuthService', ['logout']);
	//
	beforeEach( waitForAsync(() => {
		TestBed.configureTestingModule({
			imports: [
				FormsModule,
				RouterTestingModule,
				BrowserAnimationsModule,
				APP_MODULE_PRIMENG
			],
			declarations: [
				AppComponent,
				APP_GLOBAL_COMPONENTS,
				APP_COMPONENTS
			],
			providers: [
				BaseCompService,
				AlertsService,
				ConfirmationService,
				provideHttpClient(),
				provideHttpClientTesting(),
				{ provide: AuthService, useValue: authServiceSpy }
			]
		} );
		baseService = TestBed.inject( BaseCompService );
		alertService = baseService._alerts;
		http = TestBed.inject( HttpClient );
		backend = TestBed.inject( HttpTestingController );
		TestBed.compileComponents( );
	} ) );
	//
	beforeEach(() => {
		fixture = TestBed.createComponent( AppComponent );
		sut = fixture.componentInstance;
		fixture.detectChanges( );
	});
	//
	it('should create the app', () => {
		console.log( 'AppComponent' );
		// given / when / then
		expect( sut ).toBeTruthy();
	});
	//
	it('should contain ConfirmDialog with key of delete ...', () => {
		// given / when
		const confirm: ConfirmDialog = fixture.debugElement.query(
			By.css( 'p-confirmdialog' )).componentInstance;
		// then
		expect( confirm.key ).toEqual( 'delete' );
	});
	//
	it('should contain Alert component ...', () => {
		// given / when
		const alerts: AlertsComponent = fixture.debugElement.query(
			By.css( 'app-alerts' )).componentInstance;
		// then
		expect( alerts ).toBeDefined( );
	});
	//
	// onAuthLogout(event): void
	//
	it('onAuthLogout: should unset login ...', () => {
		// given
		sut.authenticated = true;
		authServiceSpy.logout.and.returnValue( { } );
		// when
		sut.onAuthLogout( 0 );
		// then
		expect( sut.authenticated ).toEqual( false );
	});
	//
	// fakeLogin()
	//
	it('fakeLogin: should unset login ...', () => {
		// given
		sut.authenticated = false;
		// when
		sut.fakeLogin( );
		// then
		expect( sut.user.UserName ).toEqual( 'Phil' );
		expect( sut.authenticated ).toEqual( true );
	});
	//
	// onAuthenticated( user: User ): void
	//
	it('onAuthenticated: should login ...', () => {
		// given
		sut.fakeLogin( );
		// when
		sut.onAuthenticated( sut.user );
		// then
		expect( AppComponent.securityManager ).toBeDefined( );
		expect( sut.authenticated ).toEqual( true );
	});
	//
	it('onAuthenticated: should not authenticate, without roles ...', () => {
		// given
		const _user = new User(
			'e0fcb3e8-252a-4311-b782-7e094f0737aa', 'Phil', 'Phillip', 'Huhn',
			'Phil Huhn', 'Phil', 'PhilHuhn@yahoo.com', true, '734-545-5845', true,
			1, [], '', undefined, []
		);
		// when
		sut.onAuthenticated( _user );
		// then
		expect( sut.authenticated ).toEqual( false );
	});
	/*
	** PrimeNG Theming configuration
	*/
	it('should set primary color to gradiants of blue ...', () => {
		// given / when / then
		const primary50Color = $dt('primary.50');
		expect( primary50Color.value ).toEqual( '#eff6ff' );
		expect( primary50Color.variable ).toEqual( 'var(--p-primary-50)' );
		expect( primary50Color.name ).toEqual( '--p-primary-50' );
		//
		const primary500Color = $dt('primary.500');
		expect( primary500Color.value ).toEqual( '#3b82f6' );
		expect( primary500Color.variable ).toEqual( 'var(--p-primary-500)' );
		expect( primary500Color.name ).toEqual( '--p-primary-500' );
	});
	//
	it('should set dialog background blue ...', () => {
		// given / when / then
		const dialogBg = $dt('dialog.background');
		expect( dialogBg.value.light.value ).toEqual( '#eff6ff' );
		expect( dialogBg.value.dark.value ).toEqual( '#1e3a8a' );
		expect( dialogBg.variable ).toEqual( 'var(--p-dialog-background)' );
		expect( dialogBg.name ).toEqual( '--p-dialog-background' );
	});
	//
	it('should set button primary background blue ...', () => {
		// given / when / then
		const buttonPrimaryBg = $dt('button.primary.background');
		// console.warn( buttonPrimaryBg );
		expect( buttonPrimaryBg.value.light.value ).toEqual( '#2563eb' );
		expect( buttonPrimaryBg.value.dark.value ).toEqual( '#2563eb' );
		expect( buttonPrimaryBg.variable ).toEqual( 'var(--p-button-primary-background)' );
		expect( buttonPrimaryBg.name ).toEqual( '--p-button-primary-background' );
	});
	//
	it('should set datatable header background blue ...', () => {
		// given / when / then
		const datatableHeaderBg = $dt('datatable.header.background');
		// console.warn( datatableHeaderBg );
		expect( datatableHeaderBg.value.light.value ).toEqual( '#bfdbfe' );
		expect( datatableHeaderBg.value.dark.value ).toEqual( '#172554' );
		expect( datatableHeaderBg.variable ).toEqual( 'var(--p-datatable-header-background)' );
		expect( datatableHeaderBg.name ).toEqual( '--p-datatable-header-background' );
	});
	//
	it('should set menubar background blue ...', () => {
		// given / when / then
		const menubarBg = $dt('menubar.background');
		// console.warn( menubarBg );
		expect( menubarBg.value.light.value ).toEqual( '#3b82f6' );
		expect( menubarBg.value.dark.value ).toEqual( '#1e40af' );
		expect( menubarBg.variable ).toEqual( 'var(--p-menubar-background)' );
		expect( menubarBg.name ).toEqual( '--p-menubar-background' );
	});
	//
	it('should set card background blue ...', () => {
		// given / when / then
		const cardBg = $dt('card.background');
		// console.warn( cardBg );
		expect( cardBg.value.light.value ).toEqual( '#dbeafe' );
		expect( cardBg.value.dark.value ).toEqual( '#1e3a8a' );
		expect( cardBg.variable ).toEqual( 'var(--p-card-background)' );
		expect( cardBg.name ).toEqual( '--p-card-background' );
	});
	//
});
// ===========================================================================
