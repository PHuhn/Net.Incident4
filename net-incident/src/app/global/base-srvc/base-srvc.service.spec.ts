// ===========================================================================
// File: base-srvc.service.spec.ts
import { TestBed, waitForAsync } from '@angular/core/testing';
//
import { HttpErrorResponse, provideHttpClient } from '@angular/common/http';
import { HttpTestingController, TestRequest, provideHttpClientTesting } from '@angular/common/http/testing';
//
import { LazyLoadMeta } from 'primeng/api';
//
import { ILazyResults } from './ilazy-results';
import { IAuthResponse, AuthResponse } from '../../public/login/iauth-response';
import { ConsoleLogService } from '../console-log/console-log.service';
import { BaseSrvcService } from './base-srvc.service';
//
export interface ITestNote {
	TestNoteId: number;
	TestNoteShortDesc: string;
	TestNoteDesc: string;
}
//
export class TestNote implements ITestNote {
	constructor(
		public TestNoteId: number,
		public TestNoteShortDesc: string,
		public TestNoteDesc: string,
	) { }
}
//
describe('BaseSrvcService', () => {
	let sut: BaseSrvcService;
	// let http: HttpClient;
	let backend: HttpTestingController;
	const status: number = 401;
	const errMsg: string = 'Fake error';
	const className: string = 'TestNote';
	const url: string = 'http://localhost/' + className;
	const mockModels: ITestNote[] = [
		new TestNote( 1, 'type 1', 'type 1 description' ),
		new TestNote( 2, 'type 2', 'type 2 description' ),
		new TestNote( 3, 'type 3', 'type 2 description' ),
		new TestNote( 4, 'type 4', 'type 4 description' ),
		new TestNote( 5, 'type 5', 'type 5 description' ),
		new TestNote( 6, 'type 6', 'type 6 description' )
	];
	//
	beforeEach( waitForAsync( ( ) => {
		TestBed.configureTestingModule( {
			providers: [
				provideHttpClient(),
				provideHttpClientTesting(),
				BaseSrvcService,
				ConsoleLogService
			]
		} );
		// Setup injected pre service for each test
		// http = TestBed.inject( HttpClient );
		backend = TestBed.inject( HttpTestingController );
		// Setup sut
		sut = TestBed.inject( BaseSrvcService );
		TestBed.compileComponents();
		sut.baseUrl = url;
		sut.baseClassName = className;
	} ) );
	//
	it('should be created', () => {
		expect( sut ).toBeTruthy();
	});
	/*
	** getModelAll<T>( ): Observable<T[]>
	*/
	it( 'getModelAll: should get all model results...', waitForAsync( ( ) => {
		// given / when
		sut.getModelAll<ITestNote>().subscribe({
			next: (datum: ITestNote[]) => {
				expect( datum.length ).toBe( 6 );
				expect( datum[ 0 ].TestNoteId ).toEqual( 1 );
				expect( datum[ 1 ].TestNoteId ).toEqual( 2 );
				expect( datum[ 2 ].TestNoteId ).toEqual( 3 );
				expect( datum[ 3 ].TestNoteId ).toEqual( 4 );
				},
			error: (error: HttpErrorResponse) => {
				fail( `getModelById: expected error... ${error}` );
			}
		});
		// then
		const request: TestRequest = backend.expectOne( `${url}/` );
		expect( request.request.method ).toBe( 'GET' );
		request.flush( mockModels );
		//
	}));
	/*
	** postJsonBody<T>( body: any ): Observable<T | never>
	** Read (get) lazy loading page of models.
	*/
	it( 'postJsonBody: should authenticate user...', waitForAsync( ( ) => {
		// given
		const _url = 'http://localhost/authenticate';
		sut.baseUrl = _url;
		const _body = {"Username": 'Phil', "Password": 'password' };
		const _token = '123456789012345678901234';
		// 2023-04-23T18:03:55.715Z
		const _srvcResults: IAuthResponse
			= new AuthResponse( _token,
				(new Date()).toISOString() );
		// when
		sut.postJsonBody<IAuthResponse>( _body ).subscribe({
			next: (response: IAuthResponse) => {
				// then
				console.log( response );
				expect( response.token ).toEqual( _token );
				expect( response.expiration.length ).toEqual( 24 );
			},
			error: (error: HttpErrorResponse) => {
				fail( `postJsonBody: expected error... ${error}` );
			},
			complete: () => { }
		});
		// then
		const request: TestRequest = backend.expectOne( `${_url}/` );
		expect( request.request.method ).toBe( 'POST' );
		request.flush( _srvcResults );
	}));
	/*
	** getModelLazy<T>( event: LazyLoadMeta ): Observable<ILazyResults | never>
	** Read (get) lazy loading page of models.
	*/
	it( 'getModelLazy: should get page of data...', waitForAsync( ( ) => {
		// given
		const event: LazyLoadMeta = {
			first: 0, rows: 4,
		};
		const srvcResults: ILazyResults<ITestNote> = {
			results: mockModels.filter( ftr => ftr.TestNoteId < 5 ),
			totalRecords: 4,
			loadEvent: JSON.stringify( event ),
			message: ''
		};
		// when
		sut.getModelLazy<ITestNote>( event ).subscribe({
			next: (results: ILazyResults<ITestNote>) => {
				// then
				expect( results.results.length ).toBe( 4 );
				expect( results.results[ 0 ].TestNoteId ).toEqual( 1 );
				expect( results.results[ 1 ].TestNoteId ).toEqual( 2 );
			},
			error: (error: HttpErrorResponse) => {
				fail( `getModelLazy: expected error... ${error}` );
			}
		});
		// then
		const request: TestRequest = backend.expectOne( `${url}/` );
		expect( request.request.method ).toBe( 'POST' );
		request.flush( srvcResults );
	}));
	/*
	** getModelSome<T>( param: any ): Observable<T[] | never>
	** Read (get) some models.
	** @param param object or data type
	*/
	it( 'getModelSome: should get some data via array...', waitForAsync( ( ) => {
		// given
		const param: object = {
			TestNoteShortDesc: ['type 3', 'type 4']
		};
		const srvcParam: string = sut.baseParamStringify( param );
		// when
		sut.getModelSome<ITestNote>( param ).subscribe({
			next: (results: ITestNote[]) => {
				// then
				expect( results.length ).toBe( 2 );
				expect( results[ 0 ].TestNoteId ).toEqual( 3 );
				expect( results[ 1 ].TestNoteId ).toEqual( 4 );
			},
			error: (error: HttpErrorResponse) => {
				fail( `getModelLazy: expected error... ${error}` );
			}
		});
		// then
		const request: TestRequest = backend.expectOne( `${url}${srvcParam}` );
		expect( request.request.method ).toBe( 'GET' );
		request.flush( [mockModels[2], mockModels[3]] );
	}));
	//
	it( 'getModelSome: should get some data...', waitForAsync( ( ) => {
		// given
		const param: object = {
			TestNoteShortDesc: 'type 3',
		};
		const srvcParam: string = sut.baseParamStringify( param );
		// when
		sut.getModelSome<ITestNote>( param ).subscribe({
			next: (results: ITestNote[]) => {
				// then
				expect( results.length ).toBe( 1 );
				expect( results[ 0 ].TestNoteId ).toEqual( 3 );
			},
			error: (error: HttpErrorResponse) => {
				fail( `getModelLazy: expected error... ${error}` );
			}
		});
		// then
		const request: TestRequest = backend.expectOne( `${url}${srvcParam}` );
		expect( request.request.method ).toBe( 'GET' );
		// returns an array
		request.flush( [mockModels[2] as ITestNote] );
	}));
	/*
	** getModelById( id: string ): Observable<T>
	*/
	it( 'getModelById: should get by id (primary key)...', waitForAsync( ( ) => {
		// given
		const mockData: ITestNote = mockModels[ 1 ];
		const id1: number = mockData.TestNoteId;
		// when
		sut.getModelById<ITestNote>( id1 ).subscribe({
			// then
			next: (value: ITestNote) => {
				expect( value.TestNoteId ).toEqual( id1 );
			},
			error: (error: HttpErrorResponse) => {
				fail( `getModelById: expected error...  ${error}` );
			}
		});
		// use the HttpTestingController to mock requests and the flush method to provide dummy values as responses
		// then
		const request: TestRequest = backend.expectOne( `${url}/${id1}` );
		expect( request.request.method ).toBe( 'GET' );
		request.flush( mockData as ITestNote );
		//
	}));
	/*
	** Get text service.
	** @param id 
	** @returns text/string
	** getText( id: ID ): Observable<string | never>
	*/
	it( 'getText: should create a new row...', waitForAsync( ( ) => {
		// given
		const mockResponse: string = 'Some Text';
		const id1: object = { a:'some', b: 'Text' };
		// when
		sut.getText( id1 ).subscribe({
			// then
			next: (value: string) => {
				expect( value ).toEqual( mockResponse );
			},
			error: (error: HttpErrorResponse) => {
				fail( 'getText: expected error... ${error}' );
			},
			complete: () => { }
		});
		// then
		const request: TestRequest = backend.expectOne( `${url}?a=some&b=Text` );
		expect( request.request.method ).toBe( 'GET' );
		request.flush( mockResponse );
		//
	} ) );
	/*
	** createModel<T>( model: T ): Observable<T | undefined>
	*/
	it( 'createModel: should create a new row...', waitForAsync( ( ) => {
		// given
		const mockData: ITestNote = mockModels[ 2 ];
		const id1: number = mockData.TestNoteId;
		// when
		sut.createModel<ITestNote>( mockData ).subscribe({
			// then
			next: (value: ITestNote) => {
				expect( value.TestNoteId ).toEqual( id1 );
			},
			error: (error: HttpErrorResponse) => {
				fail( `deleteModel: expected error... ${error}` );
			}
		});
		// then
		const request: TestRequest = backend.expectOne( `${url}/` );
		expect( request.request.method ).toBe( 'POST' );
		request.flush( mockData );
		//
	} ) );
	// handle a create error
	it( 'createModel: should handle an error on create...', waitForAsync( ( ) => {
		// given
		const mockData: ITestNote = mockModels[ 2 ];
		const errResp: HttpErrorResponse = new HttpErrorResponse({
			error: {}, status: status, statusText: errMsg
		});
		// when
		sut.createModel<ITestNote>( mockData ).subscribe({
			// then
			next: (value: ITestNote) => {
				console.log( 'createModel: expected error response:' );
				console.log( value );
				fail( 'createModel: expected error...' );
			},
			error: (error: HttpErrorResponse) => {
				expect( error.toString() ).toEqual( `Error: ${errMsg} (${status})` );
			}
		});
		// then
		const request: TestRequest = backend.expectOne( `${url}/` );
		expect( request.request.method ).toBe( 'POST' );
		request.flush( 'Invalid request parameters', errResp );
		//
	} ) );
	/*
	** updateModel<T>( id: string, model: T ): Observable<T | undefined>
	*/
	it( 'updateModel: should update Model row...', waitForAsync( ( ) => {
		// given
		const mockData: ITestNote = mockModels[ 1 ];
		const id1: number = mockData.TestNoteId;
		const param: string = sut.baseParamStringify( id1 );
		// when
		sut.updateModel( param, mockData ).subscribe({
			// then
			next: (value: ITestNote) => {
				expect( value.TestNoteId ).toEqual( id1 );
			},
			error: (error: HttpErrorResponse) => {
				fail( `updateModel: expected error...  ${error}` );
			}
		});
		// then
		const request = backend.expectOne( `${url}${param}` );
		expect( request.request.method ).toBe( 'PUT' );
		request.flush( mockData );
		//
	} ) );
	// handle a create error
	it( 'updateModel: should handle an error on update ...', waitForAsync( ( ) => {
		// given
		const mockData: ITestNote = mockModels[ 2 ];
		const id1: number = mockData.TestNoteId;
		const param: string = sut.baseParamStringify( id1 );
		const errResp: HttpErrorResponse = new HttpErrorResponse({
			error: {}, status: 401, statusText: errMsg
		});
		// when
		sut.updateModel<ITestNote>( param, mockData ).subscribe({
			// then
			next: (value: ITestNote) => {
				console.log( 'updateModel: expected error response:' );
				console.log( value );
				fail( 'updateModel: expected error...' );
			},
			error: (error: HttpErrorResponse) => {
				expect( error.toString() ).toEqual( `Error: ${errMsg} (${status})` );
			}
		});
		// then
		const request = backend.expectOne( `${url}${param}` );
		expect( request.request.method ).toBe( 'PUT' );
		request.flush( 'Invalid request parameters', errResp );
		//
	} ) );
	/*
	** deleteModel<T>( id: string ): Observable<T | undefined>
	*/
	it( 'deleteModel: should delete Model row...', waitForAsync( ( ) => {
		// given
		const mockData: ITestNote = mockModels[ 3 ];
		const id1: number = mockData.TestNoteId;
		const param: string = sut.baseParamStringify( id1 );
		// when
		sut.deleteModel<ITestNote>( id1 ).subscribe({
			// then
			next: (value: ITestNote) => {
				expect( value.TestNoteId ).toEqual( id1 );
			},
			error: (error: HttpErrorResponse) => {
				fail( `deleteModel: expected error... ${error}` );
			}
		});
		// then
		const request = backend.expectOne( `${url}${param}` );
		expect( request.request.method ).toBe( 'DELETE' );
		request.flush( mockData );
		//
	} ) );
	// handle a create error
	it( 'deleteModel: should handle an error on delete ...', waitForAsync( ( ) => {
		// given
		const mockData: ITestNote = mockModels[ 2 ];
		const id1: number = mockData.TestNoteId;
		const param: string = sut.baseParamStringify( id1 );
		const errResp: HttpErrorResponse = new HttpErrorResponse({
			error: {}, status: 401, statusText: errMsg
		});
		// when
		sut.deleteModel<ITestNote>( id1 ).subscribe({
			// then
			next: (value: ITestNote) => {
				console.log( 'deleteModel: expected error response:' );
				console.log( value );
				fail( 'deleteModel: expected error...' );
			},
			error: (error: HttpErrorResponse) => {
				expect( error.toString() ).toEqual( `Error: ${errMsg} (${status})` );
			}
		});
		// then
		const request = backend.expectOne( `${url}${param}` );
		expect( request.request.method ).toBe( 'DELETE' );
		request.flush( 'Invalid request parameters', errResp );
		//
	} ) );
	//
});
// ===========================================================================
