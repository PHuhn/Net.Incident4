# NSG.NetIncident4.Core
## This Project

This project is currently a .Net Core 6.0 Web MVC/API application.  NSG.NetIncident4.Core was first created as a .Net Core 2.2 application, but due to authentication issues was never completed.  Latter, I implemented it in .Net Core 5.0.  With persistence, I was able to work through the lack of understanding issues. 

## Overview

### Identity

The front and back-ends can operate completely with a local Microsoft Identity user authentication and authorization.
The default roles are as follows:

- Admin
- CompanyAdmin
- User
- Public

The application has a complete set of the Microsoft Identity scaffolding.  Some the Identity scaffolding has been changed per requirement and style.

The application does allow for external logins from:

- Google
- Microsoft Account

### Application Maintenance (Views)

This maintains the following from the Network Incident application data:
- General User (Roles: User/Public/CompanyAdmin/Admin)
  - Manage your user account
  - View your event logs
  - RSS feeds (Security/News/Weather)
- Company Admin Users (Roles: CompanyAdmin/Admin)
  - Manage your company's user accounts
  - Manage your company's servers
  - Manage your company's email templates
- Admin Users (Roles: Admin)
  - Manage base email templates
  - Manage base Network Information Centers (NIC)
  - Manage base Network Incident note types
  - Manage base roles

### Application Web API

This exposes the following web API for the companion Angular Net-Incident application:

- Login,
- Write event logs,
- Network Incident
  - Incident Logs,
  - Incident Notes (emails/ping/whois/etc)

## The Library dependencies
- NSG.NetIncident4.Core requires [NSG.Library.Helpers](https://github.com/PHuhn/NSG.Library/wiki/NSG.Library), a set of static helper methods used to access command line O/S commands,
- NSG.NetIncident4.Core requires [NSG.MimeKit.Extensions](https://github.com/PHuhn/NSG.Library/wiki/NSG.Library), a set of static fluent methods used to access MimeKit's email interface,
- NSG.NetIncident4.Core requires PrimeFaces NG library of [NSG.PrimeNG.LazyLoding](https://github.com/PHuhn/NSG.PrimeNG), a set of static fluent methods to handle the backend of the Angular PrimeNG's grid's paging/sorting/filtering interface.

## Installation Instructions ##
* [Installation of development NSG.NetIncident4.Core](https://github.com/PHuhn/Net.Incident4/wiki/Installation-of-NSG-NetIncident4-Core-(Web-API-and-Identity))
* [Installation of the Angular CLI net-incident application](https://github.com/PHuhn/Net.Incident4/wiki/Installation-of-Angular-net-incident-frontend)

## Other Documents ##
* [Testing of the Angular CLI net-incident application](https://github.com/PHuhn/net-incident/wiki/Testing-Angular-net-incident-application)
* [Loading Network Incident Logs from Netgear Router](https://github.com/PHuhn/net-incident/wiki/Loading-Network-Incident-Logs)
* [UI Help](https://github.com/PHuhn/net-incident/wiki/UI-Help)
