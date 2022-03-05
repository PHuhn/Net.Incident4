// ===========================================================================
import { Component, OnInit } from '@angular/core';

@Component({
	selector: 'app-about',
	templateUrl: './about.component.html'
})
export class AboutComponent {
	//
	applicationName = 'Network Incident';
	companyName = 'Northern Software Group';
	copyright ='Copyright © 2022';
	// (angular version).major.minor.build
	// major is application version
	ng_version ='13';
	version = `${this.ng_version}.4.0.0`;
	//
	constructor() { }
	//
}
// ===========================================================================
