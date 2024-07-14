// ===========================================================================
// File: filter-summary.component.ts
import { Component, Input } from '@angular/core';
//
import { LazyLoadMeta } from 'primeng/api';
//
// for formatFilters (display of current primeng table filters)
export type AssocArray = Record<string, string>;
//
@Component({
	selector: 'app-filter-summary',
	template: `	<div *ngFor='let filter of filters' class='nsg-summary-line'>
		&nbsp;{{ filter }}
	</div>`,
	styleUrl: './filter-summary.component.css'
})
export class FilterSummaryComponent {
	private _fieldDescriptions: AssocArray = {};
	filters: string[] = [];
	/*
	** --------------------------------------------------------------------
	** Inputs:
	** * fieldDescriptions
	** * lazyLoadEvent
	*/
	/**
	** translattion of field names to displayed descriptions
	*/
	@Input() set fieldDescriptions( descr: AssocArray | undefined ) {
		this._fieldDescriptions = descr !== undefined ? descr : {};
	}
	/**
	** invoke the filter summary upon change of lazyLoadEvent
	*/
	@Input() set lazyLoadEvent( event: LazyLoadMeta | undefined ) {
		this.filters = event !== undefined ?
			this.formatFilters( event, this._fieldDescriptions ) : [];
	}
	/**
	** Do an attempt on making readable descriptions from the variable name.
	** @param s 
	** @returns 
	*/
	displayTitle( s: string ) {
		return s.replace(/(^|[_-])([a-z])/g, (a, b, c) => c.toUpperCase())
			.replace(/([a-z])([A-Z])/g, (a, b, c) => `${b} ${c}`
		);
	}
	/**
	** Place filter summary in the footer as	follows:
	** | Description (contains) Fake
	** | Short Description (contains) Short
	** @param event 
	** @param translation 
	** @returns array of string
	*/
	formatFilters( event: LazyLoadMeta, translation: AssocArray ): string[] {
		const filterStrings: string[] = [];
		for ( const prop in event.filters ) {
			if( prop !== '' ) {
				const filterField: string = prop;
				const filterMeta = event.filters[filterField];
				if ( Array.isArray( filterMeta ) ) {
					for ( const meta of filterMeta ) {
						if( meta.value !== undefined && meta.value !== null ) {
							let field: string = translation[ filterField ];
							if( field === undefined ) {
								field = this.displayTitle( filterField );
							}
							filterStrings.push( `${field} (${meta.matchMode}) ${meta.value}` );
						}
					}
				}
			}
		}
		return filterStrings;
	}
}
// ===========================================================================
