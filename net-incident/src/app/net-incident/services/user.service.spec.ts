// ===========================================================================
// File: User.service.spec.ts
// Tests of service for: User
//
import { TestBed, getTestBed, inject, waitForAsync } from '@angular/core/testing';
//
import { HttpClient, HttpResponse, HttpErrorResponse, provideHttpClient } from '@angular/common/http';
import { HttpTestingController, TestRequest, provideHttpClientTesting } from '@angular/common/http/testing';
//
import { environment } from '../../../environments/environment';
import { Message } from '../../global/alerts/message';
import { AlertsService } from '../../global/alerts/alerts.service';
import { UserService } from './user.service';
import { IUser, User } from '../user';
import { ServerData } from '../server-data';
import { SelectItemClass } from '../../global/select-item-class';
//
describe('UserService', () => {
	let sut: UserService;
	let http: HttpClient;
	let backend: HttpTestingController;
	const url: string = environment.base_Url + 'User';
	const errMsg: string = 'Fake error';
	const testDate: Date = new Date('2017-01-01T00:00:00');
	const startDate: Date = new Date('2018-03-11T02:00:00');
	const endDate: Date = new Date('2018-11-04T02:00:00');
	const server = new ServerData(
		1, 1, 'NSG', 'Srv 1', 'Members Web-site',
		'Web-site', 'Web-site address: www.nsg.com',
		'We are in Michigan, USA.', 'Phil Huhn', 'Phil', 'PhilHuhn@yahoo.com',
		'EST (UTC-5)', true,  'EDT (UTC-4)', startDate, endDate
	);
	//
	const mockDatum = [
		new User('e0-01','U1','U','N','U N','U','UN1@yahoo.com',true,'734-555-1212',
			true,1, [new SelectItemClass('srv 1','Server 1')],'srv 1',server,[]),
		new User('e0-02','U2','U','N','U N','U','UN2@yahoo.com',true,'734-555-1212',
			true,1, [new SelectItemClass('srv 1','Server 1')],'srv 1',server,[]),
		new User('e0-03','U3','U','N','U N','U','UN3@yahoo.com',true,'734-555-1212',
			true,1, [new SelectItemClass('srv 1','Server 1')],'srv 1',server,[]),
		new User('e0-04','U4','U','N','U N','U','UN4@yahoo.com',true,'734-555-1212',
			true,1, [new SelectItemClass('srv 1','Server 1')],'srv 1',server,[])
	];
	//
	beforeEach( waitForAsync( ( ) => {
		TestBed.configureTestingModule( {
			providers: [
				provideHttpClient(),
				provideHttpClientTesting(),
				{ provide: UserService, useClass: UserService },
				{ provide: AlertsService, useClass: AlertsService }
			]
		} );
		sut = TestBed.inject( UserService );
		TestBed.compileComponents();
	} ) );
	//
	beforeEach(
		inject(
			[ HttpClient, HttpTestingController ],
			( httpClient: HttpClient, httpBackend: HttpTestingController ) => {
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
	it( 'should create ...', ( ) => {
		console.log(
			'===================================\n' +
			'UserService: should create ...' );
		expect( sut ).toBeTruthy( );
	});
	//
	it( 'should create an empty class ...', ( ) => {
		const newData: User = sut.emptyUser( );
		expect( newData.Id ).toEqual( '' );
	});
	//
});
// ===========================================================================
