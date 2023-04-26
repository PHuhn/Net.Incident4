// ===========================================================================
// File: login.spec.ts
//
import { TestBed, getTestBed } from '@angular/core/testing';
//
import { Login } from './login';
import { Message } from '../global/alerts/message';
import { AlertLevel } from '../global/alerts/alert-level.enum';
//
describe('Login', () => {
	//
	const _data: Login = new Login( 'UserName', 'Password', 'ShortName' );
	//
	it('should be created ...', ( ) => {
		expect( _data ).toBeTruthy();
	});
	/*
	** validate( model: IIncident, add: boolean ): Message[]
	*/
	it( 'validate: should validate...', () => {
		// given / when
		const ret: Message[] = _data.validate( );
		// then
		expect( ret.length ).toEqual( 0 );
		//
	});
	//
	it( 'validate: should handle a validation UserName required error ...', ( ) => {
		// given
        const _data: Login = new Login( 'UserName', 'Password', 'ShortName' );
		const _userNameBad: any = '';
		const _badLogin: Login = new Login( _userNameBad, 'Password', 'ShortName' );
		// when
		const ret: Message[] = _badLogin.validate( );
		// then (is required.)
		expect( ret.length ).toEqual( 1 );
		expect( ret[0].message.indexOf( 'required' ) !== -1 ).toBe( true );
	} );
	//
	it( 'validate: should handle a validation UserName is too long ...', ( ) => {
		// given
        const _data: Login = new Login( 'UserName', 'Password', 'ShortName' );
		const _userNameBad: any = 'u'.repeat(257);
		const _badLogin: Login = new Login( _userNameBad, 'Password', 'ShortName' );
		// when
		const ret: Message[] = _badLogin.validate( );
		// then (is required.)
		expect( ret.length ).toEqual( 1 );
		expect( ret[0].message.indexOf( 'max length' ) !== -1 ).toBe( true );
	} );
	//
	it( 'validate: should handle a validation Password required error ...', ( ) => {
		// given
		const _passwordBad: any = '';
		const _badLogin: Login = new Login( 'UserName', _passwordBad, 'ShortName' );
		// when
		const ret: Message[] = _badLogin.validate( );
		// then (is required.)
		expect( ret.length ).toEqual( 1 );
		expect( ret[0].message.indexOf( 'required' ) !== -1 ).toBe( true );
	} );
	//
	it( 'validate: should handle a validation Password is too long ...', ( ) => {
		// given
		const _passwordBad: any = 'p'.repeat(130);
		const _badLogin: Login = new Login( 'UserName', _passwordBad, 'ShortName' );
		// when
		const ret: Message[] = _badLogin.validate( );
		// then (is required.)
		expect( ret.length ).toEqual( 1 );
		expect( ret[0].message.indexOf( 'max length' ) !== -1 ).toBe( true );
	} );
	//
});
// ===========================================================================
