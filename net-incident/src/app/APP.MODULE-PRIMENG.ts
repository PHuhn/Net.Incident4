// ===========================================================================
// File: APP.MODULE-PRIMENG.ts
import { SharedModule } from 'primeng/api';
import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DropdownModule } from 'primeng/dropdown';
import { MenubarModule } from 'primeng/menubar';
import { FocusTrapModule } from 'primeng/focustrap';
import { ButtonModule } from 'primeng/button';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
//
export const APP_MODULE_PRIMENG = [
	SharedModule,
	TableModule,
	DialogModule,
	ConfirmDialogModule,
	DropdownModule,
	MenubarModule,
	FocusTrapModule,
	ButtonModule,
	ProgressSpinnerModule
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
