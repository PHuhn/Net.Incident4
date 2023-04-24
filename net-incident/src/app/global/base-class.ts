// ===========================================================================
// File: base-srvc.ts
export interface IBaseClass {
	/**
	** Clone the current ProdCategory class.
	** @returns copy of the class
	*/
	Clone( ): this;
	/**
	** toString implementation for base class
	** @returns JSON string representaion of class
	*/
	toString( ): string;
	//
}
//
export class BaseClass implements IBaseClass {
	/**
	** Clone the current class.
	** @returns copy of the class
	*/
	public Clone( ): this {
		return { ... this };
	}
	/**
	** toString implementation for base class
	** @returns JSON string representaion of class
	*/
	public toString = (): string => {
		return JSON.stringify( this );
	}
	//
}
// ===========================================================================
