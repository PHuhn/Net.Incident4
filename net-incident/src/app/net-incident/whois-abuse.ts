// ===========================================================================
import { environment } from '../../environments/environment';
//
export interface IWhoIsAbuse {
	nic: string;
	net: string;
	abuse: string;
	inet: string;
	//
	GetWhoIsAbuse( data: string ): void;
	//
	ParseWhoIsData( data: string ): any[];
	//
	GetNIC( raw: string ): string;
	//
	BadAbuseEmail( ): boolean;
	//
	ValidEmailAddress(ea: string): boolean;
}
//
// using whois data try to find the
// * abuse e-mail address
// * NIC (Network Information Center)
// * Network: 
// * Abuse Email:
//
export class WhoIsAbuse implements IWhoIsAbuse {
	public nic: string;
	public net: string;
	public abuse: string;
	public inet: string;
	private logLevel: number = environment.logLevel;
	private badAbuse: string[]; // environment.BadAbuseEmailAddresses;
	//
	private emailRE = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
	//
	netSolutions = environment.NetSolutionsNIC;
	//
	constructor() {
		this.nic = ''; this.net = ''; this.abuse = ''; this.inet = '';
		this.badAbuse = environment.BadAbuseEmailAddresses;
	}
	/**
	** Sets the following: nic, net, abuse, inet
	** @param data raw whois data
	*/
	GetWhoIsAbuse( data: string ): void {
		const parsed = this.ParseWhoIsData( data );
		const nic = this.GetNIC( data );
		if( nic !== '' ) {
			// now get net and abuse e-mail
			if( this.netSolutions.includes( nic ) ) {
				this.ProcessNetSolutions( nic, data );
			} else {
				if ( nic === 'twnic.net') {
					this.ProcessTw( nic, data );
				} else {
					this.ProcessParsed( nic, parsed );
				}
			}
		}
	}
	//
	// https://www.npmjs.com/package/parse-whois
	// parse to { "attribute": <attr ~key>, "value": <value> }
	ParseWhoIsData( data: string ): any[] {
		let attr: string;
		let colonPos: number;
		let endTextStr: string = '';
		const returnArray: any[] = [];
		//
		data.split( '\n' ).forEach( ( part ) => {
			if( !part ) { return; }
			colonPos = part.indexOf( ':' );
			attr = part.substring( 0, colonPos );
			if( attr !== '' ) {
				returnArray.push( {
					'attribute': attr,
					'value': part.substr( colonPos + 1 ).trim( ).replace(/(\r\n|\n|\r)/gm,'')
				} );
			} else {
				endTextStr += part.substr(colonPos+1).trim() + '\n';
			}
		});
		returnArray.push( { 'attribute': 'End Text', 'value': endTextStr } );
		//
		return returnArray;
	}
	/**
	** Parse the first 8 lines starting with [ to find the proper NIC.
	** side effect this.nic is set
	** @param raw 
	** @returns string of the NIC
	*/
	GetNIC( raw: string ): string {
		let nic = '';
		if( raw !== '' ) {
			const lines = raw.split( '\n' );
			for (let i = 0; i < 8; i++) {
				// allow for redirection, [Redirected to whois.ripe.net]
				const line = lines[i];
				if( line.substring( 0, 1 ) === '[' ) {
					let pos = line.indexOf( 'whois.' );
					if( pos === -1 ) {
						// [vault.krypt.com] length is the same!
						pos = line.indexOf( 'vault.' );
					}
					if( pos > -1 ) {
						const parts = line.substring( pos ).split( '.' );
						nic = line.substring( pos+6, line.indexOf( ']' ) );
						if( parts[1] === 'registro' ) {
							nic = nic.replace(/registro/,'nic');
						}
					}
				} else {
					i = 8; // teminate loop
				}
			}
		}
		this.nic = nic;
		return nic;
	}
	/**
	** process the raw data from whois
	** @param nic the NIC returned from GetNIC
	** @param raw whois data
	** @returns this class { nic, net, abuse, inet }
	*/
	ProcessNetSolutions( nic: string, raw: string ) {
		let net = '', abuse = '', inet = '';
		const lines = raw.split( '\n' );
		const netAtt = [ 'customer organization', 'org-name' ];
		const inetAtt = [ 'ip-network' ];
		const abuseAtt = [ 'abuse-email', 'abuse-contact;i' ];
		raw.split( '\n' ).forEach( ( line ) => {
			const flds = line.split( ':' );
			if( this.netSolutions.includes( nic ) ) {
				if( flds.length > 2 ) {
					const attrib: string = flds[1].toLowerCase( );
					if( netAtt.includes( attrib ) ) { net = flds[2].replace(/(\r\n|\n|\r)/gm,''); }
					if( abuseAtt.includes( attrib ) ) {
						const ea = flds[2].replace(/(\r\n|\n|\r)/gm,'');
						if( this.ValidEmailAddress(ea ))
							abuse = ea;
						}
					if( inetAtt.includes( attrib ) ) { inet = flds[2].replace(/(\r\n|\n|\r)/gm,''); }
				}
			}
		});
		this.nic = nic; this.net = net; this.inet = inet;
		if( this.ValidEmailAddress( abuse ) ) {
			this.abuse = abuse;
		}
		return { 'nic': nic, 'net': net, 'abuse': abuse, 'inet': inet };
	}
	/**
	** process the raw data from whois
	** @param nic the NIC returned from GetNIC
	** @param raw whois data
	** @returns this class { nic, net, abuse, inet }
	*/
	ProcessTw( nic: string, raw: string ) {
		let net = '', abuse = '', inet = '';
		const lines: string[] = raw.split( '\n' );
		let prvs: string[] = [];
		raw.split( '\n' ).forEach( ( line ) => {
			const flds: string[] = line.trim().split( ':' );
			if( flds.length > 1 ) {
				const attrib: string = flds[0].toLowerCase( );
				if( attrib === 'netname' ) { net = flds[1].trim().replace(/(\r\n|\n|\r)/gm,''); }
				if( attrib === 'netblock' ) { inet = flds[1].trim().replace(/(\r\n|\n|\r)/gm,''); }
				if( attrib === 'abuse-c' ) {
					const ea = flds[1].trim().replace(/(\r\n|\n|\r)/gm,'');
					if( this.ValidEmailAddress(ea )) abuse = ea;
				}
			} else {
				if( prvs.length > 1 ) {
					// Technical contact:
					// network-adm@hinet.net
					const attrib: string = prvs[0].toLowerCase( );
					if( attrib === 'technical contact' ) {
						const ea = flds[0].trim().replace(/(\r\n|\n|\r)/gm,'');
						if( this.ValidEmailAddress(ea )) abuse = ea;
					}
				}
			}
			prvs = flds;
		});
		this.nic = nic; this.net = net; this.inet = inet;
		if( this.ValidEmailAddress( abuse ) ) {
			this.abuse = abuse;
		}
		return { 'nic': nic, 'net': net, 'abuse': abuse, 'inet': inet };
	}
	/**
	** process the parsed data from whois
	** @param nic the NIC returned from GetNIC
	** @param parsed 
	** @returns this class { nic, net, abuse, inet }
	*/
	ProcessParsed( nic: string, parsed: any[] ) {
		let net = '', abuse = '', inet = '';
		// default values to search for...
		let netAtt = [ 'customer', 'custname', 'netname' ];
		let inetAtt = [ 'inetnum', 'netrange' ];
		let abuseAtt = [ 'abuse-mailbox', 'orgabuseemail', 'abuse-c' ];
		if( nic === 'lacnic.net' ) {
			netAtt = [ 'owner' ];
			// keep default inet
			abuseAtt = [ 'e-mail' ];
		} else if( nic === 'nic.br' ) {
			netAtt = [ 'owner' ];
			// keep default inet
			abuse = 'cert@cert.br'; // hard assignment
		} else if ( nic === 'krnic.net' ) {
			netAtt = [ 'organization name' ];
			inetAtt = [ 'ipv4 address' ];
			abuseAtt = [ 'e-mail' ];
		}
		if( parsed.length > 0 ) {
			if( abuse === '' ) {
				// parse through the comments to fine the abuse e-mail address
				const endTextStr: string = parsed[ parsed.length - 1 ].value;
				endTextStr.split( '\n' ).forEach( ( line ) => {
					if( !line ) { return; }
					const flds = line.split( ' ' );
					if( flds.length > 2 ) {
						// % Abuse contact for '145.255.0.0 - 145.255.15.255' is 'abuse@ufanet.ru'
						if( flds[0] === '%' && flds[1].toLowerCase( ) === 'abuse' ) {
							if( this.logLevel >= 4 ) {
								console.log( line );
							}
							const abuseLine = line.split( `'` );
							const ea = abuseLine[ abuseLine.length - 2 ];
							if( this.ValidEmailAddress( ea )) abuse = ea;
						}
					}
				});
			}
			for (const obj of parsed) {
				const attrib: string = obj.attribute.trim().toLowerCase();
				if( net === '' ) {
					if( netAtt.includes( attrib ) ) {
						net = obj.value;
					}
				}
				if( inet === '' ) {
					if( inetAtt.includes( attrib ) ) {
						inet = obj.value;
					}
				}
				if( abuse === '' ) {
					if( abuseAtt.includes( attrib ) ) {
						const ea = obj.value;
						if( this.ValidEmailAddress( ea )) abuse = ea;
					}
				}
			}
		}
		this.nic = nic; this.net = net; this.inet = inet;
		if( this.ValidEmailAddress( abuse ) ) {
			this.abuse = abuse;
		}
		return { 'nic': nic, 'net': net, 'abuse': abuse, 'inet': inet };
	}
	/**
	** Is this a bad abuse e-mail address.
	** @returns true if bad or false if good
	*/
	BadAbuseEmail(): boolean {
		return !this.ValidEmailAddress( this.abuse );
	}
	/**
	** Is this a good e-mail address.  Excluding NIC email addresses.
	** @param ea email address
	** @returns true if good, false if bad
	*/
	ValidEmailAddress( ea: string ): boolean {
		if( ea === '' ) {
			return false;
		}
		if( this.badAbuse.includes( ea ) ) {
			return false;
		}
		return this.ValidateEmail( ea );
	}
	/**
	** Is this a valid email address.
	** @param email 
	** @returns true if valid or false if invalid
	*/
	ValidateEmail( email: string ): boolean {
		return this.emailRE.test(String(email).toLowerCase());
	}
	//
}
// ===========================================================================
