// ===========================================================================
// File: iauth-response.ts
// {
// 	"token": "eyJh...6HY4",
// 	"expiration": "2021-11-22T01:53:20Z"
// }
export interface IAuthResponse {
	token: string;
	expiration: string;
}
//
export class AuthResponse implements IAuthResponse {
	public token: string;
	public expiration: string;
	//
	constructor (token: string, expiration: string) {
		this.token = token;
		this.expiration = expiration;
	}
}
// ===========================================================================
