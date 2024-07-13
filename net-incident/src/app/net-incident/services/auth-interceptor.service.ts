// ===========================================================================
// dev.to/seanbh/how-to-test-a-functional-interceptor-in-angular-1bgp
import { Injectable } from '@angular/core';
import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpEvent } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
//
export const AuthInterceptorService: HttpInterceptorFn = ( 
	req: HttpRequest<unknown>, next: HttpHandlerFn ): Observable<HttpEvent<unknown>> => {
	// console.log( 'AuthInterceptorService: Authorization header access_token' );
	const idToken = localStorage.getItem( 'access_token' );
	if ( idToken ) {
		const cloned = req.clone( {
			headers: req.headers.set( 'Authorization', `Bearer ${idToken}` )
		} );
		// console.log( 'AuthInterceptorService: Authorization Bearer token' );
		//
		return next( cloned );
	} else {
		return next( req );
	}
	//
};
// ===========================================================================
