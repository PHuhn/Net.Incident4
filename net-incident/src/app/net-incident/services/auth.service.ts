// ===========================================================================
// File: auth.service.ts
// Service for authentication of a user.
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
//
import { throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
//
import { IAuthResponse } from '../../public/login/iauth-response';
import { environment } from '../../../environments/environment';
import { BaseSrvcService } from '../../global/base-srvc/base-srvc.service';
import { ConsoleLogService } from '../../global/console-log/console-log.service';
//
@Injectable( { providedIn: 'root' } )
export class AuthService extends BaseSrvcService {
	/**
	** Service constructor, inject http service.
	*/
	public constructor(
		protected _http: HttpClient,
		protected _console: ConsoleLogService ) {
			super( _http, _console );
			this.baseUrl = environment.base_Url + 'Authenticate/Login';
			this.codeName = 'Auth-Service';
	}
	/**
	** Get authenticated with UserName and Pasword
	*/
	public authenticate( userName: string, password: string ) {
		// configure call to login service
		const body = {"Username": userName, "Password": password };
		this._console.Information(
			`${this.codeName}.authenticate: ${this.baseUrl} username=${userName}` );
		return this.postJsonBody<IAuthResponse>( body )
			.pipe( tap( ( authResponse: IAuthResponse ) => {
				const len = authResponse.token.length;
				if( len > 24
						&& authResponse.expiration.length > 0 ) {
					this._console.Information( `${this.codeName}.authenticate: authenticated:  ${userName}, token: ${authResponse.token.substring(0,4)}...${authResponse.token.substring(len-4)}` );
					this.setLocalStorage(authResponse.token, authResponse.expiration, userName );
				} else {
					this._console.Information( JSON.stringify( authResponse ) );
					throw new Error( `${this.codeName}.authenticate: Invalid token returned.` );
				}
				return authResponse;
			}
		) );
		//
	}
	/**
	** Set all 'access_' values to local storage
	*/
	public setLocalStorage( token: string, expires: string, userName: string ): void {
		// expiresAt is # of milliseconds
		const expiresAt: number = Date.parse( expires );
		localStorage.setItem( 'access_expires', JSON.stringify( expiresAt.valueOf() ) );
		localStorage.setItem( 'access_token', token );
		localStorage.setItem( 'access_userName', userName );
	}
	/**
	** Clear all values in local storage
	*/
	public logout( ): void {
		localStorage.removeItem( 'access_token' );
		localStorage.removeItem( 'access_expires' );
		localStorage.removeItem( 'access_userName' );
	}
	/**
	** is logged in or out
	*/
	public isLoggedIn( ): boolean {
		return ( Date.now() < this.getExpiration( ) );
	}
	//
	public isLoggedOut( ): boolean {
		return !this.isLoggedIn();
	}
	/**
	** get expiration from local storage
	*/
	public getExpiration( ): number {
		const expiration = localStorage.getItem( 'access_expires' );
		if ( expiration === null ) {
			return 0;
		}
		const expiresAt: number = JSON.parse(expiration);
		return expiresAt;
	}
	//
}
// ===========================================================================
