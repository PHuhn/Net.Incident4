import { TestBed, ComponentFixture } from '@angular/core/testing';

import { DarkModeService } from './dark-mode.service';
import { ToggleModeComponent } from './toggle-mode.component';

describe('ToggleModeComponent', () => {
	let sut: ToggleModeComponent;
	let fixture: ComponentFixture<ToggleModeComponent>;
	const darkModeServiceSpy = jasmine.createSpyObj('DarkModeService', ['switchTheme']);

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			declarations: [ToggleModeComponent
			],
			providers: [
				{ provide: DarkModeService, useValue: darkModeServiceSpy }
			]
		})
		.compileComponents();
		fixture = TestBed.createComponent(ToggleModeComponent);
		sut = fixture.componentInstance;
		fixture.detectChanges();
	});
	//
	it('ToggleModeComponent: should create ...', () => {
		expect( sut ).toBeTruthy();
	});
	/*
	** changeTheme(theme: boolean): number
	*/
	it('changeTheme: should successfully change dark theme ...', () => {
		// given
		const isDark: boolean = true;
		darkModeServiceSpy.switchTheme.and.returnValue( 0 );
		// when
		const ret = sut.changeTheme( isDark );
		// then
		expect( ret ).toEqual( 0 );
	});
	//
	it('changeTheme: should successfully change light theme ...', () => {
		// given
		const isDark: boolean = false;
		darkModeServiceSpy.switchTheme.and.returnValue( 0 );
		// when
		const ret = sut.changeTheme( isDark );
		// then
		expect( ret ).toEqual( 0 );
	});
	//
	it('changeTheme: should fail to change theme ...', () => {
		// given
		const isDark: boolean = false;
		darkModeServiceSpy.switchTheme.and.returnValue( 1 );
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
		Object.defineProperty( darkModeServiceSpy, 'isDarkTheme', {get: () => isDark});
		// when
		const ret = sut.isDarkMode;
		// then
		expect( ret ).toEqual( isDark );
	});
	//
});
