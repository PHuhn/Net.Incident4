// ===========================================================================
// File: incident.spec.ts
import { IIncident, Incident } from './incident';
//
describe('Incident', () => {
	//
	const data: IIncident = new Incident( 1, 1, 'i 1', 'i 1', 'i 1', 'i 1', 'i 1', true, true, true, 'i 1', new Date( '2000-01-01T00:00:00' ) );
	/*
	** Test for class Incident.
	*/
	it('should create an instance ...', () => {
		expect( data ).toBeTruthy();
	});
	//
	it('should assign correct values ...', () => {
		//
		expect( data.IncidentId ).toEqual( 1 );
		expect( data.ServerId ).toEqual( 1 );
		expect( data.IPAddress ).toEqual( 'i 1' );
		expect( data.NIC ).toEqual( 'i 1' );
		expect( data.NetworkName ).toEqual( 'i 1' );
		expect( data.AbuseEmailAddress ).toEqual( 'i 1' );
		expect( data.ISPTicketNumber ).toEqual( 'i 1' );
		expect( data.Mailed ).toEqual( true );
		expect( data.Closed ).toEqual( true );
		expect( data.Special ).toEqual( true );
		expect( data.Notes ).toEqual( 'i 1' );
		expect( data.CreatedDate ).toEqual( new Date( '2000-01-01T00:00:00' ) );
		//
	});
	/*
	** Clone for class Incident.
	*/
	it('clone should assign correct values ...', () => {
		const _data: Incident = data.Clone( );
		//
		expect( _data.IncidentId ).toEqual( 1 );
		expect( _data.ServerId ).toEqual( 1 );
		expect( _data.IPAddress ).toEqual( 'i 1' );
		expect( _data.NIC ).toEqual( 'i 1' );
		expect( _data.NetworkName ).toEqual( 'i 1' );
		expect( _data.AbuseEmailAddress ).toEqual( 'i 1' );
		expect( _data.ISPTicketNumber ).toEqual( 'i 1' );
		expect( _data.Mailed ).toEqual( true );
		expect( _data.Closed ).toEqual( true );
		expect( _data.Special ).toEqual( true );
		expect( _data.Notes ).toEqual( 'i 1' );
		expect( _data.CreatedDate ).toEqual( new Date( '2000-01-01T00:00:00' ) );
		//
	});
	/*
	** toString for class Incident.
	*/
	it('toString should output class ...', () => {
		const toStringValue: string = '{"IncidentId":1,"ServerId":1,"IPAddress":"i 1","NIC":"i 1","NetworkName":"i 1","AbuseEmailAddress":"i 1","ISPTicketNumber":"i 1","Mailed":true,"Closed":true,"Special":true,"Notes":"i 1","CreatedDate":"2000-01-01T05:00:00.000Z","IsChanged":false}';
		expect( data.toString() ).toEqual( toStringValue );
	});
	//
});
// ===========================================================================
