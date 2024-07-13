// ===========================================================================
// File: message.ts
import { AlertLevel } from './alert-level.enum';
//
export interface IMessage {
	//
	id: string;
	label: AlertLevel;
	message: string;
	//
}
//
export class Message implements IMessage {
	//
	public id: string;
	public label: AlertLevel;
	public message: string;
	//
	constructor(id: string, label: AlertLevel, message: string) {
		this.id = id;
		this.label = label;
		this.message = message;
	}
}
// ===========================================================================
