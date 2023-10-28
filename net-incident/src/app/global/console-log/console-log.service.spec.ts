// ===========================================================================
import { TestBed } from '@angular/core/testing';
import { environment } from '../../../environments/environment';
import { ConsoleLogService } from './console-log.service';
import { LogLevel } from './log-level.enum';
//
fdescribe( 'ConsoleLogService', ( ) => {
	let sut: ConsoleLogService;
	//
	beforeEach(( ) => {
		TestBed.configureTestingModule({});
		sut = TestBed.inject(ConsoleLogService);
		sut.logLevel = LogLevel.Debug;
	});
	//
	it( 'should be created', ( ) => {
		console.log(
			'===================================\n' +
			'ConsoleLogService should create ...' );
		// given / when
		console.log( `Log level ${environment.logLevel}` );
		// then
		expect( sut ).toBeTruthy( );
	});
	//
	it( 'should set log-level ...', ( ) => {
		// given
		sut.logLevel = LogLevel.Verbose;
		// when
		const _ret = sut.logLevel;
		// then
		expect( _ret ).toBe( LogLevel.Verbose );
	});
	//
	it( 'Error should create Error message ...', ( ) => {
		// given / when
		const _ret = sut.Error( 'Error message' );
		// then
		expect( _ret ).toEqual( 'Error: Error message' );
	});
	//
	it( 'Warning should create Warning message ...', ( ) => {
		// given / when
		const _ret = sut.Warning( 'Warning message' );
		// then
		expect( _ret ).toEqual( 'Warning: Warning message' );
	});
	//
	it( 'Success should create Success message ...', ( ) => {
		// given / when
		const _ret = sut.Success( 'Success message' );
		// then
		expect( _ret ).toEqual( 'Success: Success message' );
	});
	//
	it( 'Information should create Information message ...', ( ) => {
		// given / when
		const _ret = sut.Information( 'Information message' );
		// then
		expect( _ret ).toEqual( 'Info: Information message' );
	});
	//
	it( 'Debug should create Debug message ...', ( ) => {
		// given / when
		const _ret = sut.Debug( 'Debug message' );
		// then
		expect( _ret ).toEqual( 'Debug: Debug message' );
	});
	//
	it( 'Verbose should create Verbose message ...', ( ) => {
		// given
		sut.logLevel = LogLevel.Verbose;
		// when
		const _ret = sut.Verbose( 'Verbose message' );
		// then
		expect( _ret ).toEqual( 'Verbose: Verbose message' );
	});
	//
	it( 'Verbose should not print log message ...', ( ) => {
		// given
		sut.logLevel = LogLevel.Error;
		// when
		const _ret = sut.Verbose( 'Verbose message' );
		// then
		expect( _ret ).toEqual( '' );
	});
	//
	it( 'LogMessage should log Unknown message ...', ( ) => {
		// test a private method
		// given / when
		const _ret = ( sut as any ).LogMessage( -1, 'Test message' );
		// then
		expect( _ret ).toEqual( 'Unknown: Test message' );
	});
	/*
	** getEnumKeyByEnumValue
	*/
	it( 'should get LogLevel string value ...', ( ) => {
		const _ret = sut.getEnumKeyByEnumValue( LogLevel, LogLevel.Error );
		expect( _ret ).toBe( 'Error' );
	});
	//
	it( 'should not get LogLevel string value ...', ( ) => {
		const _ret = sut.getEnumKeyByEnumValue( LogLevel, 99 );
		expect( _ret ).toEqual( '--' );
	});
	/*
	** trace
	*/
	it( 'should get trace string value ...', ( ) => {
		// given / when
		const _ret = sut.Trace( 'Trace message' );
		// then
		expect( _ret ).toEqual( 'Trace: Trace message' );
	});
	/*
	** Circular messages
	*/
	it( 'lastMessage: should return the last register console message ...', ( ) => {
		sut.Error( 'Error message' );
		expect( sut.lastMessage ).toEqual( 'Error: Error message' );
		sut.Warning( 'Warning message' );
		expect( sut.lastMessage ).toEqual( 'Warning: Warning message' );
		sut.Information( 'Information message' );
		expect( sut.lastMessage ).toEqual( 'Info: Information message' );
	});
	//
	it( 'messages: should return the whole list of messages ...', ( ) => {
		sut.Information( '1' );
		sut.Information( '2' );
		sut.Information( '3' );
		sut.Information( '4' );
		sut.Information( '5' );
		sut.Information( '6' );
		sut.Information( '7' );
		sut.Information( '8' );
		sut.Information( '9' );
		sut.Information( '10' );
		sut.Error( 'Last message' );
		const msgs: string[] = sut.messages;
		console.warn( msgs );
		expect( msgs ).toEqual( 
			['Error: Last message', 'Info: 10','Info: 9','Info: 8','Info: 7','Info: 6','Info: 5','Info: 4','Info: 3','Info: 2'] );
	});
	//
});
