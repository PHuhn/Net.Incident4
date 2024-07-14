// ===========================================================================
// File: AbuseEmailReport.spec.ts
import { environment } from '../../environments/environment';
import { Message } from '../global/alerts/message';
import { IIncident, Incident } from './incident';
import { INetworkLog, NetworkLog } from './network-log';
import { NetworkIncident } from './network-incident';
import { IIncidentType, IncidentType } from './incident-type';
import { EmailAddress, EmailRequest } from './email';
import { IUser, User } from './user';
import { ServerData } from './server-data';
import { SelectItemClass } from '../global/primeng/select-item-class';
//
import { TestBed, getTestBed } from '@angular/core/testing';
import { IAbuseEmailReport, AbuseEmailReport } from './abuse-email-report';
//
describe('AbuseEmailReport', () => {
	//
	let sut: AbuseEmailReport;
	const startDate: Date = new Date('2018-03-11T02:00:00');
	const endDate: Date = new Date('2018-11-04T02:00:00');
	const standardDate: Date = new Date('2018-03-10T23:00:00');
	const daylightDate: Date = new Date('2018-03-11T02:01:00');
	const ipAddr: string = '192.169.1.1';
	//
	const serverMock = new ServerData(
		1, 1, 'NSG', 'Srv 1', 'Members Web-site',
		'Web-site', 'Web-site address: www.nsg.com',
		'We are in Michigan, USA.', 'User Admin', 'Admin', 'ServerAdmin@yahoo.com',
		'EST (UTC-5)', true,  'EDT (UTC-4)', startDate, endDate
	);
	//
	const userMock = new User('e0-04','Head','Head','Admin','Head Admin','U','UN4@yahoo.com',
		true,'734-555-1212', true,1, [new SelectItemClass('srv 1','Server 1')],'srv 1', undefined, ['admin']);
	//
	const inc: Incident = new Incident( 4,1,ipAddr,'arin.net','PSG169',
		'dandy@psg.com','',false,false,false,'',standardDate );
	//
	const logSqlData = [
		new NetworkLog( 6,1,4,ipAddr,standardDate,'Log incident 1',3, 'SQL', true ),
		new NetworkLog( 7,1,4,ipAddr,daylightDate,'Log incident 2',3, 'SQL', true ),
		new NetworkLog( 8,1,null,'192.3',new Date( '2018-02-27T00:00:00' ),'Log incident 3',1, 'SQL', false )
	];
	//
	const logMultiData = [
		new NetworkLog( 6,1,4,ipAddr,standardDate,'Log incident 1',3, 'SQL', true ),
		new NetworkLog( 7,1,4,ipAddr,daylightDate,'Log incident 2',5, 'XSS', true ),
		new NetworkLog( 8,1,null,'192.3',new Date( '2018-02-27T00:00:00' ),'Log incident 3',1, 'SQL', false )
	];
	//
	const incidentTypes = [
		new IncidentType( 1, 'Unk', 'Unknown', true, 'Unknown probe from ${IPAddress}', 'Hi\\n\\nStop the intrusion from your IP address ${IPAddress}.\\nThe following IP address probe my network.\\nPlease contain the following reference # in all communications: ${IncidentId}\\n\\n${Device}\\n${ServerLocation}\\nIncident times:', '${NetworkLogDate} ${TimeZone}', '\\nThank you,\\n${FromName}\\n================', '\\n${Log}\\n--------------------------------', '-' ),
		new IncidentType( 2, 'Multiple', 'Multiple Types', true, 'Network abuse from ${IPAddress}', 'Hi\\n\\nStop the intrusion from your IP address ${IPAddress}.\\nThe following IP address probe my network, probing for multiple vulnerabilities.\\nPlease contain the following reference # in all communications: ${IncidentId}\\n\\n${Device}\\n${ServerLocation}\\nIncident times:', '${IncidentTypeShortDesc}: ${NetworkLogDate} ${TimeZone}', '\\nThank you,\\n${FromName}\\n================', '\\n${Log}\\n--------------------------------', '-' ),
		new IncidentType( 3, 'SQL', 'SQL Injection', true, 'SQL Injection probe from ${IPAddress}', 'Hi\\n\\nStop the intrusion from your IP address ${IPAddress}.  This is testing SQL injection vulnerabilities.\\nPlease contain the following reference # in all communications: ${IncidentId}\\n\\n${Device}\\n${ServerLocation}\\n\\nIncident times:', '${NetworkLogDate} ${TimeZone}', '\\nThank you,\\n${FromName}\\n================', '\\n${Log}\\n--------------------------------', '-' ),
		new IncidentType( 4, 'PHP', 'PHP', true, 'PHP probe from ${IPAddress}', 'Hi\\n\\nStop the intrusion from your IP address ${IPAddress}.\\nThe following IP address probe my network, probing for ${IncidentTypeDesc} vulnerabilities.\\nPlease use the following reference # in all communications: ${IncidentId}\\n\\n${Device}\\n${ServerLocation}\\n\\nIncident times:', '${NetworkLogDate} ${TimeZone}', '\\nThank you,\\n${FromName}\\n================', '\\n${Log}\\n--------------------------------', '-' ),
		new IncidentType( 5, 'XSS', 'Cross Site Scripting', true, 'XSS probe from ${IPAddress}', 'Hi\\n\\nStop the intrusion from your IP address ${IPAddress}.\\nThe following IP address probe my network, probing for ${IncidentTypeDesc} vulnerabilities.\\nPlease use the following reference # in all communications: ${IncidentId}\\n\\n${Device}\\n${ServerLocation}\\n\\nIncident times:', '${NetworkLogDate} ${TimeZone}', '\\nThank you,\\n${FromName}\\n================', '\\n${Log}\\n--------------------------------', '-' ),
		new IncidentType( 6, 'VS', 'ViewState', true, 'ViewState probe from ${IPAddress}', 'Hi\\n\\nStop the intrusion from your IP address ${IPAddress}.\\nThe following IP address probe my network, probing for ${IncidentTypeDesc} vulnerabilities.\\nPlease use the following reference # in all communications: ${IncidentId}\\n\\n${Device}\\n${ServerLocation}\\n\\nIncident times:', '${NetworkLogDate} ${TimeZone}', '\\nThank you,\\n${FromName}\\n================', '\\n${Log}\\n--------------------------------', '-' ),
		new IncidentType( 7, 'DIR', 'Directory traversal', true, 'Directory traversal probe from ${IPAddress}', 'Hi\\n\\nStop the intrusion from your IP address ${IPAddress}.\\nThe following IP address probe my network, probing for ${IncidentTypeDesc} vulnerabilities.\\nPlease use the following reference # in all communications: ${IncidentId}\\n\\n${Device}\\n${ServerLocation}\\n\\nIncident times:', '${NetworkLogDate} ${TimeZone}', '\\nThank you,\\n${FromName}\\n================', '\\n${Log}\\n--------------------------------', '-' )
	];
	//
	// At least one 'Network Log' needs to be selected
	const logBadData = [ new NetworkLog( 6,1,4,ipAddr,standardDate,'Log 1',3, 'SQL', false ) ];
	//
	let netInc = new NetworkIncident();
	//
	beforeEach(() => {
		netInc = initializeNetworkIncident( new NetworkIncident() );
	});
	//
	function initializeNetworkIncident( ni: NetworkIncident ): NetworkIncident {
		ni.incident = inc.Clone( );
		ni.deletedLogs = [];
		ni.deletedNotes = [];
		ni.incidentNotes = [];
		ni.typeEmailTemplates = incidentTypes;
		ni.user = { ... userMock };
		ni.user.Server = { ... serverMock };
		return ni;
	};
	//
	it('should create ...', ( ) => {
		console.log(
			'===================================\n' +
			'AbuseEmailReport: should create ...' );
		netInc.networkLogs = logSqlData;
		sut = new AbuseEmailReport( netInc );
		expect( sut ).toBeTruthy();
	});
	//
	it('validate (IsValid) good data ...', ( ) => {
		netInc.networkLogs = logSqlData;
		sut = new AbuseEmailReport( netInc );
		expect( sut ).toBeTruthy();
		const actual: boolean = sut.IsValid();
		if ( sut.errMsgs.length > 0 ) {
			console.error( sut.errMsgs );
		}
		expect( actual ).toBeTruthy();
		expect( sut.errMsgs.length ).toEqual( 0 );
	});
	//
	it('validate (IsValid) bad data ...', ( ) => {
		netInc.networkLogs = logBadData;
		sut = new AbuseEmailReport( netInc );
		expect( sut ).toBeTruthy();
		const actual: boolean = sut.IsValid();
		console.log( sut.errMsgs );
		expect( actual ).toBeFalsy();
		expect( sut.errMsgs.length ).toEqual( 1 );
	});
	//
	it('validate (IsValid) bad Incident Id ...', ( ) => {
		// given
		const _netIncBad = initializeNetworkIncident( new NetworkIncident );
		_netIncBad.networkLogs = logSqlData;
		_netIncBad.incident.IncidentId = 0;
		sut = new AbuseEmailReport( _netIncBad );
		expect( sut ).toBeTruthy();
		// when
		const actual: boolean = sut.IsValid();
		// then
		expect( actual ).toBeFalsy();
		expect( sut.errMsgs.length ).toEqual( 1 );
	});
	//
	it('validate (IsValid) bad Server Id ...', ( ) => {
		// given
		const _netIncBad = initializeNetworkIncident( new NetworkIncident );
		_netIncBad.networkLogs = logSqlData;
		_netIncBad.incident.ServerId = 0;
		sut = new AbuseEmailReport( _netIncBad );
		expect( sut ).toBeTruthy();
		// when
		const actual: boolean = sut.IsValid();
		// then
		expect( actual ).toBeFalsy();
		expect( sut.errMsgs.length ).toEqual( 1 );
	});
	//
	it('validate (IsValid) bad Abuse Email ...', ( ) => {
		// given
		const _netIncBad = initializeNetworkIncident( new NetworkIncident );
		_netIncBad.networkLogs = [ ... logSqlData];
		_netIncBad.incident.AbuseEmailAddress = '';
		sut = new AbuseEmailReport( _netIncBad );
		expect( sut ).toBeTruthy();
		// when
		const actual: boolean = sut.IsValid();
		// then
		console.warn( sut.errMsgs );
		expect( actual ).toBeFalsy();
		expect( sut.errMsgs.length ).toEqual( 1 );
	});
	//
	it('validate (IsValid) bad IP Address ...', ( ) => {
		// given
		const _netIncBad = initializeNetworkIncident( new NetworkIncident );
		_netIncBad.networkLogs = logSqlData;
		_netIncBad.incident.IPAddress = '';
		sut = new AbuseEmailReport( _netIncBad );
		expect( sut ).toBeTruthy();
		// when
		const actual: boolean = sut.IsValid();
		// then
		expect( actual ).toBeFalsy();
		expect( sut.errMsgs.length ).toEqual( 1 );
	});
	// from user
	it('validate (IsValid) bad user name ...', ( ) => {
		// given
		const _netIncBad = initializeNetworkIncident( new NetworkIncident );
		_netIncBad.networkLogs = logSqlData;
		_netIncBad.user.UserName = '';
		sut = new AbuseEmailReport( _netIncBad );
		expect( sut ).toBeTruthy();
		// when
		const actual: boolean = sut.IsValid();
		// then
		expect( actual ).toBeFalsy();
		expect( sut.errMsgs.length ).toEqual( 1 );
	});
	//
	it('validate (IsValid) bad user nic name ...', ( ) => {
		// given
		const _netIncBad = initializeNetworkIncident( new NetworkIncident );
		_netIncBad.networkLogs = logSqlData;
		_netIncBad.user.UserNicName = '';
		sut = new AbuseEmailReport( _netIncBad );
		expect( sut ).toBeTruthy();
		// when
		const actual: boolean = sut.IsValid();
		// then
		expect( actual ).toBeFalsy();
		expect( sut.errMsgs.length ).toEqual( 1 );
	});
	//
	it('validate (IsValid) bad user email ...', ( ) => {
		// given
		const _netIncBad = initializeNetworkIncident( new NetworkIncident );
		_netIncBad.networkLogs = logSqlData;
		_netIncBad.user.Email = '';
		sut = new AbuseEmailReport( _netIncBad );
		expect( sut ).toBeTruthy();
		// when
		const actual: boolean = sut.IsValid();
		// then
		expect( actual ).toBeFalsy();
		expect( sut.errMsgs.length ).toEqual( 1 );
	});
	// from server
	it('validate (IsValid) bad server name ...', ( ) => {
		// given
		const _netIncBad = initializeNetworkIncident( new NetworkIncident );
		_netIncBad.networkLogs = logSqlData;
		_netIncBad.user.Server.ServerName = '';
		sut = new AbuseEmailReport( _netIncBad );
		expect( sut ).toBeTruthy();
		// when
		const actual: boolean = sut.IsValid();
		// then
		expect( actual ).toBeFalsy();
		expect( sut.errMsgs.length ).toEqual( 1 );
	});
	//
	it('validate (IsValid) bad server website ...', ( ) => {
		// given
		const _netIncBad = initializeNetworkIncident( new NetworkIncident );
		_netIncBad.networkLogs = logSqlData;
		_netIncBad.user.Server.WebSite = '';
		sut = new AbuseEmailReport( _netIncBad );
		expect( sut ).toBeTruthy();
		// when
		const actual: boolean = sut.IsValid();
		// then
		expect( actual ).toBeFalsy();
		expect( sut.errMsgs.length ).toEqual( 1 );
	});
	//
	it('validate (IsValid) bad server location ...', ( ) => {
		// given
		const _netIncBad = initializeNetworkIncident( new NetworkIncident );
		_netIncBad.networkLogs = logSqlData;
		_netIncBad.user.Server.ServerLocation = '';
		sut = new AbuseEmailReport( _netIncBad );
		expect( sut ).toBeTruthy();
		// when
		const actual: boolean = sut.IsValid();
		// then
		expect( actual ).toBeFalsy();
		expect( sut.errMsgs.length ).toEqual( 1 );
	});
	//
	it('validate (IsValid) bad server from name ...', ( ) => {
		// given
		const _netIncBad = initializeNetworkIncident( new NetworkIncident );
		_netIncBad.networkLogs = logSqlData;
		_netIncBad.user.Server.FromName = '';
		sut = new AbuseEmailReport( _netIncBad );
		expect( sut ).toBeTruthy();
		// when
		const actual: boolean = sut.IsValid();
		// then
		expect( actual ).toBeFalsy();
		expect( sut.errMsgs.length ).toEqual( 1 );
	});
	//
	it('validate (IsValid) bad server from nic name ...', ( ) => {
		// given
		const _netIncBad = initializeNetworkIncident( new NetworkIncident );
		_netIncBad.networkLogs = logSqlData;
		_netIncBad.user.Server.FromNicName = '';
		sut = new AbuseEmailReport( _netIncBad );
		expect( sut ).toBeTruthy();
		// when
		const actual: boolean = sut.IsValid();
		// then
		expect( actual ).toBeFalsy();
		expect( sut.errMsgs.length ).toEqual( 1 );
	});
	//
	it('validate (IsValid) bad server from email ...', ( ) => {
		// given
		const _netIncBad = initializeNetworkIncident( new NetworkIncident );
		_netIncBad.networkLogs = logSqlData;
		_netIncBad.user.Server.FromEmailAddress = '';
		sut = new AbuseEmailReport( _netIncBad );
		expect( sut ).toBeTruthy();
		// when
		const actual: boolean = sut.IsValid();
		// then
		expect( actual ).toBeFalsy();
		expect( sut.errMsgs.length ).toEqual( 1 );
	});
	//
	it('render simple data ...', ( ) => {
		const template: string = 'This is it.';
		netInc.networkLogs = logBadData;
		sut = new AbuseEmailReport( netInc );
		const actual: string = sut.Renderer( template, {} );
		expect( actual ).toEqual( template );
	});
	//
	it('render multi-valued data ...', ( ) => {
		const template: string = '${multi}-${valued}';
		netInc.networkLogs = logBadData;
		sut = new AbuseEmailReport( netInc );
		const actual: string = sut.Renderer( template, {multi:'multi', valued: 'valued'} );
		expect( actual ).toEqual( 'multi-valued' );
	});
	//
	it('compose a 2 incident SQL injection email ...', ( ) => {
		netInc.networkLogs = logSqlData;
		sut = new AbuseEmailReport( netInc );
		expect( sut ).toBeTruthy();
		const valid: boolean = sut.IsValid();
		expect( valid ).toBeTruthy();
		const actual: string = sut.ComposeEmail( );
		expect( actual ).toContain( ipAddr );
	});
	//
	it('compose a 2 incident multi-type email ...', ( ) => {
		netInc.networkLogs = logMultiData;
		sut = new AbuseEmailReport( netInc );
		expect( sut ).toBeTruthy();
		const valid: boolean = sut.IsValid();
		expect( valid ).toBeTruthy();
		const actual: string = sut.ComposeEmail( );
		expect( actual ).toContain( ipAddr );
	});
	//
	it('ComposeEmail: should fail if template issue ...', ( ) => {
		// given
		const _netIncBad = initializeNetworkIncident( new NetworkIncident );
		_netIncBad.networkLogs = logSqlData;
		_netIncBad.typeEmailTemplates = [];
		sut = new AbuseEmailReport( _netIncBad );
		expect( sut ).toBeTruthy();
		// when
		const actual: string = sut.ComposeEmail();
		// then
		expect( actual ).toEqual( `Email Template error: not found.` );
	});
	//
	it('ComposeEmail: should fail if IsValid ...', ( ) => {
		// given
		const _netIncBad = initializeNetworkIncident( new NetworkIncident );
		_netIncBad.networkLogs = logSqlData;
		_netIncBad.incident.IPAddress = '';
		_netIncBad.incident.AbuseEmailAddress = '';
		sut = new AbuseEmailReport( _netIncBad );
		expect( sut ).toBeTruthy();
		// when
		const actual: string = sut.ComposeEmail();
		// then
		expect( actual ).toEqual( `From Incident, 'Email Address' is required.;  From Incident, 'IP Address' is required.` );
		console.log(
			'End of abuse-email-report.spec\n' +
			'=================================' );
	});
	//
});
