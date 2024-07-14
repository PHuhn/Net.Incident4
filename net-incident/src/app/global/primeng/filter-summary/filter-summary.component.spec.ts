// ===========================================================================
// File: filter-summary.component.spec.ts
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
//
import { LazyLoadMeta } from 'primeng/api';
//
import { FilterSummaryComponent, AssocArray } from './filter-summary.component';
//
export class TestClass {
	constructor(
		public TestClassId: number,
		public TestClassType: string,
		public TestClassDescription: string,
		public SortOrder: number
	) { }
}
//
describe('FilterSummaryComponent', () => {
	let sut: FilterSummaryComponent;
	let fixture: ComponentFixture<FilterSummaryComponent>;
	//
	beforeEach(async () => {
		await TestBed.configureTestingModule({
			declarations: [FilterSummaryComponent]
		})
		.compileComponents();
		//
		fixture = TestBed.createComponent(FilterSummaryComponent);
		sut = fixture.componentInstance;
		fixture.detectChanges();
	});
	//
	it('should create', () => {
		expect(sut).toBeTruthy();
	});
	/*
	** displayTitle( s: string )
	*/
	it('displayTitle: should display format camel case string ...', () => {
		// given / when
		const display: string = sut.displayTitle( 'abbbCdddEff' );
		// then
		expect( display ).toEqual( 'Abbb Cddd Eff' );
	} );
	/*
	** formatFilters( event: LazyLoadMeta, translation: AssocArray ): string[]
	*/
	it('formatFilters: should not display string when no filters ...', () => {
		// given
		const event: LazyLoadMeta = {
			first: 0, rows: 10,
			filters: {}
		};
		const trans: AssocArray = { 'TestClassType': 'Type' };
		// when
		const display: string[] = sut.formatFilters( event, trans );
		// then
		expect( display.length ).toEqual( 0 );
	} );
	//
	it('formatFilters: should format filter display string ...', () => {
		// given
		const event: LazyLoadMeta = {
			first: 0, rows: 10,
			filters: {
				'TestClassType': [ { value: 'type 2', matchMode: 'equals' } ],
				'SortOrder':	 [ { value: 50, matchMode: 'gt' } ]
			}
		};
		const trans: AssocArray = { 'TestClassType': 'Type' };
		// when
		const display: string[] = sut.formatFilters( event, trans );
		// then
		expect( display[0] ).toEqual( 'Type (equals) type 2' );
		expect( display[1] ).toEqual( 'Sort Order (gt) 50' );
	} );
	//
	it('FilterSummary: should format filter display string ...', () => {
		// given
		const event: LazyLoadMeta = {
			first: 0, rows: 10,
			filters: {
				'TestClassType': [ { value: 'type 2', matchMode: 'equals' } ],
				'SortOrder':	 [ { value: 50, matchMode: 'gt' } ]
			}
		};
		sut.fieldDescriptions = { 'TestClassType': 'Type' };
		// when
		sut.lazyLoadEvent = event;
		// then
		expect( sut.filters[0] ).toEqual( 'Type (equals) type 2' );
		expect( sut.filters[1] ).toEqual( 'Sort Order (gt) 50' );
	} );
	//
	it('FilterSummary: should contain FilterSummary component ...', () => {
		// given
		sut.filters = [ 'Short Description (startsWith) S' ];
		fixture.detectChanges();
		// when
		const summary: HTMLDivElement = fixture.debugElement.query(
			By.css( 'div.nsg-summary-line' )).nativeElement;
		// then
		expect( summary ).toBeTruthy( );
		sut.filters = [];
	});
	//
});
// ===========================================================================
