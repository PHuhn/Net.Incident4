// ===========================================================================
// File: incident-note.spec.ts
import { SelectItemClass, SelectItemExtra } from './select-item-class';
//
describe('SelectItemClass', () => {
	//
	const data: SelectItemClass = new SelectItemClass( 1, 'Lab 1' );
	/*
	** Test for class SelectItemClass.
	*/
	it('SelectItemClass: should create an instance ...', () => {
		expect( data ).toBeTruthy();
	});
	//
	it('SelectItemClass: should assign correct values ...', () => {
		//
		expect( data.value ).toEqual( 1 );
		expect( data.label ).toEqual( 'Lab 1' );
		expect( data.styleClass ).toEqual( undefined );
		//
	});
	/*
	** toString for class SelectItemClass.
	*/
	it('SelectItemClass: toString should output class ...', () => {
		const toStringValue: string = '{"value":1,"label":"Lab 1"}';
		expect( data.toString() ).toEqual( toStringValue );
	});
	/*
	** Clone for class SelectItemClass.
	*/
	it('SelectItemClass: clone should assign correct values ...', () => {
		const _data: SelectItemClass = data.Clone( );
		//
		expect( _data.value ).toEqual( 1 );
		expect( _data.label ).toEqual( 'Lab 1' );
		expect( _data.styleClass ).toEqual( undefined );
		//
	});
	//
});
//
describe('SelectItemExtra', () => {
	//
	const extra: SelectItemExtra = new SelectItemExtra( 1, 'Lab 1', 'Extra 1');
	/*
	** Test for class SelectItemExtra.
	*/
	it('SelectItemExtra: should create of extra an instance ...', () => {
		expect( extra ).toBeTruthy();
	});
	//
	it('SelectItemExtra: should assign correct values to extra ...', () => {
		//
		expect( extra.value ).toEqual( 1 );
		expect( extra.label ).toEqual( 'Lab 1' );
		expect( extra.extra ).toEqual( 'Extra 1' );
		expect( extra.styleClass ).toEqual( undefined );
		//
	});
	/*
	** toString for class SelectItemExtra.
	*/
	it('SelectItemExtra: toString should output class ...', () => {
		const toStringValue: string = '{"value":1,"label":"Lab 1","extra":"Extra 1"}';
		expect( extra.toString() ).toEqual( toStringValue );
	});
	/*
	** Clone for class SelectItemExtra.
	*/
	it('SelectItemExtra: clone should assign correct values ...', () => {
		const _extra: SelectItemExtra = extra.Clone( );
		//
		expect( _extra.value ).toEqual( 1 );
		expect( _extra.label ).toEqual( 'Lab 1' );
		expect( _extra.styleClass ).toEqual( undefined );
		//
	});
	//
});
// ===========================================================================
