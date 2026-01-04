// ===========================================================================
// File: app.component.spec.ts
import { TestBed, ComponentFixture, waitForAsync } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { By } from '@angular/platform-browser';
import { AppComponent } from './app.component';
//
import { HttpClient, provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
//
import { ConfirmationService } from 'primeng/api';
import { ConfirmDialog } from 'primeng/confirmdialog';
import { $dt } from '@primeuix/themes';
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
		const blue50 = $dt('blue.50');
		const blue500 = $dt('blue.500');
		const primary50Color = $dt('primary.50');
		expect( primary50Color.value ).toEqual( blue50.value );
		expect( primary50Color.variable ).toEqual( 'var(--p-primary-50)' );
		expect( primary50Color.name ).toEqual( '--p-primary-50' );
		//
		const primary500Color = $dt('primary.500');
		expect( primary500Color.value ).toEqual( blue500.value );
		expect( primary500Color.variable ).toEqual( 'var(--p-primary-500)' );
		expect( primary500Color.name ).toEqual( '--p-primary-500' );
	});
	//
	it('should set dialog background blue ...', () => {
		// given / when / then
		const blue50 = $dt('blue.50');
		const blue900 = $dt('blue.900');
		const dialogDarkBg = $dt('components.dialog.colorScheme.dark.root.background');
		// console.warn( $dt('components.dialog.colorScheme.dark.root.background') );
		expect( $dt('components.dialog.colorScheme.light.root.background').value ).toEqual( blue50.value );
		expect( $dt('components.dialog.colorScheme.dark.root.background').value ).toEqual( blue900.value );
		expect( dialogDarkBg.variable ).toEqual( 'var(--p-dialog-background)' );
		expect( dialogDarkBg.name ).toEqual( '--p-dialog-background' );
	});
	//
	it('should set button primary background blue ...', () => {
		// given / when / then
		const blue600 = $dt('blue.600');
		const buttonPrimaryBg = $dt('components.button.colorScheme.light.root.primary.background');
		expect( $dt('components.button.colorScheme.light.root.primary.background').value ).toEqual( blue600.value );
		expect( $dt('components.button.colorScheme.dark.root.primary.background').value ).toEqual( blue600.value );
		expect( buttonPrimaryBg.variable ).toEqual( 'var(--p-button-primary-background)' );
		expect( buttonPrimaryBg.name ).toEqual( '--p-button-primary-background' );
	});
	//
	it('should set datatable header background blue ...', () => {
		// given / when / then
		const blue200 = $dt('blue.200');
		const blue950 = $dt('blue.950');
		const datatableHeaderBg = $dt('components.datatable.colorScheme.light.header.background');
		expect( $dt( 'components.datatable.colorScheme.light.header.background' ).value ).toEqual( blue200.value );
		expect( $dt( 'components.datatable.colorScheme.dark.header.background' ).value ).toEqual( blue950.value );
		expect( datatableHeaderBg.variable ).toEqual( 'var(--p-datatable-header-background)' );
		expect( datatableHeaderBg.name ).toEqual( '--p-datatable-header-background' );
	});
	//
	it('should set menubar background blue ...', () => {
		// given / when / then
		const blue500 = $dt('blue.500');
		const blue800 = $dt('blue.800');
		const menubarBg = $dt('components.menubar.colorScheme.light.background');
		expect( $dt('components.menubar.colorScheme.light.background').value ).toEqual( blue500.value );
		expect( $dt('components.menubar.colorScheme.dark.background').value ).toEqual( blue800.value );
		expect( menubarBg.variable ).toEqual( 'var(--p-menubar-background)' );
		expect( menubarBg.name ).toEqual( '--p-menubar-background' );
	});
	//
	it('should set card background blue ...', () => {
		// given / when / then
		const blue100 = $dt('blue.100');
		const blue900 = $dt('blue.900');
		const cardBg = $dt('components.card.colorScheme.light.background');
		expect( $dt('components.card.colorScheme.light.background').value ).toEqual( blue100.value );
		expect( $dt('components.card.colorScheme.dark.background').value ).toEqual( blue900.value );
		expect( cardBg.variable ).toEqual( 'var(--p-card-background)' );
		expect( cardBg.name ).toEqual( '--p-card-background' );
	});
	//
});
// ===========================================================================
