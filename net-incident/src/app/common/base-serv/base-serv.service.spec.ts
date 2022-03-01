// ===========================================================================
// File: base-srvc.service.spec.ts
import { TestBed, waitForAsync } from '@angular/core/testing';
import { HttpClientModule, HttpClient, HttpErrorResponse } from '@angular/common/http';
import { HttpClientTestingModule, HttpTestingController, TestRequest } from '@angular/common/http/testing';
//
import { ConsoleLogService } from '../../global/console-log/console-log.service';
import { BaseServService } from './base-serv.service';
//
describe('BaseServService', () => {
	//
	let sut: BaseServService;
	let http: HttpClient;
	//
	beforeEach( waitForAsync( ( ) => {
		TestBed.configureTestingModule( {
			imports: [
				// HttpClient 4.3 testing
				HttpClientModule,
				HttpClientTestingModule
			],
			providers: [
				HttpClient,
				ConsoleLogService,
				BaseServService
			]
		} );
		// Setup sut
		http = TestBed.inject( HttpClient );
		sut = TestBed.inject( BaseServService );
		TestBed.compileComponents();
	} ) );
	//
	it('should be created ...', () => {
		expect( sut ).toBeTruthy();
	});
	//
	it('should inject HttpClient ...', () => {
		expect( sut['_http'] ).toBeDefined( );
	});
	//
	it('should inject ConsoleLogService ...', () => {
		expect( sut['_console'] ).toBeDefined( );
	});
	//
	// baseServiceError( error: any )
	//
	it( 'should throw a response error...', waitForAsync(() => {
		// given
		const errMsg = `Code: 599, Message: `
		const resp: any = new HttpErrorResponse(
			{ status: 599, url: 'http://localhost', statusText: 'Fake error' } );
		// when
		sut.baseServiceError( resp ).subscribe( () => {
				fail( 'handleError: expected error...' );
			}, ( error ) => {
				// then
				expect( error.substring(0, errMsg.length) ).toEqual( errMsg );
		} );
		//
	}));
	//
	it( 'should throw a string error...', waitForAsync(() => {
		// given
		const errMsg: string = 'Fake error';
		// when
		sut.baseServiceError( errMsg ).subscribe( () => {
				fail( 'handleError: expected error...' );
			}, ( error ) => {
				// then
				expect( error ).toEqual( errMsg );
		} );
		//
	}));
	//
});
// ===========================================================================
