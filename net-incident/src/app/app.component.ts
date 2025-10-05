// ===========================================================================
// File: app.component.ts
import { Component, OnInit, AfterViewInit } from '@angular/core';
import { environment } from '../environments/environment';
//
import { PrimeNG } from 'primeng/config';
import { definePreset } from "@primeuix/themes";
import Aura from '@primeuix/themes/aura';
//
import { AlertsService } from './global/alerts/alerts.service';
import { AuthService } from './net-incident/services/auth.service';
import { ConsoleLogService } from './global/console-log/console-log.service';
import { IUser, User } from './net-incident/user';
import { Security } from './net-incident/security';
import { ServerData } from './net-incident/server-data';
import { SelectItemClass } from './global/primeng/select-item-class';
//
@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    standalone: false
})
export class AppComponent implements OnInit {
	/*
	** --------------------------------------------------------------------
	** Data declaration.
	*/
	static securityManager: Security | undefined;
	codeName: string = 'App-Component';
	//
	authenticated: boolean = false;
	userAccount: string = environment.defaultUserAccount;
	userPassword: string = '';
	user: User;
	/*
	** Constructor of the this the app.component
	*/
	constructor(
		private _alerts: AlertsService,
		private _console: ConsoleLogService,
		private _auth: AuthService,
		public _config: PrimeNG
	) {
		this.user = User.empty( );
		this.setPrimeConfig( this._config );
	}
	/**
	** Configure the following parts of this app:
	** * PrimeNG
	**
	** @param config 
	*/
	setPrimeConfig( config: PrimeNG ): void {
		/*
			0: '#ffffff',
			1: '#ffffc8',
			50: '{blue.50}',
			100: '{blue.100}',
			200: '{blue.200}',
			300: '{blue.300}',
			400: '{blue.400}',
			500: '{blue.500}',
			600: '{blue.600}',
			700: '{blue.700}',
			800: '{blue.800}',
			900: '{blue.900}',
			950: '{blue.950}',
			999: '#000038'
			//
			0: '#ffffff',
			1: '#ffc8c8',
			50: '{purple.50}',
			100: '{purple.100}',
			200: '{purple.200}',
			300: '{purple.300}',
			400: '{purple.400}',
			500: '{purple.500}',
			600: '{purple.600}',
			700: '{purple.700}',
			800: '{purple.800}',
			900: '{purple.900}',
			950: '{purple.950}',
			999: '#200540'
			//
			0: '#ffffff',
			1: '#e8e8e8',
			50: '{slate.50}',
			100: '{slate.100}',
			200: '{slate.200}',
			300: '{slate.300}',
			400: '{slate.400}',
			500: '{slate.500}',
			600: '{slate.600}',
			700: '{slate.700}',
			800: '{slate.800}',
			900: '{slate.900}',
			950: '{slate.950}',
			999: '#010415'
		*/
		const auraPreset = definePreset(Aura, {
			semantic: {
				primary: {
					0: '#ffffff',
					1: '#ffffc8',
					50: '{blue.50}',
					100: '{blue.100}',
					200: '{blue.200}',
					300: '{blue.300}',
					400: '{blue.400}',
					500: '{blue.500}',
					600: '{blue.600}',
					700: '{blue.700}',
					800: '{blue.800}',
					900: '{blue.900}',
					950: '{blue.950}',
					999: '#000038'
				},
				colorScheme: {
					light: {
						primary: {
							color: '{primary.600}',
							inverseColor: '#ffffff',
							hoverColor: '{primary.700}',
							activeColor: '{primary.800}'
						},
						highlight: {
							background: '{primary.500}',
							focusBackground: '{primary.500}',
							color: '{primary.50}',
							focusColor: '#ffffff'
						},
					},
					dark: {
						primary: {
							color: '{primary.50}',
							inverseColor: '{primary.950}',
							hoverColor: '{primary.100}',
							activeColor: '{primary.50}'
						},
						highlight: {
							background: 'rgba(250, 250, 250, .16)',
							focusBackground: 'rgba(250, 250, 250, .24)',
							color: 'rgba(255,255,255,.87)',
							focusColor: 'rgba(255,255,255,.87)'
						},
					}
				},
			},
			components: {
				card: {
					colorScheme: {
						light: {
							root: {
								background: '{primary.100}',
								borderRadius: '5px',
								color: '{primary.950}',
							},
						},
						dark: {
							root: {
								background: '{primary.900}',
								borderRadius: '5px',
								color: '{primary.50}',
							}
						}
					}
				},
				menubar: {
					colorScheme: {
						light: {
							root: {
								background: '{primary.500}',
								color: '{primary.950}',
								borderColor: '{primary.500}',
							}
						},
						dark: {
							root: {
								background: '{primary.800}',
								color: '{primary.50}',
								borderColor: '{primary.800}',
							},
							submenu: {
								background: "{primary.950}",
							}
						}
					}
				},
				datatable: {
					colorScheme: {
						light: {
							row: {
								background: '{primary.50}',
							},
							header: {
								background: '{primary.200}',
							},
							headerCell: {
								background: '{primary.100}',
							},
							footer: {
								background: '{primary.100}',
							},
							// paginatorTop: {
							// 	background: '{primary.100}',
							// },
							// paginatorBottom: {
							// 	background: '{primary.100}',
							// },
						},
						dark: {
							row: {
								background: '{primary.950}',
							},
							header: {
								background: '{primary.950}',
							},
							headerCell: {
								background: '{primary.900}',
							},
							footer: {
								background: '{primary.900}',
							},
							// paginatorBottom: {
							// 	background: '{primary.900}',
							// },
						}
					}
				},
				button: {
					colorScheme: {
						light: {
							root: {
								secondary: {
									borderColor: '#000000',
									hoverBorderColor: '{primary.950}',
								},
							},
						},
						dark: {
							root: {
								primary: {
									background: '{primary.600}',
									hoverBackground: '{primary.700}',
									activeBackground: '{primary.700}',
									borderColor: '{primary.600}',
									hoverBorderColor: '{primary.700}',
									activeBorderColor: '{primary.700}',
									color: '{primary.50}',
									hoverColor: '{primary.100}',
									activeColor: '{primary.100}',
									focusRing: {
										color: '{primary.600}',
										shadow: 'none'
									}
								},
								secondary: {
									borderColor: '{primary.400}',
									hoverBorderColor: '{primary.50}',
								},
							},
						},
					},
				},
				dialog: {
					colorScheme: {
						light: {
							root: {
								background: '{primary.50}',
								color: '{primary.950}',
							},
						},
						dark: {
							root: {
								background: '{primary.900}',
								color: '{primary.50}',
							},
						}
					}
				}
			}
		} );
		config.theme.set({
			preset: auraPreset,
			options: {
				darkModeSelector: '.p-dark',
			},
		});
		//
	}
	/*
	** On component initialization, get all data from the data service.
	*/
	ngOnInit() {
		this._console.Information(
			`${this.codeName}.${this.ngOnInit.name}: ...`);
	}
	/*
	** (onClose)='onAuthenticated($event)
	** of the login.component
	*/
	onAuthenticated( user: User ): void {
		this.user = user;
		const security: Security = new Security( this.user );
		if( security.isValidIncidentGrid( ) ) {
			AppComponent.securityManager = security;
			this.authenticated = true;
		} else {
			this._alerts.setWhereWhatWarning( this.codeName, 'Not authorized' );
		}
	}
	/*
	** (logout)='onAuthLogout($event)
	** onClick of Logout button in the header.component
	*/
	onAuthLogout(event: any): void {
		this._console.Information(
			`${this.codeName}.${this.onAuthLogout.name}: Logout clicked.`);
		this._auth.logout( );
		AppComponent.securityManager = undefined;
		this.authenticated = false;
	}
	//
	fakeLogin() {
		const server = new ServerData(
			1, 1, 'Northern Software Group', 'NSG Memb', 'Members Web-site',
			'Public facing members Web-site', 'Web-site address: www.mimilk.com',
			'We are in Michigan, USA.', 'Phil Huhn', 'Phil', 'PhilHuhn@yahoo.com',
			'EST (UTC-5)', true, 'EDT (UTC-4)',
			new Date('2018-03-11T02:00:00'), new Date('2018-11-04T02:00:00')
		);
		this.user = new User(
			'e0fcb3e8-252a-4311-b782-7e094f0737aa', 'Phil', 'Phillip', 'Huhn',
			'Phil Huhn', 'Phil', 'PhilHuhn@yahoo.com', true, '734-545-5845', true,
			1, [ new SelectItemClass( 'NSG Memb', 'Members Web-site' ),
				new SelectItemClass( 'NSG Router', 'NSG Router' ) ],
			'nsg router', server, ['admin']
		);
		this.authenticated = true;
	}
	//
}
// ===========================================================================
