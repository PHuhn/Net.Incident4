// ===========================================================================
// File: base-comp.component.spec.ts
import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
//
import { ConfirmationService, Confirmation } from 'primeng/api';
//
import { AlertsService } from '../alerts/alerts.service';
import { ConsoleLogService } from '../console-log/console-log.service';
import { BaseCompService } from './base-comp.service';
import { BaseComponent } from './base-comp.component';
//
describe('BaseCompComponent', () => {
	let sut: BaseComponent;
	let fixture: ComponentFixture<BaseComponent>;
	let alertService: AlertsService;
	let consoleService: ConsoleLogService;
	let confirmService: ConfirmationService;
	//
	beforeEach(async () => {
		await TestBed.configureTestingModule({
			declarations: [
				BaseComponent
			],
			providers: [
				BaseCompService,
				AlertsService,
				ConsoleLogService,
				ConfirmationService
			]
		} );
		TestBed.compileComponents( );
	});
	//
	beforeEach(() => {
		fixture = TestBed.createComponent(BaseComponent);
		sut = fixture.componentInstance;
		alertService = sut._baseServices._alerts;
		consoleService = sut._baseServices._console;
		confirmService = sut._baseServices._confirmationService;
		fixture.detectChanges();
	});
	//
	it('should create ...', () => {
		expect( sut ).toBeTruthy();
	});
	/*
	** baseErrorHandler( where: string, what: string, error: string )
	*/
	it( 'baseErrorHandler should call alert service ...', fakeAsync( ( ) => {
		// given
		spyOn( alertService, 'setWhereWhatError' );
		// when
		sut.baseErrorHandler( 'where', 'what', 'error' );
		// then
		expect( alertService.setWhereWhatError ).toHaveBeenCalledWith( 'where', 'what', 'error' );
		//
	} ) );
	//
	it( 'baseErrorHandler should call alert service with undefined ...', fakeAsync( ( ) => {
		// given
		spyOn( alertService, 'setWhereWhatError' );
		// when
		sut.baseErrorHandler( 'where', 'what', undefined );
		// then
		expect( alertService.setWhereWhatError ).toHaveBeenCalledWith( 'where', 'what', 'Server error' );
		//
	} ) );
	//
	it('ConsoleLogService should take _console Error message ...', () => {
		// given/when
		const _ret = sut._baseServices._console.Error('Error message');
		// then
		expect( _ret ).toEqual( 'Error: Error message' );
	});
	//
	it('ConfirmationService should exist ...', () => {
		// given/when/then
		expect( sut._baseServices._confirmationService ).toBeDefined( );
	});
	/*
	** baseDeleteConfirm<T>( id: T, callBack: DeleteCallback<T> ): boolean
	*/
	it('baseDeleteConfirm should callback when accept ...', fakeAsync(() => {
		// given
		spyOn( consoleService, 'Information' );
		spyOn(confirmService, 'confirm').and.callFake(
			(confirmation: Confirmation) => {
				console.log(confirmation.message);
				expect( confirmation.message ).toEqual( `Are you sure you want to delete Display (id-value)?` );
				if( confirmation.accept !== undefined ) {
					return confirmation.accept();
				}
			});
		const id: string = 'id-value';
		// when
		const ret: boolean = sut.baseDeleteConfirm<string>( id, (ident: string): boolean => {
			expect( ident ).toEqual( id );
			return true;
		}, 'Display' );
		// then
		expect( ret ).toEqual( false );
		tick( 1 ); // give it very small amount of time
		expect( consoleService.Information )
			.toHaveBeenCalledWith( `base-component.baseDeleteConfirm: User's response: true` );
	}));
	//
	it('baseDeleteConfirm should Cancel when reject ...', fakeAsync(() => {
		// given
		// console.log( `baseDeleteConfirm should Cancel when reject ...` )
		// consoleService.Warning( 'here' );
		// consoleService.Success( 'here' );
		// consoleService.Information( 'here' );
		// consoleService.Verbose( 'here' );
		// consoleService.Debug( 'here' );
		spyOn( consoleService, 'Verbose' );
		spyOn( confirmService, 'confirm' ).and.callFake(
			( confirmation: Confirmation ) => {
				if( confirmation.reject !== undefined ) {
					return confirmation.reject();
				}
				return false;
			});
		sut._baseServices._console.logLevel = 5;
		const id: object = { field1: 'one', field2: 'two'};
		// when
		const ret: boolean = sut.baseDeleteConfirm<object>( id, (): boolean => {
			consoleService.Warning( 'Delete canceled.' );
			fail( 'baseDeleteConfirm should Cancel' );
			return true;
		} );
		// then
		expect( ret ).toEqual( false );
		tick( 1 ); // give it very small amount of time
		expect( consoleService.Verbose )
			.toHaveBeenCalledWith( `base-component.baseDeleteConfirm: User's dismissed.` );
		expect( consoleService.lastMessage )
			.toEqual( 'Info: base-component.baseDeleteConfirm: one - two' );
	}));
	//
});
// ===========================================================================
