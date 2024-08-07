// ===========================================================================
// HttpClient use HttpClientModule https://github.com/angular/angular/issues/11694
//
import { withInterceptors, provideHttpClient } from '@angular/common/http';
//
// infrastructure services
import { ConfirmationService } from 'primeng/api';
import { AlertsService } from './global/alerts/alerts.service';
import { ConsoleLogService } from './global/console-log/console-log.service';
// user/auth services
import { UserService } from './net-incident/services/user.service';
import { AuthService } from './net-incident/services/auth.service';
// https://ryanchenkie.com/angular-authentication-using-the-http-client-and-http-interceptors
import { AuthInterceptorService } from './net-incident/services/auth-interceptor.service';
// application services
import { ServicesService } from './net-incident/services/services.service';
import { IncidentService } from './net-incident/services/incident.service';
import { NetworkIncidentService } from './net-incident/services/network-incident.service';
//
export const APP_PROVIDERS = [
	UserService,
	AuthService,
	provideHttpClient(withInterceptors([AuthInterceptorService])),
	//
	ServicesService,
	IncidentService,
	NetworkIncidentService
];
// ===========================================================================
