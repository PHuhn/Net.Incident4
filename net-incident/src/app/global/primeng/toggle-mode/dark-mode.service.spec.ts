// ===========================================================================
// file: theme.service.spec.ts
import { TestBed, fakeAsync } from '@angular/core/testing';
import { DOCUMENT } from '@angular/common';
import { DarkModeService } from './dark-mode.service';
//
describe('DarkModeService', () => {
	let sut: DarkModeService;
	let document: Document;
	//
	beforeEach( fakeAsync( ( ) => {
		TestBed.configureTestingModule( {
			imports: [
			],
			providers: [
				DarkModeService,
				{ provide: Document, useValue: document }
			]
		} );
		document = TestBed.inject(DOCUMENT);
		// Setup sut
		sut = TestBed.inject( DarkModeService );
		TestBed.compileComponents();
	} ) );
	//
	it('should be created ...', () => {
		expect( sut ).toBeTruthy();
	});
	/*
	** switchTheme(isDark: boolean): number
	*/
	it('switchTheme: should successfully return dark theme ...', () => {
		// given
		const isDark: boolean = false;
		sut._isDarkTheme = isDark;
		const _themeLink: HTMLLinkElement = document.createElement( 'link' );
		spyOn(document, 'getElementById').and.callFake( ( ) => _themeLink as HTMLLinkElement );
		// when
		const ret = sut.switchTheme( !isDark );
		// then
		expect( ret ).toEqual( 0 );
		expect( sut.isDarkTheme ).toEqual( !isDark );
	});
	//
	it('switchTheme: should successfully return light theme ...', () => {
		// given
		const isDark: boolean = true;
		sut._isDarkTheme = isDark;
		const _themeLink: HTMLLinkElement = document.createElement( 'link' );
		spyOn(document, 'getElementById').and.callFake( ( ) => _themeLink as HTMLLinkElement );
		// when
		const ret = sut.switchTheme( !isDark );
		// then
		expect( ret ).toEqual( 0 );
		expect( sut.isDarkTheme ).toEqual( !isDark );
	});
	/*
	** get isDarkTheme(): boolean
	*/
	it('isDarkTheme: should successfully return dark theme ...', () => {
		// given
		const isDark: boolean = true;
		sut._isDarkTheme = isDark;
		// when
		const ret = sut.isDarkTheme;
		// then
		expect( ret ).toEqual( isDark );
	});
	//
	it('isDarkTheme: should successfully return light theme ...', () => {
		// given
		const isDark: boolean = false;
		sut._isDarkTheme = isDark;
		// when
		const ret = sut.isDarkTheme;
		// then
		expect( ret ).toEqual( isDark );
	});
	//
});
// ===========================================================================
