// ===========================================================================
// File: console-log.service.ts
import { Injectable } from '@angular/core';
//
import { environment } from '../../../environments/environment';
import { LogLevel } from './log-level.enum';
//
@Injectable({ providedIn: 'root' })
export class ConsoleLogService {
	//
	private _logLevel: LogLevel;
	get logLevel(): LogLevel {
		return this._logLevel;
	}
	set logLevel(newValue: LogLevel) {
		if( newValue >= LogLevel.Error && newValue <= LogLevel.Verbose ) {
			this._logLevel = newValue;
		}
	}
	/*
	** Circular messages
	*/
	private maxMsgNbr: number = 9;
	private lastMsgNbr: number = this.maxMsgNbr;
	private msgs: string[] = Array(this.maxMsgNbr+1);
		// ['', '', '', '', '', '', '', '', '', ''];
	get lastMessage(): string {
		return this.msgs[this.lastMsgNbr];
	}
	/**
	** return in reverse order 'lastMsgNbr' first
	*/
	get messages(): string[] {
		return this.msgs.slice(0,this.lastMsgNbr+1).reverse()
			.concat(this.msgs.slice(this.lastMsgNbr+1).reverse());
	}
	//
	constructor() {
		this._logLevel = environment.logLevel;
	}
	/**
	** Write an error (LogLevel) LogMessage to console.
	** @param message 
	** @returns string of level plus message or '' if not loglevel
	*/
	public Error(message: string ): string {
		return this.LogMessage(LogLevel.Error, message);
	}
	/**
	** Write a warning (LogLevel) LogMessage to console.
	** @param message 
	** @returns string of level plus message or '' if not loglevel
	*/
	public Warning(message: string): string {
		return this.LogMessage(LogLevel.Warning, message);
	}
	/**
	** Write a success (LogLevel) LogMessage to console.
	** @param message 
	** @returns string of level plus message or '' if not loglevel
	*/
	public Success(message: string): string {
		return this.LogMessage(LogLevel.Success, message);
	}
	/**
	** Write a info (LogLevel) LogMessage to console.
	** @param message 
	** @returns string of level plus message or '' if not loglevel
	*/
	public Information(message: string): string {
		return this.LogMessage(LogLevel.Info, message);
	}
	/**
	** Write a debug (LogLevel) LogMessage to console.
	** @param message 
	** @returns string of level plus message or '' if not loglevel
	*/
	public Debug(message: string): string {
		return this.LogMessage(LogLevel.Debug, message);
	}
	/**
	** Write a verbose (LogLevel) LogMessage to console.
	** @param message 
	** @returns string of level plus message or '' if not loglevel
	*/
	public Verbose(message: string): string {
		return this.LogMessage(LogLevel.Verbose, message);
	}
	/**
	** Write a stack trace to console.
	** @param message 
	** @returns string of level plus message
	*/
	public Trace(message: string): string {
		const msg = `Trace: ${message}`;
		console.trace( msg );
		return msg;
	}
	/**
	** Write message in one place
	** @param logLevel 
	** @param message 
	** @returns 
	*/
	private LogMessage( logLevel: LogLevel, message: string): string {
		// 0=error, 1=warning, 2=success, 3=info, 4=verbose, 5=debug
		if( logLevel <= this._logLevel ) {
			const _logString = this.getEnumKeyByEnumValue(LogLevel, logLevel);
			let msg: string = `${_logString}: ${message}`;
			switch(logLevel) {
				case LogLevel.Error:
					console.error( msg );
					break;
				case LogLevel.Warning:
					console.warn( msg );
					break;
				case LogLevel.Success:
					// display in bold; green & then cancel
					console.log( `\x1b[1;32m${msg}\x1b[0m` );
					break;
				case LogLevel.Info:
					// display in bold; blue & then cancel
					console.log( `\x1b[1;34m${msg}\x1b[0m` );
					break;
				case LogLevel.Verbose:
					console.log( msg );
					break;
				case LogLevel.Debug:
					// The message is only displayed to the user if
					// the console is configured to display debug output,
					// otherwise, change to log.
					console.debug( msg );
					break;
				default:
					msg = `Unknown: ${message}`;
					console.error( msg );
					break;
			}
			this.lastMsgNbr = this.lastMsgNbr === this.maxMsgNbr ? 0 : this.lastMsgNbr + 1;
			this.msgs[this.lastMsgNbr] = msg;
			return msg;
		} else {
			return '';
		}
	}
	/*
	** Convert the enum into a string value
	*/
	public getEnumKeyByEnumValue(myEnum: any, enumValue: any): string {
		const keys = Object.keys(myEnum).filter(x => myEnum[x] === enumValue);
		return keys.length > 0 ? keys[0] : '--';
	}
	//
}
// ===========================================================================
