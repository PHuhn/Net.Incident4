// ===========================================================================
// File: loading-spinner.component.ts
import { Component, Input } from '@angular/core';
//
@Component({
    selector: 'app-loading-spinner',
    template: `		<ng-template [ngIf]='_loading'>
			<span>
			&nbsp; &nbsp;
			<p-progressSpinner id='loadingSpinner' [style]="{width: '30px', height: '30px'}"
				animationDuration='1.0s' aria-label='Loading' strokeWidth='6'>
			</p-progressSpinner>
			</span>
		</ng-template>`,
    standalone: false
})
export class LoadingSpinnerComponent {
	_loading: boolean = false;
	@Input() set loading( val: boolean ) {
		this._loading = val;
	}
}
// ===========================================================================
