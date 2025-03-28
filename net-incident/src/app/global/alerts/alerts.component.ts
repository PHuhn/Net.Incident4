// ===========================================================================
import { Component, OnInit } from '@angular/core';
import { trigger, state, animate, transition, style } from '@angular/animations';
//
import { AlertLevel } from './alert-level.enum';
import { Message } from './message';
import { Alerts } from './alerts';
import { AlertsService } from './alerts.service';
//
@Component({
    selector: 'app-alerts',
    animations: [
        trigger('visibilityChanged', [
            state('true', style({ opacity: 1, transform: 'scale(1.0)' })),
            state('false', style({ opacity: 0, transform: 'scale(0.0)' })),
            transition('1 => 0', animate('300ms')),
            transition('0 => 1', animate('300ms'))
        ])
    ],
    templateUrl: './alerts.component.html',
    styleUrls: ['./alerts.component.css'],
    standalone: false
})
export class AlertsComponent implements OnInit {
	/*
	** Handle a message from the data service.
	** 1) Display a color coded message area in upper right corner,
	** 2) display an unordered list of messages.
	*/
	private codeName = 'alerts.component';
	showMsgs: boolean = false;
	level: AlertLevel = AlertLevel.Success;
	msgs: Message[] = [];
	//
	constructor( private _alertService: AlertsService ) {
	}
	/*
	** On init, subscribe to the alert service.
	*/
	ngOnInit() {
		// console.log( 'AlertsComponent, OnInit ... ' );
		// Subscribe to the service
		// Will fire everytime other component use the set methods
		this._alertService.getAlerts().subscribe({
			next: ( alertMsg: Alerts ) => {
				this.showMessage( alertMsg.level, alertMsg.messages );
			},
			error: (error) => {
				this.showMessage( AlertLevel.Error, [new Message('ERR-1', AlertLevel.Error, error)] );
				console.error( `${this.codeName}.subscription: ${error}` );
			}
		} );
	}
	/*
	** display the messages
	*/
	showMessage( level: AlertLevel, msgs: Message[]): string {
		this.showMsgs = true;
		this.level = level;
		this.msgs = msgs;
		return msgs[0].message;
	}
	/*
	** close the message
	*/
	onClick(): boolean {
		this._alertService.warningInit();
		this.msgs = [];
		this.showMsgs = false;
		return false;
	}
	/*
	** return the class for the message
	*/
	getClass( msglevel: AlertLevel = +this.level ): string {
		switch( msglevel ) {
			case AlertLevel.Error:
				return 'nsg-msg-danger';
			case AlertLevel.Warning:
				return 'nsg-msg-warning';
			case AlertLevel.Success:
				return 'nsg-msg-success';
		}
		return 'nsg-msg-info';
	}
	//
}
// ===========================================================================
