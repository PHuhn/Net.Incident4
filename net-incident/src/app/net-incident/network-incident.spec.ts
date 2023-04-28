// ===========================================================================
// File: network-incident.spec.ts
import { INetworkIncident, NetworkIncident } from './network-incident';
import { IIncident, Incident } from './incident';
import { IUser, User } from './user';
//
describe('NetworkLog', () => {
	//
	const data: INetworkIncident = new NetworkIncident( );
	/*
	** Test for class NetworkLog.
	*/
	it('should create an instance ...', () => {
		expect( data ).toBeTruthy();
	});
	//
	it('should assign correct values ...', () => {
		//
		expect( data.incident.IncidentId ).toEqual( 0 );
		expect( data.user.Id ).toEqual( '' );
		//
	});
	//
});
// ===========================================================================
