import { TestBed, ComponentFixture, inject, fakeAsync, tick } from '@angular/core/testing';
import { FormsModule, NgForm } from '@angular/forms';

import { ThemeService } from './theme.service';
import { ToggleThemeComponent } from './toggle-theme.component';

describe('ToggleThemeComponent', () => {
	let sut: ToggleThemeComponent;
	let fixture: ComponentFixture<ToggleThemeComponent>;
	const themeServiceSpy = jasmine.createSpyObj('ThemeService', ['switchTheme']);

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			declarations: [ToggleThemeComponent
			],
			providers: [
				{ provide: ThemeService, useValue: themeServiceSpy }
			]
		})
		.compileComponents();
		fixture = TestBed.createComponent(ToggleThemeComponent);
		sut = fixture.componentInstance;
		fixture.detectChanges();
	});
	//
	it('ToggleThemeComponent: should create ...', () => {
		expect( sut ).toBeTruthy();
	});
	/*
	** changeTheme(theme: boolean): number
	*/
	it('changeTheme: should successfully change dark theme ...', () => {
		// given
		const isDark: boolean = true;
		themeServiceSpy.switchTheme.and.returnValue( 0 );
		// when
		const ret = sut.changeTheme( isDark );
		// then
		expect( ret ).toEqual( 0 );
	});
	//
	it('changeTheme: should successfully change light theme ...', () => {
		// given
		const isDark: boolean = false;
		themeServiceSpy.switchTheme.and.returnValue( 0 );
		// when
		const ret = sut.changeTheme( isDark );
		// then
		expect( ret ).toEqual( 0 );
	});
	//
	it('changeTheme: should fail to change theme ...', () => {
		// given
		const isDark: boolean = false;
		themeServiceSpy.switchTheme.and.returnValue( 1 );
		// when
		const ret = sut.changeTheme( isDark );
		// then
		expect( ret ).toEqual( 1 );
	});
	/*
	** get isDarkMode()
	*/
	it('isDarkMode: should successfully return dark theme ...', () => {
		// given
		const isDark: boolean = true;
		Object.defineProperty( themeServiceSpy, 'isDarkTheme', {get: () => isDark});
		// when
		const ret = sut.isDarkMode;
		// then
		expect( ret ).toEqual( isDark );
	});
	//
});
