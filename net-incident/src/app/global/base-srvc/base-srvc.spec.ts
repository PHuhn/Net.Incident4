// ===========================================================================
// File: base-srvc.ts
import { waitForAsync } from '@angular/core/testing';
import { HttpErrorResponse } from '@angular/common/http';
//
import { ConsoleLogService } from '../console-log/console-log.service';
//
import { BaseSrvc } from './base-srvc';
//
export interface IChangeDateId {
	ChangeDate: Date;
	Id: number;
}
//
describe('BaseSrvc', () => {
	//
	const _console: ConsoleLogService = new ConsoleLogService();
	const sut: BaseSrvc = new BaseSrvc(_console);
	const errMsg: string = 'Fake error';
	const errSrv: string = 'Service error';
	//
	it('should create an instance', () => {
		expect( sut ).toBeTruthy();
	});
	//
	it('baseUrl: should get property', () => {
		const value: string = 'https://abc-xyz.com';
		sut.baseUrl = value;
		expect( sut.baseUrl ).toEqual( value );
	});
	//
	it('baseClassName: should get property', () => {
		const value: string = 'abc-xyz.service';
		sut.baseClassName = value;
		expect( sut.baseClassName ).toEqual( value );
	});
	//
	it('baseTotalRecords: should get property', () => {
		const value: number = 111;
		sut.baseTotalRecords = value;
		expect( sut.baseTotalRecords ).toEqual( value );
	});
	/*
	** baseParamStringify( param: any ): string
	** types: Date/string/number/bigint/boolean/symbol/undefined/object/function
	*/
	it('baseParamStringify: should get RESTFull undefined parameter string ...', () => {
		const param: string = sut.baseParamStringify( null );
		expect( param ).toEqual( '/' );
	});
	//
	it('baseParamStringify: should get RESTFull object parameter string ...', () => {
		const param: string = sut.baseParamStringify( {parm1:1, parm2:'two',parm3:3.1});
		expect( param ).toEqual( '?parm1=1&parm2=two&parm3=3.1' );
	});
	//
	it('baseParamStringify: should get RESTFull object with date parameter string ...', () => {
		const value: IChangeDateId = {ChangeDate: new Date('2023-01-20T11:11:00'), Id:3.1};
		const param: string = sut.baseParamStringify( value );
		expect( param ).toEqual( '?ChangeDate=2023-01-20T11:11:00&Id=3.1' );
	});
	//
	it('baseParamStringify: should get RESTFull numeric parameter string ...', () => {
		const param: string = sut.baseParamStringify( 1 );
		expect( param ).toEqual( '/1' );
	});
	//
	it('baseParamStringify: should get RESTFull string parameter string ...', () => {
		const param: string = sut.baseParamStringify( 'cat' );
		expect( param ).toEqual( '/cat' );
	});
	//
	it('baseParamStringify: should get RESTFull boolean parameter string ...', () => {
		const param: string = sut.baseParamStringify( true );
		expect( param ).toEqual( '/true' );
	});
	//
	it('baseParamStringify: should get RESTFull Date parameter string ...', () => {
		const d: Date =  new Date( '2000-01-01T01:01:01' );
		const param: string = sut.baseParamStringify( d );
		expect( param ).toEqual( '/2000-01-01T01:01:01' );
	});
	//
	it('baseParamStringify: should get RESTFull param parameter string ...', () => {
		const param: string = sut.baseParamStringify( '/1' );
		expect( param ).toEqual( '/1' );
	});
	/*
	** baseSrvcErrorHandler( error: any )
	*/
	it( 'baseSrvcErrorHandler: should throw an any error ...', waitForAsync( ( ) => {
		// given
		const resp: string = errMsg;
		// when
		sut.baseSrvcErrorHandler( resp ).subscribe({
			next: ( ) => {
				console.log( JSON.stringify( resp ) );
				fail( 'handleError: expected error...' );
			},
			error: (error: HttpErrorResponse | string) => {
				// then
				expect( error.toString() ).toEqual( `Error: ${errMsg}` );
			}
		});
		//
	} ) );
	//
	it( 'baseSrvcErrorHandler: should throw an any service error ...', waitForAsync( ( ) => {
		// given
		const resp: string = '';
		// when
		sut.baseSrvcErrorHandler( resp ).subscribe({
			next: ( ) => {
				console.log( JSON.stringify( resp ) );
				fail( 'handleError: expected error...' );
			},
			error: (error: HttpErrorResponse | string) => {
				// then
				expect( error.toString() ).toEqual( `Error: ${errSrv}` );
			}
		});
		//
	} ) );
	//
	it( 'baseSrvcErrorHandler: should throw a HttpErrorResponse error...', waitForAsync( ( ) => {
		// given
		const resp: HttpErrorResponse = new HttpErrorResponse({
			error: {}, status: 599, statusText: errMsg
		});
		// when
		sut.baseSrvcErrorHandler( resp ).subscribe({
			next: (val) => {
				console.log( `${JSON.stringify( resp )}, ${val}` );
				fail( 'handleError: expected error...' );
			},
			error: (error: HttpErrorResponse | string) => {
				// then
				expect( error.toString() ).toEqual( `Error: ${errMsg} (599)` );
			}
		});
		//
	} ) );
	//
});
// ===========================================================================
