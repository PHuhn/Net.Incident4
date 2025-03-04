// ===========================================================================
// File: APP.GLOBAL.ts
import { AlertsComponent } from './alerts/alerts.component';
import { TruncatePipe } from './truncate.pipe';
import { ToggleModeComponent } from './primeng/toggle-mode/toggle-mode.component';
import { LoadingSpinnerComponent } from './primeng/loading-spinner/loading-spinner.component';
// PrimeNG custome component
import { FilterSummaryComponent } from './primeng/filter-summary/filter-summary.component';
// global providers
import { AlertsService } from './alerts/alerts.service';
import { ConsoleLogService } from './console-log/console-log.service';
import { BaseCompService } from './base-comp/base-comp.service';
//
export const APP_GLOBAL_COMPONENTS = [
	// global
	AlertsComponent,
	TruncatePipe,
	ToggleModeComponent,
	LoadingSpinnerComponent,
	//
	FilterSummaryComponent,
	//
];
//
export const APP_GLOBAL_PROVIDERS = [
	AlertsService,
	ConsoleLogService,
	BaseCompService,
];
// ===========================================================================
