// ===========================================================================
// File: login.ts
//
import { Message } from '../global/alerts/message';
import { AlertLevel } from '../global/alerts/alert-level.enum';
//
export interface ILogin {
	UserName: string;
	Password: string;
	ServerShortName: string,
	//
	validate( ): Message[]
}
//
export class Login implements ILogin {
	// using short-hand declaration...
	constructor(
		public UserName: string,
		public Password: string,
		public ServerShortName: string
	) { }
	//
	// Class validation rules.
	//
	validate( ): Message[] {
		const errMsgs: Message[] = [];
		//
		if( this.UserName.length === 0 || this.UserName === undefined ) {
			errMsgs.push( new Message( 'UserName-1', AlertLevel.Warning, `'User Name' is required.` ) );
		}
		if( this.UserName.length > 256 ) {
			errMsgs.push( new Message( 'UserName-2', AlertLevel.Warning, `'User Name' max length of 256.` ) );
		}
		if( this.Password.length === 0 || this.Password === undefined ) {
			errMsgs.push( new Message( 'Password-1', AlertLevel.Warning, `'Password' is required.` ) );
		}
		if( this.Password.length > 128 ) {
			errMsgs.push( new Message( 'Password-2', AlertLevel.Warning, `'Password' max length of 128.` ) );
		}
		//
		return errMsgs;
	}
}
//
// ===========================================================================
