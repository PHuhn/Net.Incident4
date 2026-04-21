// ===========================================================================
// File: loading.spinner.component.ts
import { DebugElement } from '@angular/core';
import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
//
import { ProgressSpinnerModule } from 'primeng/progressspinner';
//
import { LoadingSpinnerComponent } from './loading-spinner.component';
//
describe('LoadingSpinnerComponent', () => {
	let sut: LoadingSpinnerComponent;
	let fixture: ComponentFixture<LoadingSpinnerComponent>;
	//
	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [
				ProgressSpinnerModule
			],
			declarations: [
				LoadingSpinnerComponent,
    		]
		})
		.compileComponents();
		//
		fixture = TestBed.createComponent(LoadingSpinnerComponent);
		sut = fixture.componentInstance;
		fixture.detectChanges();
	});
	/*
	** Cleanup so no dialog window will still be open
	*/
	function setLoadingInput( value: boolean ): void {
		fixture.componentRef.setInput('loading', value);
		// Trigger change detection to reflect the update
		fixture.detectChanges();
	}
	/*
	** Cleanup so no dialog window will still be open
	*/
	function windowCleanup( ): void {
		setLoadingInput( false );
		tickFakeWait( 1 );
	}
	/*
	** Pause for events to process.
	*/
	function tickFakeWait( ticks: number ) {
		tick( ticks );
		fixture.detectChanges( ); // trigger initial data binding
		fixture.whenStable( );
	}
	//
	it('LoadingSpinner: should create', () => {
		expect(sut).toBeTruthy();
	});
	//
	it( 'LoadingSpinner: should render if loading ...', fakeAsync( ( ) => {
		// given
		setLoadingInput( false );
		tickFakeWait( 10 );
		// when
		setLoadingInput( true );
		tickFakeWait( 10 );
		// then
		const spinner: HTMLDivElement = fixture.debugElement.query(By.css(
			'#loadingSpinner' )).nativeElement;
		expect( spinner ).toBeDefined( );
		windowCleanup( );
	} ) );
	//
	it( 'LoadingSpinner: should stop render if loading ...', fakeAsync( ( ) => {
		// given
		setLoadingInput( false );
		tickFakeWait( 10 );
		setLoadingInput( true );
		tickFakeWait( 10 );
		// when
		setLoadingInput( false );
		tickFakeWait( 10 );
		// then
		const spinner: DebugElement = fixture.debugElement.query(By.css( '#loadingSpinner' ));
		expect( spinner ).toBeNull();
		windowCleanup( );
	} ) );
	//
});
// ===========================================================================
