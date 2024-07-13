// ===========================================================================
// File: Incident.service.spec.ts
// Tests of service for: Incident
//
import { TestBed, waitForAsync } from '@angular/core/testing';
//
import { HttpClient, provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
//
import { IncidentService } from './incident.service';
import { Incident } from '../incident';
//
describe('IncidentService', () => {
	let sut: IncidentService;
	let http: HttpClient;
	let backend: HttpTestingController;
	//
	beforeEach( waitForAsync( ( ) => {
		TestBed.configureTestingModule( {
			providers: [
				provideHttpClient(),
				provideHttpClientTesting(),
				IncidentService
			]
		} );
		// Setup injected pre service for each test
		http = TestBed.inject( HttpClient );
		backend = TestBed.inject( HttpTestingController );
		// Setup sut
		sut = TestBed.inject( IncidentService );
		TestBed.compileComponents();
	} ) );
	//
	afterEach( ( ) => {
		// cleanup
		backend.verify();
	});
	//
	it( 'should create ...', ( ) => {
		console.log(
			'===================================\n' +
			'IncidentService should create ...' );
		expect( sut ).toBeTruthy( );
	});
	//
	it( 'should create an empty class ...', ( ) => {
		const newData: Incident = sut.emptyIncident( );
		expect( newData.IncidentId ).toEqual( 0 );
	});
	//
});
// ===========================================================================
