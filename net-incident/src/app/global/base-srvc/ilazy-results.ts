// ===========================================================================
// File: ilazy-results.ts
/**
** lazy loading return results
*/
export interface ILazyResults<T> {
	/**
	** results is an array of generic objects
	*/
	results: T[];
	/**
	** totalRecords is the total number of records
	*/
	totalRecords: number;
	/**
	** the calling lazy-load-event
	*/
	loadEvent: string;
	/**
	** message if any from the service
	*/
	message: string;
}
// ===========================================================================
