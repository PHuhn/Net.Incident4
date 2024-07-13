// ===========================================================================
// file: alerts.service.ts
import { Injectable } from '@angular/core';
//
import { Observable, Subject } from 'rxjs';
//
import { AlertLevel } from './alert-level.enum';
import { Message } from './message';
import { Alerts } from './alerts';
//
@Injectable()
export class AlertsService {
	/*
	** Send messages to the alert component that displays a message
	** in the upper right-hand corner of the screen.
	** Three different ways to use this:
	** 1) setWhereWhatSuccess/setWhereWhatInfo/setWhereWhatWarning/setWhereWhatError
	** 2) warningSet
	** 3) warningInit/warningAdd/warningPost/warningCount
	** See the unit test for examples of usage.
	*/
	private alerted: Message[] = [];
	private level: AlertLevel = AlertLevel.Error;
	private subject$: Subject< Alerts > = new Subject< Alerts >();
	/*
	** Circular messages
	*/
	private maxAltNbr: number = 9;
	private lastAltNbr: number = this.maxAltNbr;
	private alts: Message[] = Array<Message>(10);
	/**
	** Circular message queue property
	** @returns in last message item
	**		(not what was displayed)
	*/
	get lastAlertMessage(): Message {
		return this.alts[this.lastAltNbr];
	}
	/**
	** Circular message queue property,
	** returns all messages in the queue.
	** @returns in reverse order 'lastAltNbr' first
	*/
	get alertMessages(): Message[] {
		return this.alts.slice(0,this.lastAltNbr+1).reverse()
			.concat(this.alts.slice(this.lastAltNbr+1).reverse());
	}
	/*
	** See setAlerts for adding the messages to the queue.
	** End of circular messages ...
	*/
	/**
	** Subscriber gets this to get the logged values.
	** @returns last instance of Alerts objects
	*/
	public getAlerts(): Observable< Alerts > {
		return this.subject$.asObservable();
	}
	/**
	** Add a next Alerts subject
	** @param errs 
	** @param alerted 
	*/
	public setAlerts( errs: AlertLevel, alerted: Message[] ): void {
		alerted.forEach(_alt => {
			this.lastAltNbr = this.lastAltNbr === this.maxAltNbr ? 0 : this.lastAltNbr + 1;
			this.alts[this.lastAltNbr] = _alt;
		});
		// console.log( 'setAlerts: ' + errs.toString() );
		this.subject$.next( new Alerts( errs, alerted ) );
	}
	// Turn the two parameters into an array.
	private whereWhatMessages( msgs: Message[], label: AlertLevel, where: string, what: string) {
		const count: number = msgs.length;
		where = ( where === '' ? '-' : where );
		what = ( what === '' ? '-' : what );
		msgs.push( new Message( `${count + 1}-WHERE`, label, where ) );
		msgs.push( new Message( `${count + 2}-WHAT`, label, what ) );
		this.alerted = [];
	}
	/**
	** Turn the two parameters into an array and display the alert.
	** @param where where the message originated
	** @param what an information message
	*/
	public setWhereWhatInfo( where: string, what: string ): void {
		const msgs: Message[] = [];
		this.whereWhatMessages( msgs, AlertLevel.Info, where, what );
		this.setAlerts( AlertLevel.Info, msgs );
	}
	/**
	 * Turn the two parameters into an array and display the alert.
	** @param where where the message originated
	** @param what a success message
	 */
	public setWhereWhatSuccess( where: string, what: string ): void {
		const msgs: Message[] = [];
		this.whereWhatMessages( msgs, AlertLevel.Success, where, what );
		this.setAlerts( AlertLevel.Success, msgs );
	}
	/**
	** Turn the two parameters into an array and display the alert.
	** @param where where the message originated
	** @param what a warning message
	*/
	public setWhereWhatWarning( where: string, what: string ): void {
		const msgs: Message[] = [];
		this.whereWhatMessages( msgs, AlertLevel.Warning, where, what );
		this.setAlerts( AlertLevel.Warning, msgs );
	}
	/**
	** Turn the three parameters into a collection and
	** display the alert.
	** example:
	**  error: (error) => {
	**   this.baseErrorHandler(
	**    this.codeName, `User not found: ${userName}`, error );
	**  },
	** @param where where the message originated
	** @param what what is happening message
	** @param err an error message
	*/
	public setWhereWhatError( where: string, what: string, err: string ): void {
		const msgs: Message[] = [];
		this.whereWhatMessages( msgs, AlertLevel.Error, where, what );
		const count: number = msgs.length;
		err = ( err === '' ? '-' : err );
		msgs.push( new Message( `${count + 1}-ERR`, AlertLevel.Error, err ) );
		this.setAlerts( AlertLevel.Error, msgs );
	}
	/**
	** Warning validation error messages
	** @param msgs array 
	** @returns true if success or false
	*/
	public warningSet( msgs: Message[] ): boolean {
		if( msgs.length > 0 ) {
			this.setAlerts( AlertLevel.Warning, msgs );
			return true;
		} else {
			console.log( 'Messages array empty.' );
		}
		return false;
	}
	/**
	** Initialize a validation warning
	*/
	public warningInit(): void {
		this.level = AlertLevel.Warning;
		this.alerted = [];
	}
	/**
	** Add a validation warning
	** @param warning 
	** @returns 
	*/
	public warningAdd( warning: string ): boolean {
		const id: string =
			( this.alerted.length + 1 ).toString();
		if( warning !== '' ) {
			this.alerted.push( new Message( id, AlertLevel.Warning, warning ));
			return true;
		} else {
			this.alerted.push( new Message( id, AlertLevel.Warning, '-' ));
		}
		return false;
	}
	/**
	** Post a validation warning
	** @returns true if add alerts subject or false
	*/
	public warningPost( ): boolean {
		if( this.alerted.length > 0 ) {
			this.subject$.next( new Alerts( this.level, this.alerted ) );
			return true;
		} else {
			console.log( 'Alerted message array empty.' );
		}
		return false;
	}
	/**
	** Return count of messages in Init/Add/Post alert process.
	** @returns number of Message elements in alerted array
	*/
	public warningCount(): number {
		return this.alerted.length;
	}
	/**
	** Set an error message.
	** @param msg the alert message
	** @returns the message id
	*/
	public errorAlert(msg: string): string {
		return this.addAlert( AlertLevel.Error, msg );
	}
	/**
	** Set an warning message.
	** @param msg the alert message
	** @returns the message id
	*/
	public warnAlert(msg: string): string {
		return this.addAlert( AlertLevel.Warning, msg );
	}
	/**
	** Set an success message.
	** @param msg the alert message
	** @returns the message id
	*/
	public successAlert(msg: string): string {
		return this.addAlert( AlertLevel.Success, msg );
	}
	/**
	** Set an information message.
	** @param msg the alert message
	** @returns the message id
	*/
	public infoAlert(msg: string): string {
		return this.addAlert( AlertLevel.Info, msg );
	}
	/**
	** set any level of alert message
	** @param level AlertLevel enum
	** @param msg the alert message
	** @returns the message id
	*/
	public addAlert(level: AlertLevel, msg: string): string {
		const id: string = (this.alerted.length + 1).toString().padStart(3, "0");
		const alertMsg: Message = new Message(id, level, msg );
		return this.addAlertMessage( alertMsg );
	}
	/**
	** set a message object to the alert service
	** @param message an alert message
	** @returns the message id
	 */
	public addAlertMessage(message: Message): string {
		this.alerted.push( message );
		this.setAlerts( message.label, this.alerted );
		return message.id;
	}
	//
}
// ===========================================================================
