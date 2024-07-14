// ===========================================================================
// File: select-item-class.ts
//  define the interface(IDropDown/class(DropDown)
//
import { SelectItem } from 'primeng/api';
import { BaseClass } from '../base-class';
import { ID } from '../global';
//
export class SelectItemClass extends BaseClass implements SelectItem {
	public value: ID;
	public label: string;
	public styleClass?: string;
	/**
	** Create a dropdown object using 2 parameters constructor, default selected to false.
	** @param value 
	** @param label 
	*/
	constructor( value: ID, label: string ) {
		super( );
		this.value = value;
		this.label = label;
	}
	// BaseClass contains Clone( )
	// BaseClass contains toString( )
	//
}
/**
** Create a dropdown object with and extra column.
** This was created for NoteType scripts.
*/
export class SelectItemExtra extends BaseClass implements SelectItem {
	public value: ID;
	public label: string;
	public styleClass?: string;
	public extra: string;
	/**
	** Create a dropdown object using 3 parameters constructor, default selected to false.
	*/
	constructor( value: ID, label: string, extra: string ) {
		super( );
		this.value = value;
		this.label = label;
		this.extra = extra;
	}
	// BaseClass contains Clone( )
	// BaseClass contains toString( )
	//
}
// ===========================================================================
