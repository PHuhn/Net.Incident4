// ===========================================================================
// File: global/global.ts
// See type alias ...
// https://www.typescriptlang.org/docs/handbook/2/everyday-types.html#type-aliases
export declare type ID = string | number | bigint | boolean | Date | object | null;
//
export class _GLOBAL {
    /**
	** https://stackoverflow.com/questions/62215454/how-to-get-enum-key-by-value-in-typescript
	** Convert the enum into a string value
    ** @param myEnum 
    ** @param enumValue 
    ** @returns 
    */
	public static getEnumKeyByEnumValue(myEnum: any, enumValue: number | string): string {
		const keys = Object.keys(myEnum).filter(x => myEnum[x] === enumValue);
		return keys.length > 0 ? keys[0] : '--';
	}
	//
}
// ===========================================================================
