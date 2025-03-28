// ===========================================================================
import { ComponentFixture, TestBed, inject, fakeAsync, tick, waitForAsync } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule, NgForm } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Observable, throwError } from 'rxjs';
import { By } from '@angular/platform-browser';
//
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { FocusTrapModule } from 'primeng/focustrap';
import { SelectItem } from 'primeng/api';
//
import { ServerSelectionWindowComponent } from './server-selection-window.component';
import { SelectItemClass } from '../../global/primeng/select-item-class';
//
describe('ServerSelectionWindowComponent', () => {
	let sut: ServerSelectionWindowComponent;
	let fixture: ComponentFixture<ServerSelectionWindowComponent>;
	//
	const windowTitleSelector: string =
		'#serverSelectionWindow > div > div > div.p-dialog-header';
	// document.querySelector('#serverSelectionWindow > div > div > div > span.p-dialog-title > p-header')
	const expectedWindowTitle: string = 'Select a server';
	const mockData: SelectItem[] = [
		new SelectItemClass( 'nsg-1', 'Router 1' ),
		new SelectItemClass( 'nsgServ2', 'Router 2' ),
		new SelectItemClass( 'nsg-3', 'Web Server' )
	];
	const displayWindow: boolean = true;
	//
	beforeEach(fakeAsync(() => {
		TestBed.configureTestingModule({
			imports: [
				FormsModule,
				FocusTrapModule,
				BrowserAnimationsModule,
				DialogModule
			],
			declarations: [
				ServerSelectionWindowComponent,
			],
			// Unhandled promise rejection: Error: NG0205: Injector has already been destroyed.
			teardown: {destroyAfterEach: false}
		})
		.compileComponents();
	}));
	//
	beforeEach(() => {
		fixture = TestBed.createComponent(ServerSelectionWindowComponent);
		sut = fixture.componentInstance;
		sut.selectItems = mockData;
		sut.displayWin = displayWindow;
		fixture.detectChanges();
		fixture.whenStable( );
	});
	/*
	** Cleanup so no dialog window will still be open
	*/
	function windowCleanup( ) {
		sut.displayWin = false;
		fixture.detectChanges( ); // trigger initial data binding
		fixture.whenStable( );
	}
	//
	it('should be created ...', fakeAsync( () => {
		console.log(
			'===================================\n' +
			'ServerSelectionWindowComponent should create ...' );
		expect(sut).toBeTruthy();
		windowCleanup( );
	} ) );
	//
	it('should initialize (input) with all server data ...', fakeAsync( () => {
		expect(sut.model.length).toEqual(mockData.length);
		windowCleanup( );
	} ) );
	//
	it('should accept display window (input) ...', fakeAsync( () => {
		expect(sut.displayWin).toEqual(displayWindow);
		windowCleanup( );
	} ) );
	//
	it('@Input should get back the input values ...', fakeAsync( () => {
		expect(sut.selectItems).toEqual(mockData);
		windowCleanup( );
	} ) );
	//
	it('should launch window when display window set ...', fakeAsync( () => {
		// const titleVar = fixture.debugElement.query(By.css(
		// 	'#serverSelectionWindow' )).nativeElement;
		// console.warn( titleVar );
		const title: HTMLDivElement = fixture.debugElement.query(By.css(
			windowTitleSelector )).nativeElement;
		console.warn( title );
		expect( title.innerText ).toEqual( expectedWindowTitle );
		windowCleanup( );
	} ) );
	//
	it('should return selected server 0 ...', fakeAsync( () => {
		const idx: number = 0;
		const server: SelectItem = mockData[ idx ];
		const value: string = server.label;
		const radioSelector: string =
			`div.p-dialog-content > div > div > div:nth-child(${idx + 1}) > input[type=radio]`;
		const radio: HTMLInputElement = fixture.debugElement.query(By.css(
			radioSelector )).nativeElement;
		spyOn( sut.emitClose, 'emit' );
		expect( radio.checked ).toBeFalsy(); // default state
		//
		radio.click();
		expect( sut.emitClose.emit ).toHaveBeenCalledWith( value );
		windowCleanup( );
	}));
	//
	it('should return selected server 1 ...', fakeAsync( () => {
		const idx: number = 1;
		const server: SelectItem = mockData[ idx ];
		const value: string = server.label;
		const radioSelector: string =
			`div.p-dialog-content > div > div > div:nth-child(${idx + 1}) > input[type=radio]`;
		const radio: HTMLInputElement = fixture.debugElement.query(By.css(
			radioSelector )).nativeElement;
		spyOn( sut.emitClose, 'emit' );
		expect( radio.checked ).toBeFalsy(); // default state
		//
		radio.click();
		expect( sut.emitClose.emit ).toHaveBeenCalledWith( value );
		windowCleanup( );
	}));
	//
});
// ===========================================================================
