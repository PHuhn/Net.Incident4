// ===========================================================================
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { environment } from '../../../environments/environment';
//
import { SelectItem } from 'primeng/api';
import { Dialog } from 'primeng/dialog';
//
import { AlertsService } from '../../global/alerts/alerts.service';
import { Message } from '../../global/alerts/message';
import { AlertLevel } from '../../global/alerts/alert-level.enum';
import { UserService } from '../../net-incident/services/user.service';
import { AuthService } from '../../net-incident/services/auth.service';
import { BaseCompService } from '../../common/base-comp/base-comp.service';
import { BaseComponent } from '../../common/base-comp/base-comp.component';
import { IUser, User } from '../../net-incident/user';
import { IAuthResponse } from './iauth-response';
import { Login } from '../../net-incident/login';
import { ServerSelectionWindowComponent } from '../../net-incident/server-selection-window/server-selection-window.component';
import { ConsoleLogService } from '../../global/console-log/console-log.service';
//
@Component({
	selector: 'app-login',
	templateUrl: './login.component.html'
})
export class LoginComponent extends BaseComponent implements OnInit {
	//
	user: User;
	model: Login;
	selectItemsWindow: SelectItem[];
	displayServersWindow: boolean = false;
	// communicate to the AlertComponent
	protected _alerts: AlertsService;
	// to write console logs condition on environment log-level
	protected _console: ConsoleLogService;
	/**
	** constructor... initialize variables.
	*/
	constructor(
		// inject the base components services
		private _baseSrvc: BaseCompService,
		private _auth: AuthService,
		private _user: UserService,
	) {
		super( _baseSrvc );
		// get the needed services from the base component
		this._alerts = _baseSrvc._alerts;
		this._console = _baseSrvc._console;
		this.codeName = 'Login-Component';
		//
		this.user = User.empty( );
		this.model = new Login( '', '', '' );
		this.selectItemsWindow = [];
		//
	}
	//
	@Output() emitClose = new EventEmitter<User>();
	/**
	** Init, get defaults from environment.
	*/
	ngOnInit() {
		//
		this.model = new Login(
			environment.defaultUserAccount, '', environment.defaultUserServer
		);
		//
	}
	/**
	** Authenticate user with auth service
	*/
	loginUser(): number {
		// get the user record
		this._console.Information(
			`${this.codeName}.authUser: ${this.model.UserName}` );
		//
		const errMsgs: Message[] = this.model.validate( );
		if( errMsgs.length > 0 ) {
			this._alerts.setAlerts( AlertLevel.Error, errMsgs );
			return -1;
		}
		this._auth.authenticate( this.model.UserName, this.model.Password )
			.subscribe( ( authData: IAuthResponse ) => {
			// token_type: string;
			// expires_in: string;
			this._console.Information(
				`${this.codeName}.authUser: authenticated: ${this.model.UserName} ${authData.expiration}, token: ${authData.token.substring(0,6)}...` );
			this.getUserServer( this.model.UserName, this.model.ServerShortName );
			return 1;
			//
		},
		error => {
			this._console.Error(
				`${this.codeName}.authUser: authenticate: ${this.model.UserName} ${JSON.stringify(error)}` );
			const errMsg = (error.error.title === undefined ? error.message : error.error.title );
			this.baseErrorHandler(
				this.codeName, `User not found: ${this.model.UserName}`, errMsg );
			return -2;
		});
		return 0;
		//
	}
	/**
	** get user with user service
	*/
	getUserServer( userName: string, serverShortName: string ) {
		//
		this._user.getUserServer( userName, serverShortName )
			.subscribe( ( userData: User ) => {
				this._console.Information(
					`${this.codeName}.getUserServer: user: ${userData.UserName}` );
				this._console.Information( JSON.stringify( userData ) );
				this.user = userData;
				if( serverShortName !== ''
						&& userData.ServerShortName.toLowerCase()
							=== serverShortName.toLowerCase() ) {
					this.emitClose.emit( this.user );
				} else {
					this._console.Information(
						`${this.codeName}.getUserServer: Returned: ${userData.ServerShortName}` );
					this.selectItemsWindow = this.user.ServerShortNames;
					this.displayServersWindow = true;
				}
		},
		error => this.baseErrorHandler(
			this.codeName, `User not found: ${userName}`, error ));
		//
	}
	/**
	** on server-selection-window closed
	*/
	onServerSelected( shortName: string ) {
		this._console.Information(
			`${this.codeName}.onServerSelected: selected: ${shortName}` );
		this.displayServersWindow = false;
		this.model.ServerShortName = shortName;
		this.getUserServer( this.model.UserName, this.model.ServerShortName );
	}
	//
}
// ===========================================================================
