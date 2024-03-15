import { TestBed, fakeAsync } from '@angular/core/testing';
import { DOCUMENT } from '@angular/common';
import { ThemeService } from './theme.service';
//
describe('ThemeService', () => {
	let sut: ThemeService;
	let document: Document;
	const docmentServiceSpy = jasmine.createSpyObj('Document', ['getElementById']);
	//
	beforeEach( fakeAsync( ( ) => {
		TestBed.configureTestingModule( {
			imports: [
			],
			providers: [
				ThemeService,
				{ provide: Document, useValue: document }
			]
		} );
		document = TestBed.inject(DOCUMENT);
		const mockedElement: Element = jasmine.createSpyObj('element', ['getElementById']);
		// Setup sut
		sut = TestBed.inject( ThemeService );
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
		spyOn(document, 'getElementById').and.callFake( (elementId: string) => _themeLink as HTMLLinkElement );
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
		spyOn(document, 'getElementById').and.callFake( (elementId: string) => _themeLink as HTMLLinkElement );
		// when
		const ret = sut.switchTheme( !isDark );
		// then
		expect( ret ).toEqual( 0 );
		expect( sut.isDarkTheme ).toEqual( !isDark );
	});
	//
	it('switchTheme: should fail and not change light theme ...', () => {
		// given
		const isDark: boolean = false;
		sut._isDarkTheme = isDark;
		let _themeLink: HTMLLinkElement;
		spyOn(document, 'getElementById').and.callFake( (elementId: string) => _themeLink as HTMLLinkElement );
		// when
		const ret = sut.switchTheme( !isDark );
		// then
		expect( ret ).toEqual( 2 );
		expect( sut.isDarkTheme ).toEqual( isDark );
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
