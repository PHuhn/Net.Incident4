// ===========================================================================
// File: auth.service.spec.ts
import { TestBed, waitForAsync } from '@angular/core/testing';
//
import { HttpClient, provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
//
import { AuthResponse } from '../../public/login/iauth-response';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';
//
describe('AuthService', () => {
	let sut: AuthService;
	let http: HttpClient;
	let backend: HttpTestingController;
	const url: string = environment.base_Url + 'Authenticate/Login';
	beforeEach( waitForAsync( ( ) => {
		TestBed.configureTestingModule( {
			providers: [
				provideHttpClient(),
				provideHttpClientTesting(),
				AuthService
			]
		} );
		// Setup injected pre service for each test
		http = TestBed.inject( HttpClient );
		backend = TestBed.inject( HttpTestingController );
		//
		sut = TestBed.inject( AuthService );
		TestBed.compileComponents();
	} ) );
	//
	afterEach( ( ) => {
		// cleanup
		backend.verify();
	});
	//
	function setLocalStorage( token: string, expiresAt: number, userName: string ) {
		localStorage.setItem( 'access_expires', JSON.stringify( expiresAt.valueOf() ) );
		localStorage.setItem( 'access_token', token );
		localStorage.setItem( 'access_userName', userName );
	}
	//
	function unsetLocalStorage( ) {
		localStorage.removeItem( 'access_token' );
		localStorage.removeItem( 'access_expires' );
		localStorage.removeItem( 'access_userName' );
	}
	//
	it( 'should be created ...', ( ) => {
		console.log(
			'===================================\n' +
			'AuthService should create ...' );
		expect( sut ).toBeTruthy( );
	});
	// public logout( )
	it( 'should clear local storage when logout ...', ( ) => {
		setLocalStorage( '1234567890', Date.now() + 10000, 'TestUser' );
		sut.logout( );
		const token = localStorage.getItem( 'access_token' );
		expect( token ).toBeNull();
		unsetLocalStorage( );
	});
	// public isLoggedIn()
	it( 'should be logged in when local storage set ...', ( ) => {
		setLocalStorage( '1234567890', Date.now() + 10000, 'TestUser' );
		const ret: boolean = sut.isLoggedIn( );
		expect( ret ).toBeTruthy( );
		unsetLocalStorage( );
	});
	// public isLoggedIn()
	it( 'should not be logged in when local storage not set ...', ( ) => {
		const ret: boolean = sut.isLoggedIn( );
		expect( ret ).toBeFalsy( );
	});
	// public isLoggedOut()
	it( 'should not be logged out when local storage set ...', ( ) => {
		setLocalStorage( '1234567890', Date.now() + 10000, 'TestUser' );
		const ret: boolean = sut.isLoggedOut( );
		expect( ret ).toBeFalsy( );
		unsetLocalStorage( );
	});
	// public isLoggedOut()
	it( 'should be logged out when local storage not set ...', ( ) => {
		const ret: boolean = sut.isLoggedOut( );
		expect( ret ).toBeTruthy( );
	});
	// public getExpiration()
	it( 'should get the expiration that was set ...', ( ) => {
		const expires: number = Date.now() + 10000;
		setLocalStorage( '1234567890', expires, 'TestUser' );
		const ret: number = sut.getExpiration( );
		expect( ret ).toBe( expires );
		unsetLocalStorage( );
	});
	// public authenticate( userName: string, password: string )
	it( 'should authenticate user name and password ...', ( ) => {
		//
		const userName: string = 'TestUser';
		const token: string = '123456789012345678901234567890';
		const expiresAt: number = Date.now() + 1000;
		const response: AuthResponse = new AuthResponse(
			token, new Date(expiresAt).toISOString()
		);
		sut.authenticate( userName, 'asdfdsaf' ).subscribe({
			next: ( tokenData: AuthResponse ) => {
				//
				expect( tokenData.token ).toEqual( token );
				const expire: number = sut.getExpiration( );
				const accessToken: string | null = localStorage.getItem( 'access_token' );
				expect( expire ).toEqual( expiresAt );
				expect( accessToken ).toEqual( token );
				unsetLocalStorage( );
				//
			},
			error: (error) => {
				expect( error ).toBeFalsy( );
			},
			complete: () => { }
		});
		const request = backend.expectOne( `${url}/` );
		expect( request.request.method ).toBe( 'POST' );
		request.flush( response );
		//
	});
	// public authenticate( userName: string, password: string )
	it( 'should fail authentication with bad token length ...', ( ) => {
		//
		const userName: string = 'TestUser';
		const token: string = '1234567890';
		const response: AuthResponse = new AuthResponse(
			token, ''
		);
		const errMsg = `Error: Auth-Service.authenticate: Invalid token returned.`;
		sut.authenticate( 'badUserName', 'asdfdsaf' ).subscribe({
			next: ( tokenData: AuthResponse ) => {
				//
				console.log( JSON.stringify( tokenData ) );
				unsetLocalStorage( );
				fail( 'Bad user name should fail.' );
				//
			},
			error: (error) => {
				expect( String( error ) ).toEqual( errMsg );
			},
			complete: () => { }
		});

		const request = backend.expectOne( `${url}/` );
		expect( request.request.method ).toBe( 'POST' );
		request.flush( response );
		//
	});
	//
});
// ===========================================================================
