// ===========================================================================
// File: base-srvc.spec.ts
import { IBaseClass, BaseClass } from './base-class';
//
export interface ITestNote extends IBaseClass {
	TestNoteId: number;
	TestNoteShortDesc: string;
	TestNoteDesc: string;
}
//
export class TestNote extends BaseClass implements ITestNote {
	constructor(
		public TestNoteId: number,
		public TestNoteShortDesc: string,
		public TestNoteDesc: string,
	) { super( ); }
}
//
export class TestData extends BaseClass {
	constructor(
		public ChangeDate: Date,
		public Id: number,
		public Description: string
	) { super( ); }
}
//
describe('BaseClass', () => {
	//
	const testNote: ITestNote = new TestNote( 75, 'Short', 'Description' );
	//
	it('should create an instance', () => {
		expect(new BaseClass()).toBeTruthy();
	});
	/*
	** toString for class Incident.
	*/
	it('toString should output TestNote class ...', () => {
		const toStringValue: string = '{"TestNoteId":75,"TestNoteShortDesc":"Short","TestNoteDesc":"Description"}';
		expect( testNote.toString() ).toEqual( toStringValue );
	});
	//
	it('toString should output TestData class ...', () => {
		const testData: TestData = new TestData( new Date( '2000-01-01T00:00:00' ), 1, 'a 1' );
		const toStringValue: string = '{"ChangeDate":"2000-01-01T05:00:00.000Z","Id":1,"Description":"a 1"}';
		expect( testData.toString() ).toEqual( toStringValue );
	});
	/*
	** Clone the current class.
	*/
	it('Clone should output TestData class ...', () => {
		const testData: TestData = new TestData( new Date( '2000-01-01T00:00:00' ), 1, 'a 1' );
		const testClone: TestData = testData.Clone();
		expect( { ... testData } ).not.toBe( testData );
		expect( testClone.ChangeDate ).toEqual( new Date( '2000-01-01T00:00:00' ) );
		expect( testClone.Id ).toEqual( 1 );
		expect( testClone.Description ).toEqual( 'a 1' );
	});
	//
	it('Clone should output TestNote class ...', () => {
		const testNote: ITestNote = new TestNote( 22, 'Short', 'Long Description' );
		const testClone: ITestNote = testNote.Clone();
		expect( { ... testNote } ).not.toBe( testNote );
		expect( testClone.TestNoteId ).toEqual( 22 );
		expect( testClone.TestNoteShortDesc ).toEqual( 'Short' );
		expect( testClone.TestNoteDesc ).toEqual( 'Long Description' );
	});
	//
});
// ===========================================================================
