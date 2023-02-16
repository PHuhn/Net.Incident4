// ===========================================================================
// File: Incident.service.spec.ts
// Tests of service for: Incident
//
import { TestBed, getTestBed, inject, waitForAsync } from '@angular/core/testing';
//
import { HttpClientModule, HttpClient, HttpErrorResponse, HttpRequest, HttpResponseBase } from '@angular/common/http';
import { HttpClientTestingModule, HttpTestingController, TestRequest } from '@angular/common/http/testing';
//
import { LazyLoadEvent } from 'primeng/api';
//
import { environment } from '../../../environments/environment';
import { Message } from '../../global/alerts/message';
// import { AlertsService } from '../../global/alerts/alerts.service';
import { IncidentService } from './incident.service';
import { IIncident, Incident } from '../incident';
import { ILazyResults } from '../../common/base-srvc/ibase-srvc';
//
describe('IncidentService', () => {
	let sut: IncidentService;
	let http: HttpClient;
	let backend: HttpTestingController;
	const url: string = environment.base_Url + 'Incidents';
	const errMsg: string = 'Fake error';
	const testDate: Date = new Date('2000-01-01T00:00:00');
	//
	const mockDatum = [
		new Incident( 1,1,'192.1','ripe.net','nn1','a@1.com','',true,true,true,'i 1',testDate ),
		new Incident( 2,1,'192.2','ripe.net','nn2','a@2.com','',false,false,false,'i 2',testDate ),
		new Incident( 3,1,'192.3','arin.net','nn3','a@3.com','',true,true,true,'i 3',testDate ),
		new Incident( 4,1,'192.4','ripe.net','nn4','a@4.com','',false,false,false,'i 4',testDate ),
		new Incident( 5,2,'192.5','ripe.net','nn4','a@4.com','',false,false,false,'i 4',testDate ),
		new Incident( 6,1,'192.6','arin.net','nn3','a@3.com','',true,true,true,'i 3',testDate ),
		new Incident( 7,1,'192.7','arin.net','nn3','a@3.com','',true,true,true,'i 3',testDate ),
		new Incident( 8,1,'192.8','arin.net','nn3','a@3.com','',true,true,true,'i 3',testDate ),
		new Incident( 9,1,'192.9','arin.net','nn3','a@3.com','',true,true,true,'i 3',testDate )
	];
	//
	beforeEach( waitForAsync( ( ) => {
		TestBed.configureTestingModule( {
			imports: [
				// HttpClient 4.3 testing
				HttpClientModule,
				HttpClientTestingModule
			],
			providers: [
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
