// ===========================================================================
// File: select-item-class.ts
//  define the interface(IDropDown/class(DropDown)
import { SelectItem } from 'primeng/api';
//
export class SelectItemClass implements SelectItem {
	public value: any;
	public label: string;
	public styleClass?: string;
	/**
	** Create a dropdown object using 2 parameters constructor, default selected to false.
	*/
	constructor( value: any, label: string ) {
		this.value = value;
		this.label = label;
	}
	/**
	** Create a 'to string'.
	*/
	toString( ): string {
	   return `SelectItem:[Value: ${this.value}, Text: ${this.label}, styleClass: ${this.styleClass}]`;
	}
	//
}
/**
** Create a dropdown object with and extra column.
** This was created for NoteType scripts.
*/
export class SelectItemExtra implements SelectItem {
	public value: any;
	public label: string;
	public styleClass?: string;
	public extra: string;
	/**
	** Create a dropdown object using 3 parameters constructor, default selected to false.
	*/
	constructor( value: any, label: string, extra: string ) {
		this.value = value;
		this.label = label;
		this.extra = extra;
	}
	/**
	** Create a 'to string'.
	*/
	toString( ): string {
	   return `SelectItemExtra:[Value: ${this.value}, Text: ${this.label}, styleClass: ${this.styleClass}, extra: ${this.extra}]`;
	}
	//
}
// ===========================================================================
