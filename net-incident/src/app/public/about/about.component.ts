// ===========================================================================
import { Component, OnInit } from '@angular/core';
import pkg from 'package.json'

@Component({
	selector: 'app-about',
	templateUrl: './about.component.html'
})
export class AboutComponent {
	//
	applicationName = 'Network Incident';
	companyName = 'Phillip N. Huhn, DBA Northern Software Group';
	copyright ='Copyright © 2022';
	// (angular version).major.minor.build
	// major is application version
	version = `${pkg.version}`;
	ng_version = (pkg.version.split('.'))[0];
	//
	constructor() { }
	//
}
// ===========================================================================
