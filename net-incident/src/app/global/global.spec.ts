// ===========================================================================
// File: global/global.spec.ts
import { LogLevel } from './console-log/log-level.enum';
import { ID, _GLOBAL } from './global';
//
enum Direction {
	Up = "UP",
	Down = "DOWN",
	Left = "LEFT",
	Right = "RIGHT",
}
//
describe( '_GLOBAL', ( ) => {
	/*
	** ID
    ** string | number | bigint | boolean | Date | object | null
	*/
	it( 'ID: should accept string value ...', ( ) => {
		const id: ID = 'test';
		expect( id ).toEqual( 'test' );
	});
    //
	it( 'ID: should accept number value ...', ( ) => {
		const id: ID = 1;
		expect( id ).toEqual( 1 );
	});
    //
	it( 'ID: should accept bigint value ...', ( ) => {
		const id: ID = 100n;
		expect( id ).toEqual( 100n );
	});
    //
	it( 'ID: should accept boolean value ...', ( ) => {
		const id: ID = true;
		expect( id ).toEqual( true );
	});
    //
	it( 'ID: should accept Date value ...', ( ) => {
		const id: ID = new Date( '2000-01-01T00:00:00' );
		expect( id ).toEqual( new Date( '2000-01-01T00:00:00' ) );
	});
    //
	it( 'ID: should accept object value ...', ( ) => {
        const idObj: object = {id: 1, subId: 'a'}; 
		const id: ID = idObj;
		expect( id ).toEqual( idObj );
	});
    //
	it( 'ID: should accept null value ...', ( ) => {
		const id: ID = null;
		expect( id ).toEqual( null );
	});
	/*
	** getEnumKeyByEnumValue
	** number
	*/
	it( 'getEnumKeyByEnumValue: should get LogLevel string value ...', ( ) => {
		const _ret = _GLOBAL.getEnumKeyByEnumValue( LogLevel, LogLevel.Error );
		expect( _ret ).toBe( 'Error' );
	});
	//
	it( 'getEnumKeyByEnumValue: should not get LogLevel string value ...', ( ) => {
		const _ret = _GLOBAL.getEnumKeyByEnumValue( LogLevel, 99 );
		expect( _ret ).toEqual( '--' );
	});
	// strings
	it( 'getEnumKeyByEnumValue: should get Direction string value ...', ( ) => {
		const _ret = _GLOBAL.getEnumKeyByEnumValue( Direction, Direction.Down );
		expect( _ret ).toBe( 'Down' );
	});
	//
	it( 'getEnumKeyByEnumValue: should not get Direction string value ...', ( ) => {
		const _ret = _GLOBAL.getEnumKeyByEnumValue( Direction, 99 );
		expect( _ret ).toEqual( '--' );
	});
	//
});
// ===========================================================================
