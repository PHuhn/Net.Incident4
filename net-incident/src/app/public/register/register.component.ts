// ===========================================================================
import { Component, OnInit } from '@angular/core';
import { environment } from '../../../environments/environment';
//
import { AlertsService } from '../../global/alerts/alerts.service';
import { UserService } from '../../net-incident/services/user.service';
import { IUser, User } from '../../net-incident/user';
//
@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    standalone: false
})
export class RegisterComponent {
	//
	// local variables
	//
	private codeName: string = 'Register-Component';
	registerAccount: string = '';
	registerEmail: string = '';
	registerPassword: string = '';
	registerConfirmPassword: string = '';
	registerFirstName: string = '';
	registerLastName: string = '';
	registerNicName: string = '';
	registerServerShortName: string = '';
	//
	// constructor...
	//
	constructor(
		private _alerts: AlertsService,
		private _user: UserService ) { }
	//
	registerUser() {
		this._alerts.setWhereWhatWarning( this.codeName, 'Not implemented.');
	}
	//
}
// ===========================================================================
