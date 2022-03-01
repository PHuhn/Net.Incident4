// ===========================================================================
// File: auth.service.ts
// Service for authentication of a user.
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
//
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
//
import { IAuthResponse } from '../../public/login/iauth-response';
import { environment } from '../../../environments/environment';
import { ConsoleLogService } from '../../global/console-log/console-log.service';
//
@Injectable( { providedIn: 'root' } )
export class AuthService {
	//
	private url: string;
	public codeName: string;
	/**
	** Service constructor, inject http service.
	*/
	public constructor(
		private http: HttpClient,
		private _console: ConsoleLogService ) {
		// base_Url: 'https://localhost:9114/api/',
		this.url = environment.base_Url + 'Authenticate/Login';
		this.codeName = 'Auth-Service';
	}
	/**
	** Get authenticated with UserName and Pasword
	*/
	public authenticate( userName: string, password: string ) {
		// configure call to login service
		const body = {"Username": userName, "Password": password };
		this._console.Information(
			`${this.codeName}.authenticate: ${this.url} username=${userName}` );
		const options = { headers: new HttpHeaders().set( 'Content-Type', 'application/json' ) };
		// call to login service
		return this.http.post<IAuthResponse>( this.url, body, options )
			.pipe(
				tap( ( authResponse: IAuthResponse ) => {
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
			},( err: any ) => {
				this.handleError( err );
			} ) );
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
	/**
	** General error handler
	*/
	handleError( error: any ) {
		this._console.Error( this.codeName + '.handleError: ' + error );
		if ( error instanceof HttpErrorResponse ) {
			return throwError( `Code: ${error.status}, Message: ${error.message}` || 'Service error' );
		}
		return throwError( error.toString() || 'Service error' );
	}
	//
}
// ===========================================================================
