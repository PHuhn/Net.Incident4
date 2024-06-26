// ===========================================================================
// File: services.service.spec.ts
import { TestBed, getTestBed, inject, waitForAsync } from '@angular/core/testing';
//
import { HttpClient, HttpResponse, HttpErrorResponse, provideHttpClient } from '@angular/common/http';
import { HttpTestingController, TestRequest, provideHttpClientTesting } from '@angular/common/http/testing';
//
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
//
import { Message } from '../../global/alerts/message';
import { environment } from '../../../environments/environment';
import { ServicesService } from './services.service';
//
describe('ServicesService', () => {
	let sut: ServicesService;
	const baseUrl: string = environment.base_Url;
	let http: HttpClient;
	let backend: HttpTestingController;
	//
	beforeEach( waitForAsync( ( ) => {
		TestBed.configureTestingModule( {
			providers: [
				provideHttpClient(),
				provideHttpClientTesting(),
				{ provide: ServicesService, useClass: ServicesService }
			]
		} );
		sut = TestBed.inject( ServicesService );
		TestBed.compileComponents();
	} ) );
	//
	beforeEach(
		inject(
			[HttpClient, HttpTestingController],
			(httpClient: HttpClient, httpBackend: HttpTestingController) => {
				http = httpClient;
				backend = httpBackend;
		}
	) );
	//
	// Run HttpTestingController's verify to make sure that there are no
	// outstanding requests.
	//
	afterEach(() => {
		backend.verify();
	});
	//
	it('should be created ...', ( ) => {
		console.log(
			'=================================\n' +
			'ServicesService: should create ...' );
		expect(sut).toBeTruthy();
	} );
	//
	// getPing( ipAddress: string ): Observable<string>
	//
	it( 'should get ping results...', waitForAsync( ( ) => {
		//
		const ipAddress = '192.168.0.0';
		const pingUrl: string = `${baseUrl}services?service=ping&ipaddress=${ipAddress}`;
		sut.getPing( ipAddress ).subscribe( ( datum ) => {
			// console.log( datum );
			expect( datum ).toBe( ipAddress );
		});
		// use the HttpTestingController to mock requests and the flush method to provide dummy values as responses
		const request = backend.expectOne( pingUrl );
		expect( request.request.method ).toBe( 'GET' );
		request.flush(ipAddress);
		//
	}));
	//
	// getWhoIs( ipAddress: string ): Observable<string>
	//
	it( 'should get whois results...', waitForAsync( ( ) => {
		//
		const ipAddress = '192.168.0.1';
		const whoisUrl: string = `${baseUrl}services?service=whois&ipaddress=${ipAddress}`;
		sut.getWhoIs( ipAddress ).subscribe( ( datum ) => {
			expect( datum ).toBe( ipAddress );
		});
		// use expectOne(), expectNone() and match()
		const request = backend.expectOne( whoisUrl );
		expect( request.request.method ).toBe( 'GET' );
		request.flush(ipAddress);
		//
	}));
	//
	// getWhoIs( ipAddress: string ): Observable<string>
	//
	it( 'should handle a whois error results...', waitForAsync( ( ) => {
		//
		const ipAddress = '192.168.0.1';
		const errMsg: string = 'Fake error';
		const whoisUrl: string = `${baseUrl}services?service=whois&ipaddress=${ipAddress}`;
		//
		sut.getWhoIs( ipAddress ).subscribe( ( datum ) => {
			console.log( datum );
			// expect( datum ).toBe( resp.json( ) );
			expect( datum ).toBe( errMsg );
		});
		// use the HttpTestingController to mock requests and the flush method to provide dummy values as responses
		const request = backend.expectOne( whoisUrl );
		expect( request.request.method ).toBe( 'GET' );
		request.flush( errMsg );
		//
	}));
  	//
});
// ===========================================================================
