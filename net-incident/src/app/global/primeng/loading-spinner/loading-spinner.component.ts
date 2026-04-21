// ===========================================================================
// File: loading-spinner.component.ts
import { Component, input } from '@angular/core';
//
@Component({
	selector: 'app-loading-spinner',
	template: `		@if (loading()) {
		  <span>
		    &nbsp; &nbsp;
		    <p-progressSpinner id='loadingSpinner' [style]="{width: '30px', height: '30px'}"
		      animationDuration='1.0s' aria-label='Loading' strokeWidth='6'>
		    </p-progressSpinner>
		  </span>
		}`,
	standalone: false
})
export class LoadingSpinnerComponent {
	loading = input(false); // Returns an InputSignal<boolean>
}
// ===========================================================================
