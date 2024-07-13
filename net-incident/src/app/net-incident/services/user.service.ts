// ===========================================================================
// File: User.service.ts
// Service for User class
//
import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest, HttpErrorResponse } from '@angular/common/http';
//
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
//
import { Message } from '../../global/alerts/message';
import { IUser, User } from '../user';
import { environment } from '../../../environments/environment';
import { BaseSrvcService } from '../../global/base-srvc/base-srvc.service';
import { ConsoleLogService } from '../../global/console-log/console-log.service';
//
//
@Injectable( { providedIn: 'root' } )
export class UserService extends BaseSrvcService {
	//
	// Service constructor, inject http service.
	//
	constructor(
		protected _http: HttpClient,
		protected _console: ConsoleLogService ) {
			super( _http, _console );
			this.baseUrl = environment.base_Url + 'User';
			this.codeName = 'User-Service';
	}
	//
	// Single place to create a new User.
	//
	emptyUser( ): IUser {
		return User.empty( );
	}
	//
}
// ===========================================================================
