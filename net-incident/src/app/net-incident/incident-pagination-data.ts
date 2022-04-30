// ===========================================================================
// File: incident-pagination-data.ts
import { Incident } from './incident';
import { LazyLoadEvent } from 'primeng/api';
//
export class IncidentPaginationData {
	public incidentsList: Incident[] = [];
	//
	public totalRecords: number = 0;
	//
	public loadEvent: string = '';
	//
	public message: string = '';
	//
}
// ===========================================================================
