// ===========================================================================
// file: LazyLoading.mock
import { LazyLoadEvent, FilterMetadata } from 'primeng/api';
//
import { ILazyResults } from '../../../global/base-srvc/ibase-srvc';
//
export class LazyLoadingMock {
	//
	public codeName: string = 'lazy-loading.mock';
	/**
	** apply filters, sort/ordered by, skip/take
	** @param datasource 
	** @param event 
	** @returns ILazyResults
	*/
	LazyLoading<T>( datasource: T[], event: LazyLoadEvent ): ILazyResults {
		//
		const results: ILazyResults = { results: datasource, totalRecords: datasource ? datasource.length : 0, loadEvent: JSON.stringify( event ), message: ''};
		if ( datasource && datasource.length ) {
			let filtered: T[] = datasource.slice( 0 );
			if( event.filters ) {
				filtered = this.LazyFilters<T>( filtered, event );
			}
			results.totalRecords = filtered.length;
			// sort
			if( event.sortField !== undefined && event.sortOrder !== undefined ) {
				filtered = this.LazyOrderBy<T>( filtered, event );
			}
			// skip & take (needs to be last)
			if( event.first !== undefined && event.rows !== undefined ) {
				filtered = this.LazySkipTake<T>( filtered, event );
			}
			results.results = filtered;
			return results;
		}
		results.results = [];
		results.message = 'no source data';
		console.error( `${this.codeName}.lazyLoading: ${results.message}.` );
		return results;
	}
	/**
	** Filters (where)
	** filtered: an array of data to be filtered
	** event: the lazy-load-event passed by primeng to the onLazyLoad event.
	**   Uses only the filters? data in lazy-load-event
	** @param filtered 
	** @param event 
	** @returns 
	*/
	LazyFilters<T>( filtered: T[], event: LazyLoadEvent ): T[] {
		if( event.filters ) {
			for (const key in event.filters) {
				if (event.filters.hasOwnProperty( key )) {
					const filterMeta: FilterMetadata = event.filters[key];
					if (Array.isArray(filterMeta)) {
						let tempCat: any[] = [];
						let opOr: boolean = false;
						for (const filter of filterMeta.filter(f=> f.value !== null)) {
							const temp: T[] = this._filter( filtered, key, filter );
							if( filter.operator === 'and' ) {
								filtered = temp;
							} else {
								opOr = true;
								tempCat = tempCat.concat( temp );
							}
						}
						if( opOr ) {
							filtered = tempCat;
						}
					} else {
						filtered = this._filter( filtered, key, filterMeta );
					}
				}
			}
		}
		return filtered;
	}
	//
	_filter( filtered: any[], key: string, filter: FilterMetadata ): any[] {
		const matchMode = filter.matchMode !== undefined ? filter.matchMode : '';
		const value: any = filter.value !== null ? filter.value : '';
		if( matchMode !== '' ) {
			switch( matchMode.toLowerCase() ) {
				case 'equals': {
					filtered = filtered.filter( el => el[key] === value );
					break;
				}
				case 'notequals': {
					filtered = filtered.filter( el => el[key] !== value );
					break;
				}
				case 'gt': {
					filtered = filtered.filter( el => el[key] > value );
					break;
				}
				case 'lt': {
					filtered = filtered.filter( el => el[key] < value );
					break;
				}
				case 'startswith': {
					const _len = value.length;
					filtered = filtered.filter( el => el[key].substring(0, _len) === value );
					break;
				}
				case 'endswith': {
					const _len = value.length;
					filtered = filtered.filter( el => el[key].endsWith( value ) );
					break;
				}
				case 'in': {
					filtered = filtered.filter( el => value.includes( el[key] ) );
					break;
				}
				case 'contains': {
					filtered = filtered.filter( el => el[key].includes( value ) );
					break;
				}
				case 'notcontains': {
					filtered = filtered.filter( el => !el[key].includes( value ) );
					break;
				}
				default: {
					console.log(`matchMode not found: ${filter.matchMode}`);
					break;
				}
			}
		}
		return filtered;
	}
	/**
	** Order-by (sort)
	** event.sortField = Field name to sort with
	** event.sortOrder = Sort order as number, 1 for asc and -1 for dec
	** @param data 
	** @param event 
	** @returns 
	*/
	LazyOrderBy<T>( data: T[], event: LazyLoadEvent ): T[] {
		if( data.length > 0 ) {
			if( event.sortField !== undefined ) {
				const key = event.sortField;
				if( event.sortOrder !== undefined ) {
					const sortOrder: number = event.sortOrder !== undefined ? event.sortOrder : 1;
					return data.sort( ( n1: any, n2: any ) => {
						if( n1[key] > n2[key] ) {
							return ( sortOrder === 1 ? 1: -1 );
						}
						return ( sortOrder === 1 ? -1: 1 );
					});
				}
			}
		}
		return data;
	}
	/**
	** skip-take (page of data)
	** @param data 
	** @param event 
	** @returns 
	*/
	LazySkipTake<T>( data: T[], event: LazyLoadEvent ): T[] {
		if( event.first !== undefined && event.rows !== undefined ) {
			return data.slice( event.first, ( event.first + event.rows ) );
		}
		return data;
	}
	//
}
// ===========================================================================
