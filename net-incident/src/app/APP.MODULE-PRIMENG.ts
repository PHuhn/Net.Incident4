// ===========================================================================
// File: APP.MODULE-PRIMENG.ts
import { SharedModule } from 'primeng/api';
import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
// import { DropdownModule } from 'primeng/dropdown'; remove in PrimeNG 20
import { SelectModule } from 'primeng/select';
import { MenubarModule } from 'primeng/menubar';
import { FocusTrapModule } from 'primeng/focustrap';
import { ButtonModule } from 'primeng/button';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { CardModule } from 'primeng/card';
//
export const APP_MODULE_PRIMENG = [
	SharedModule,
	TableModule,
	DialogModule,
	ConfirmDialogModule,
	// DropdownModule,
	SelectModule,
	MenubarModule,
	FocusTrapModule,
	ButtonModule,
	ProgressSpinnerModule,
	CardModule
];
/**
** Array of PrimeNG components.
** Used in tests...
*/
import { ConfirmDialog } from 'primeng/confirmdialog';
//
export const APP_PRIMENG_COMPONENTS = [
	ConfirmDialog,
];
/**
** Array of PrimeNG services
*/
import { ConfirmationService } from 'primeng/api';
//
export const APP_PRIMENG_PROVIDERS = [
	ConfirmationService,
];
// ===========================================================================
