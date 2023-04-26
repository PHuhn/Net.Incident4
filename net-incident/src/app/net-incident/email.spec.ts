// ===========================================================================
// File: email.spec.ts
//
import { TestBed, getTestBed } from '@angular/core/testing';
//
import { EmailAddress, EmailContent,
        EmailPersonalization, EmailRequest } from './email';
//
describe('Email', () => {
	//
	const _emailAddress: EmailAddress = new EmailAddress( 'UserName@any.com' );
	const _emailContent: EmailContent = new EmailContent('content');
	const _emailPersonalization: EmailPersonalization =
		new EmailPersonalization( 'to@any.com', 'email subject');
	const _emailRequest: EmailRequest =
        new EmailRequest('from@anycompany.com', 'to@any.com', 'email subject', 'body content');
	//
	it('EmailAddress: should be created ...', ( ) => {
		expect( _emailAddress ).toBeTruthy();
	});
	//
	it('EmailContent: should be created plain text ...', ( ) => {
		expect( _emailContent ).toBeTruthy();
		expect( _emailContent.type ).toEqual( 'text/plain' );
	});
	//
	it('EmailContent: should be created html text ...', ( ) => {
		const _emailContent: EmailContent = new EmailContent('<body>content</body>');
		expect( _emailContent ).toBeTruthy( );
		expect( _emailContent.type ).toEqual( 'text/html' );
	});
	//
	it('EmailPersonalization: should be created ...', ( ) => {
		expect( _emailPersonalization ).toBeTruthy();
	});
	//
	it('EmailRequest: should be created ...', ( ) => {
		expect( _emailRequest ).toBeTruthy();
	});
	//
});
// ===========================================================================
