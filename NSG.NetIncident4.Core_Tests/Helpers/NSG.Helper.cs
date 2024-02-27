using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Reflection;
//
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Moq;
//
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Persistence;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using MockQueryable.Moq;
using NSG.NetIncident4.Core.Application.Commands.ApplicationUsers;
using Microsoft.AspNetCore.Hosting.Server;
using NSG.NetIncident4.Core.Application.Commands.Logs;
using NSG.NetIncident4.Core.Application.Commands.Servers;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using NSG.NetIncident4.Core.Application.Commands.Incidents;
//
namespace NSG.Integration.Helpers
{
    //
    /// <summary>
    /// Static helper methods
    /// </summary>
    public static class NSG_Helpers
    {
        //
        // Testing constants
        //
        public static string User_Name = "Phil";
        public static string User_Email = @"Phil@any.net";
        public static string Password = @"P@ssword0";
        public static string User_Name2 = "author";
        public static string User_Email2 = @"author@any.net";
        public static string Password2 = @"P@ssword0";
        public static int companyId = 1;
        public static int serverId = 1;
        // Companies
        public static Company company1 = new Company()
        {
            CompanyId = companyId, CompanyShortName = "NSG", CompanyName = "Northern Software Group",
            Address = "123 Any St.", City = "Ann Arbor", State = "MI", PostalCode = "48104", Country = "USA",
            PhoneNumber = "(734)662-1688", Notes = "Nothing of note.",
        };
        public static Company company2 = new Company()
        {
            CompanyId = 2, CompanyShortName = "TST", CompanyName = "Testing Company",
            Address = "321 Any St.", City = "Ann Arbor", State = "MI", PostalCode = "48104", Country = "USA",
            PhoneNumber = "(734)662-1699", Notes = "Nothing of note."
        };
        //public static Company company3 = new Company()
        //{
        //    CompanyId = 3, CompanyShortName = "C 3", CompanyName = "C 3",
        //    Address = "C 3", City = "C 3", State = "MI", PostalCode = "48104", Country = "USA",
        //    PhoneNumber = "(734)662-1699", Notes = "Nothing of note."
        //};
        //public static Company company4 = new Company()
        //{
        //    CompanyId = 4, CompanyShortName = "C 4", CompanyName = "C 4",
        //    Address = "C 4", City = "C 4", State = "MI", PostalCode = "48104", Country = "USA",
        //    PhoneNumber = "(734)662-1699", Notes = "Nothing of note."
        //};
        //
        public static Server server1 = new Server()
        {
            ServerId = serverId, CompanyId = companyId, ServerShortName = "NSG Memb", ServerName = "Members Web-site", ServerDescription = "Public facing members Web-site",
            WebSite = "Web-site address: www.mimilk.com", ServerLocation = "We are in Michigan, USA.",
            FromName = "Phil Huhn", FromNicName = "Phil", FromEmailAddress = "PhilHuhn@yahoo.com",
            TimeZone = "EST (UTC-5)", DST = true, TimeZone_DST = "EDT (UTC-4)", DST_Start = new DateTime(2019, 3, 10, 2, 0, 0), DST_End = new DateTime(2019, 11, 3, 2, 0, 0),
            Company = company1,
        };
        public static Server server2 = new Server()
        {
            ServerId = 2, CompanyId = 1, ServerShortName = "SrvShort 2", ServerName = "Server Long Name 2", ServerDescription = "Server Description",
            WebSite = "WebSite", ServerLocation = "ServerLocation",
            FromName = "FromName", FromNicName = "FromNicName", FromEmailAddress = "FromEmailAddress",
            TimeZone = "TimeZone", DST = false, TimeZone_DST = "TimeZone_DST", DST_Start = new DateTime(2020, 3, 10, 2, 0, 0), DST_End = new DateTime(2020, 11, 3, 2, 0, 0),
            Company = company1,
        };
        //
        public static ApplicationUser user1 =
            new ApplicationUser { Id = "u1", Email = NSG_Helpers.User_Email, UserName = NSG_Helpers.User_Name, PasswordHash = "AQAAAAEAACcQAAAAEB4oAR8WhJGi5QVXpuONJ4z69YqF/69GlCz4TtjbQVLA4ef69x0iDq5GLTzvqM2vwQ==", SecurityStamp = "VFV7PXFFMU4VZF57I3T7A6TXVF4VAY2M", ConcurrencyStamp = "24240e95-400c-434e-b498-16542c90de13", CompanyId = companyId, FirstName = "Phillip", LastName = "Huhn", FullName = "Phillip Huhn", UserNicName = NSG_Helpers.User_Name, EmailConfirmed = true, Company = company1 };
        public static ApplicationUser user2 =
            new ApplicationUser { Id = "u2", Email = NSG_Helpers.User_Email2, UserName = NSG_Helpers.User_Name2, PasswordHash = "AQAAAAEAACcQAAAAEGG4L+8q4FXRLAhrLWuALpnyStwzaYOaVA6LjBUrHHqs3AreDKMm9DnRa3B4PM1DYg==", SecurityStamp = "LTCQ4W2NCVQRESG6ZWMC7IBMWDZNICK7", ConcurrencyStamp = "2dab2144-81e5-4b76-a09c-c3b6c37f0f3b", CompanyId = companyId, FirstName = "Author", LastName = "Huhn", FullName = "Author Huhn", UserNicName = "Art", Company = company1 };
        //
        public static List<ApplicationUser> usersFakeData = new List<ApplicationUser>()
        {
            user1, user2
        };
        public static List<Company> companiesFakeData = new List<Company>()
        {
            company1, company2, 
            // company3, company4
        };
        //
        public static List<Server> serversFakeData = new List<Server>()
        {
            server1, server2,
            new Server()
            {
                ServerId = 3,
                CompanyId = 1,
                ServerShortName = "SrvShort 3",
                ServerName = "Server Long Name 3",
                ServerDescription = "Server Description",
                WebSite = "WebSite",
                ServerLocation = "ServerLocation",
                FromName = "FromName",
                FromNicName = "FromNicName",
                FromEmailAddress = "FromEmailAddress",
                TimeZone = "TimeZone",
                DST = false,
                TimeZone_DST = "TimeZone_DST",
                DST_Start = new DateTime(2020, 3, 10, 2, 0, 0),
                DST_End = new DateTime(2020, 11, 3, 2, 0, 0)
            },
            new Server()
            {
                ServerId = 4,
                CompanyId = 2,
                ServerShortName = "SrvShort 4",
                ServerName = "Server Long Name 4",
                ServerDescription = "Server Description",
                WebSite = "WebSite",
                ServerLocation = "ServerLocation",
                FromName = "FromName",
                FromNicName = "FromNicName",
                FromEmailAddress = "FromEmailAddress",
                TimeZone = "TimeZone",
                DST = false,
                TimeZone_DST = "TimeZone_DST",
                DST_Start = new DateTime(2020, 3, 10, 2, 0, 0),
                DST_End = new DateTime(2020, 11, 3, 2, 0, 0)
            },
        };
        //
        public static List<ApplicationUserServer> userSrvrsFakeData = new List<ApplicationUserServer>()
        {
            new ApplicationUserServer() {Id = user1.Id, User = user1, ServerId = server1.ServerId, Server = server1},
            new ApplicationUserServer() {Id = user1.Id, User = user1, ServerId = server2.ServerId, Server = server2},
            new ApplicationUserServer() {Id = user2.Id, User = user2, ServerId = server1.ServerId, Server = server1},
        };
        //
        public static ApplicationRole admRole = new ApplicationRole() { Id = "adm", Name = "Admin" };
        public static ApplicationRole usrRole = new ApplicationRole() { Id = "usr", Name = "User" };
        public static List<ApplicationRole> rolesFakeData = new List<ApplicationRole>()
        {
            new ApplicationRole() { Id = "pub", Name = "Public" },
            new ApplicationRole() { Id = "usr", Name = "User" },
            new ApplicationRole() { Id = "adm", Name = "Admin" },
            new ApplicationRole() { Id = "cadm", Name = "CompanyAdmin" }
        };
        public static List<ApplicationUserRole> userRolesFakeData = new List<ApplicationUserRole>()
        {
            new ApplicationUserRole() { RoleId = "adm", Role = admRole, UserId = user1.Id, User = user1},
            new ApplicationUserRole() { RoleId = "usr", Role = usrRole, UserId = user2.Id, User = user2},
        };
        /*
        ** Network Incident Note Type
        */
        public static List<NoteType> noteTypesFakeData = new List<NoteType>()
        {
            new NoteType() { NoteTypeId = 1, NoteTypeShortDesc = "Ping", NoteTypeDesc = "Ping", NoteTypeClientScript = "ping" },
            new NoteType() { NoteTypeId = 2, NoteTypeShortDesc = "WhoIs", NoteTypeDesc = "WhoIs", NoteTypeClientScript = "whois" },
            new NoteType() { NoteTypeId = 3, NoteTypeShortDesc = "ISP Rpt", NoteTypeDesc = "Abuse Report to ISP", NoteTypeClientScript = "email" },
            new NoteType() { NoteTypeId = 4, NoteTypeShortDesc = "ISP Addl", NoteTypeDesc = "Additional Communication from ISP", NoteTypeClientScript = "" },
            new NoteType() { NoteTypeId = 5, NoteTypeShortDesc = "ISP Resp", NoteTypeDesc = "ISP Response", NoteTypeClientScript = "" }
        };
        /*
        ** NIC
        */
        public static List<NIC> nicsFakeData = new List<NIC>()
        {
            new NIC() { NIC_Id = "afrinic.net", NICDescription = "Africian Network Information Centre", NICAbuseEmailAddress = " ", NICRestService = "http://www.afrinic.net/", NICWebSite = "http://www.afrinic.net/" },
            new NIC() { NIC_Id = "apnic.net", NICDescription = "Asian-Pacfic Network Information Centre", NICAbuseEmailAddress = "abuse@org.apnic.net", NICRestService = "https://wq.apnic.net/whois-search/static/search.html?query=", NICWebSite = " " },
            new NIC() { NIC_Id = "arin.net", NICDescription = "Americian (North) Registry of Internet Numbers", NICAbuseEmailAddress = "abuse@arin.net", NICRestService = "http://whois.arin.net/rest/ip/", NICWebSite = "https://www.arin.net/" },
            new NIC() { NIC_Id = "lacnic.net", NICDescription = "Latin America and Caribbean Network Information Centre", NICAbuseEmailAddress = "abuse@lacnic.net", NICRestService = "https://rdap.lacnic.net/rdap-web/home", NICWebSite = "http://www.lacnic.net/web/lacnic/inicio" },
            new NIC() { NIC_Id = "jpnic.net", NICDescription = "Japan", NICAbuseEmailAddress = " ", NICRestService = "https://wq.apnic.net/whois-search/static/search.html?query=", NICWebSite = " " },
            new NIC() { NIC_Id = "nic.br", NICDescription = "Brazilian Network Information Center", NICAbuseEmailAddress = "cert@cert.br", NICRestService = "https://registro.br/2/whois?query=", NICWebSite = " " },
            new NIC() { NIC_Id = "ripe.net", NICDescription = "Réseaux IP Européens Network Coordination Centre (Europe)", NICAbuseEmailAddress = "abuse@ripe.net", NICRestService = "https://apps.db.ripe.net/db-web-ui/#/query?searchtext=", NICWebSite = "https://www.ripe.net/" },
            new NIC() { NIC_Id = "twnic.net", NICDescription = "Taiwan NIC", NICAbuseEmailAddress = " ", NICRestService = "https://www.twnic.net.tw/en_index.php", NICWebSite = "https://www.twnic.net.tw/" },
            new NIC() { NIC_Id = "hostwinds.com", NICDescription = "hostwinds NIC", NICAbuseEmailAddress = " ", NICRestService = " ", NICWebSite = "https://www.hostwinds.com/" },
            new NIC() { NIC_Id = "unknown", NICDescription = "Unknown", NICAbuseEmailAddress = " ", NICRestService = " ", NICWebSite = " " },
            new NIC() { NIC_Id = "other", NICDescription = "Other", NICAbuseEmailAddress = " ", NICRestService = " ", NICWebSite = " " }
        };
        /*
        ** Incident Types
        */
        public static List<IncidentType> incidentTypesFakeData = new List<IncidentType>()
        {
            new IncidentType() { IncidentTypeId = 1, IncidentTypeShortDesc = "Unk", IncidentTypeDesc = "Unknown", IncidentTypeFromServer = true, IncidentTypeSubjectLine = "Unknown probe from ${IPAddress}", IncidentTypeEmailTemplate = "Hi\\n\\nStop the intrusion from your IP address ${IPAddress}.\\nThe following IP address probe my network.\\nPlease contain the following reference # in all communications: ${IncidentId}\\n\\n${Device}\\n${ServerLocation}\\nIncident times:", IncidentTypeTimeTemplate = "${NetworkLogDate} ${TimeZone}", IncidentTypeThanksTemplate = "\\nThank you,\\n${FromName}\\n================", IncidentTypeLogTemplate = "\\n${Log}\\n--------------------------------", IncidentTypeTemplate = "-" },
            new IncidentType() { IncidentTypeId = 2, IncidentTypeShortDesc = "Multiple", IncidentTypeDesc = "Multiple Types", IncidentTypeFromServer = true, IncidentTypeSubjectLine = "Network abuse from ${IPAddress}", IncidentTypeEmailTemplate = "Hi\\n\\nStop the intrusion from your IP address ${IPAddress}.\\nThe following IP address probe my network, probing for multiple vulnerabilities.\\nPlease contain the following reference # in all communications: ${IncidentId}\\n\\n${Device}\\n${ServerLocation}\\nIncident times:", IncidentTypeTimeTemplate = "${IncidentTypeShortDesc}: ${NetworkLogDate} ${TimeZone}", IncidentTypeThanksTemplate = "\\nThank you,\\n${FromName}\\n================", IncidentTypeLogTemplate = "\\n${Log}\\n--------------------------------", IncidentTypeTemplate = "-" },
            new IncidentType() { IncidentTypeId = 3, IncidentTypeShortDesc = "SQL", IncidentTypeDesc = "SQL Injection", IncidentTypeFromServer = true, IncidentTypeSubjectLine = "SQL Injection probe from ${IPAddress}", IncidentTypeEmailTemplate = "Hi\\n\\nStop the intrusion from your IP address ${IPAddress}.  This is testing SQL injection vulnerabilities.\\nPlease contain the following reference # in all communications: ${IncidentId}\\n\\n${Device}\\n${ServerLocation}\\n\\nIncident times:", IncidentTypeTimeTemplate = "${NetworkLogDate} ${TimeZone}", IncidentTypeThanksTemplate = "\\nThank you,\\n${FromName}\\n================", IncidentTypeLogTemplate = "\\n${Log}\\n--------------------------------", IncidentTypeTemplate = "-" },
            new IncidentType() { IncidentTypeId = 4, IncidentTypeShortDesc = "PHP", IncidentTypeDesc = "PHP", IncidentTypeFromServer = true, IncidentTypeSubjectLine = "PHP probe from ${IPAddress}", IncidentTypeEmailTemplate = "Hi\\n\\nStop the intrusion from your IP address ${IPAddress}.\\nThe following IP address probe my network, probing for ${IncidentTypeDesc} vulnerabilities.\\nPlease use the following reference # in all communications: ${IncidentId}\\n\\n${Device}\\n${ServerLocation}\\n\\nIncident times:", IncidentTypeTimeTemplate = "${NetworkLogDate} ${TimeZone}", IncidentTypeThanksTemplate = "\\nThank you,\\n${FromName}\\n================", IncidentTypeLogTemplate = "\\n${Log}\\n--------------------------------", IncidentTypeTemplate = "-" },
            new IncidentType() { IncidentTypeId = 5, IncidentTypeShortDesc = "XSS", IncidentTypeDesc = "Cross Site Scripting", IncidentTypeFromServer = true, IncidentTypeSubjectLine = "XSS probe from ${IPAddress}", IncidentTypeEmailTemplate = "Hi\\n\\nStop the intrusion from your IP address ${IPAddress}.\\nThe following IP address probe my network, probing for ${IncidentTypeDesc} vulnerabilities.\\nPlease use the following reference # in all communications: ${IncidentId}\\n\\n${Device}\\n${ServerLocation}\\n\\nIncident times:", IncidentTypeTimeTemplate = "${NetworkLogDate} ${TimeZone}", IncidentTypeThanksTemplate = "\\nThank you,\\n${FromName}\\n================", IncidentTypeLogTemplate = "\\n${Log}\\n--------------------------------", IncidentTypeTemplate = "-" },
            new IncidentType() { IncidentTypeId = 6, IncidentTypeShortDesc = "VS", IncidentTypeDesc = "ViewState", IncidentTypeFromServer = true, IncidentTypeSubjectLine = "ViewState probe from ${IPAddress}", IncidentTypeEmailTemplate = "Hi\\n\\nStop the intrusion from your IP address ${IPAddress}.\\nThe following IP address probe my network, probing for ${IncidentTypeDesc} vulnerabilities.\\nPlease use the following reference # in all communications: ${IncidentId}\\n\\n${Device}\\n${ServerLocation}\\n\\nIncident times:", IncidentTypeTimeTemplate = "${NetworkLogDate} ${TimeZone}", IncidentTypeThanksTemplate = "\\nThank you,\\n${FromName}\\n================", IncidentTypeLogTemplate = "\\n${Log}\\n--------------------------------", IncidentTypeTemplate = "-" },
            new IncidentType() { IncidentTypeId = 7, IncidentTypeShortDesc = "DIR", IncidentTypeDesc = "Directory traversal", IncidentTypeFromServer = true, IncidentTypeSubjectLine = "Directory traversal probe from ${IPAddress}", IncidentTypeEmailTemplate = "Hi\\n\\nStop the intrusion from your IP address ${IPAddress}.\\nThe following IP address probe my network, probing for ${IncidentTypeDesc} vulnerabilities.\\nPlease use the following reference # in all communications: ${IncidentId}\\n\\n${Device}\\n${ServerLocation}\\n\\nIncident times:", IncidentTypeTimeTemplate = "${NetworkLogDate} ${TimeZone}", IncidentTypeThanksTemplate = "\\nThank you,\\n${FromName}\\n================", IncidentTypeLogTemplate = "\\n${Log}\\n--------------------------------", IncidentTypeTemplate = "-" },
            new IncidentType() { IncidentTypeId = 8, IncidentTypeShortDesc = "DoS", IncidentTypeDesc = "Denial-of-service attack", IncidentTypeFromServer = true, IncidentTypeSubjectLine = "Denial-of-service attack from ${IPAddress}", IncidentTypeEmailTemplate = "Hi\n\nStop the intrusion from your IP address ${IPAddress}.  This is a DoS, affecting by my router.\nPlease contain the following reference # in all communications: ${IncidentId}\n\n${Device}\n${ServerLocation}\n\nIncident times:", IncidentTypeTimeTemplate = "${NetworkLogDate} ${TimeZone}", IncidentTypeThanksTemplate = "\\nThank you,\\n${FromName}\\n================", IncidentTypeLogTemplate = "\\n${Log}\\n--------------------------------", IncidentTypeTemplate = "-" }
        };
        /*
        ** Add company specific E-mail templates.
        */
        public static List<EmailTemplate> emailTemplatesFakeData = new List<EmailTemplate>() {
            new EmailTemplate()
            {
                CompanyId = companyId,
                IncidentTypeId = 8,
                FromServer = true,
                SubjectLine = "Denial-of-service attack from ${ IPAddress}",
                EmailBody = "Hi\n\nStop the intrusion from your IP address ${IPAddress}.  This is a DoS, affecting by my router.\nPlease contain the following reference # in all communications: ${IncidentId}\n\n${Device}\n${ServerLocation}\nIncident times:",
                TimeTemplate = "${NetworkLogDate} ${TimeZone}",
                ThanksTemplate = "\nThank you,\n${FromName}\n================",
                LogTemplate = "\n${Log}\n--------------------------------",
                Template = "-",
                Company = company1,
                IncidentType = incidentTypesFakeData.Find( it => it.IncidentTypeId == 8)
            }
        };
        /*
        ** Add a couple of fake network incident logs.
        */
        public static DateTime _dt = DateTime.Now.AddDays(-1);
        public static int _incTypeSql = 3;
        public static List<NetworkLog> networkLogsFakeData = new List<NetworkLog>()
        {
            new NetworkLog() { NetworkLogId = 11, ServerId = serverId, IncidentId = 3, IPAddress = "94.41.54.105", NetworkLogDate = _dt.AddMilliseconds(15), Log = "Fake log 1, Fake log 1, Fake log 1", IncidentTypeId = _incTypeSql },
            new NetworkLog() { NetworkLogId = 12, ServerId = serverId, IncidentId = 4, IPAddress = "104.42.229.49", NetworkLogDate = _dt.AddMinutes(4), Log = "Fake log 2, Fake log 2, Fake log 2", IncidentTypeId = _incTypeSql },
            new NetworkLog() { NetworkLogId = 13, ServerId = serverId, IncidentId = 5, IPAddress = "104.42.229.49", NetworkLogDate = _dt.AddMinutes(5), Log = "Fake log 3, Fake log 3, Fake log 3", IncidentTypeId = _incTypeSql },
            new NetworkLog() { NetworkLogId = 14, ServerId = serverId, IncidentId = 6, IPAddress = "54.183.209.144", NetworkLogDate = _dt.AddMinutes(10), Log = "Fake log 4, Fake log 4, Fake log 4", IncidentTypeId = _incTypeSql },
            new NetworkLog() { NetworkLogId = 15, ServerId = serverId, IncidentId = null, IPAddress = "54.183.209.144", NetworkLogDate = _dt.AddMinutes(60), Log = "Fake log 5, Fake log 5, Fake log 5", IncidentTypeId = _incTypeSql }
        };
        /*
        ** Incidents
        */
        public static Incident incident3 = new Incident()
        {
            IncidentId = 3, ServerId = serverId,
            IPAddress = "94.41.54.105", NIC_Id = "ripe.net",
            NetworkName = "UBN Network", AbuseEmailAddress = "abuse@AbuseEmailAddress.com",
            ISPTicketNumber = "",
            Mailed = false, Closed = false,Special = false,
            Notes = "", CreatedDate = DateTime.Now
        };
        public static Incident incident4 = new Incident()
        {
            IncidentId = 4,
            ServerId = serverId,
            IPAddress = "104.42.229.49",
            NIC_Id = "arin.net",
            NetworkName = "Amazon",
            AbuseEmailAddress = "abuse@Amazon.com",
            ISPTicketNumber = "",
            Mailed = false,
            Closed = false,
            Special = false,
            Notes = "",
            CreatedDate = DateTime.Now,
            Server = server1
        };
        public static Incident incident5 = new Incident()
        {
            IncidentId = 5,
            ServerId = serverId,
            IPAddress = "104.42.229.49",
            NIC_Id = "ripe.net",
            NetworkName = "UBN Network",
            AbuseEmailAddress = "abuse@AbuseEmailAddress.com",
            ISPTicketNumber = "",
            Mailed = false,
            Closed = false,
            Special = false,
            Notes = "",
            CreatedDate = DateTime.Now,
            Server = server1
        };
        public static Incident incident6 = new Incident()
        {
            IncidentId = 6,
            ServerId = serverId,
            IPAddress = "54.183.209.144",
            NIC_Id = "ripe.net",
            NetworkName = "UBN Network",
            AbuseEmailAddress = "abuse@AbuseEmailAddress.com",
            ISPTicketNumber = "",
            Mailed = false,
            Closed = false,
            Special = false,
            Notes = "",
            CreatedDate = DateTime.Now,
            Server = server1
        };
        public static List<Incident> incidentsFakeData = new List<Incident>()
        {
            incident3, incident4, incident5, incident6
        };
        /*
        ** IncidentNotes
        */
        public static IncidentNote incidentNote1 = new IncidentNote()
        {
            IncidentNoteId = 1, NoteTypeId = 1,
            Note = "Pinging 94.41.54.105 with 32 bytes of data:",
            CreatedDate = DateTime.Now
        };
        public static IncidentNote incidentNote2 = new IncidentNote()
        {
            IncidentNoteId = 2, NoteTypeId = 2,
            Note = "whois 94.41.54.105", CreatedDate = DateTime.Now
        };
        public static IncidentNote incidentNote3 = new IncidentNote()
        {
            IncidentNoteId = 3, NoteTypeId = 1,
            Note = "Pinging 104.42.229.49 with 32 bytes of data:",
            CreatedDate = DateTime.Now
        };
        public static IncidentNote incidentNote4 = new IncidentNote()
        {
            IncidentNoteId = 4, NoteTypeId = 2,
            Note = "whois 104.42.229.49", CreatedDate = DateTime.Now
        };
        public static IncidentNote incidentNote5 = new IncidentNote()
        {
            IncidentNoteId = 5, NoteTypeId = 2,
            Note = "whois 104.42.229.49",
            CreatedDate = DateTime.Now
        };
        public static IncidentNote incidentNote6 = new IncidentNote()
        {
            IncidentNoteId = 6, NoteTypeId = 2,
            Note = "whois 54.183.209.144",
            CreatedDate = DateTime.Now
        };
        public static List<IncidentNote> incidentNotesFakeData = new List<IncidentNote>()
        {
            incidentNote1, incidentNote2, // incident id 3
            incidentNote3, incidentNote4, // incident id 4
            incidentNote5, incidentNote6, // incident id 5 & 6
        };
        //
        public static List<IncidentIncidentNote> incidentIncidentNotesFakeData = new List<IncidentIncidentNote>()
        {
            new IncidentIncidentNote()
            {
                IncidentId = incident3.IncidentId,
                Incident = incident3,
                IncidentNoteId = incidentNote1.IncidentNoteId,
                IncidentNote = incidentNote1
            },
            new IncidentIncidentNote()
            {
                IncidentId = incident3.IncidentId,
                Incident = incident3,
                IncidentNoteId = incidentNote2.IncidentNoteId,
                IncidentNote = incidentNote2
            },
            new IncidentIncidentNote()
            {
                IncidentId = incident4.IncidentId,
                Incident = incident4,
                IncidentNoteId = incidentNote3.IncidentNoteId,
                IncidentNote = incidentNote3
            },
            new IncidentIncidentNote()
            {
                IncidentId = incident4.IncidentId,
                Incident = incident4,
                IncidentNoteId = incidentNote4.IncidentNoteId,
                IncidentNote = incidentNote4
            },
            new IncidentIncidentNote()
            {
                IncidentId = incident5.IncidentId,
                Incident = incident5,
                IncidentNoteId = incidentNote5.IncidentNoteId,
                IncidentNote = incidentNote5
            },
            new IncidentIncidentNote()
            {
                IncidentId = incident6.IncidentId,
                Incident = incident6,
                IncidentNoteId = incidentNote6.IncidentNoteId,
                IncidentNote = incidentNote6
            },
        };
        /*
        ** LogData
        */
        public static List<LogData> logsFakeData = new List<LogData>()
        {
            new LogData
            {
                Id = 1,
                Date = DateTime.Now,
                Application = "This application",
                Method = "The method",
                LogLevel = (byte)LoggingLevel.Info,
                Level = Enum.GetName(LoggingLevel.Info.GetType(), LoggingLevel.Info),
                UserAccount = NSG_Helpers.User_Name,
                Message = "Information Message",
                Exception = ""
            },
            new LogData
            {
                Id = 2,
                Date = DateTime.Now,
                Application = "This application",
                Method = "The method",
                LogLevel = (byte)LoggingLevel.Error,
                Level = Enum.GetName(LoggingLevel.Error.GetType(), LoggingLevel.Info),
                UserAccount = NSG_Helpers.User_Name,
                Message = "Error Message",
                Exception = ""
            },
            new LogData
            {
                Id = 3,
                Date = DateTime.Now,
                Application = "This application",
                Method = "The method",
                LogLevel = (byte)LoggingLevel.Error,
                Level = Enum.GetName(LoggingLevel.Error.GetType(), LoggingLevel.Info),
                UserAccount = NSG_Helpers.User_Name,
                Message = "Error Message",
                Exception = ""
            },
            new LogData
            {
                Id = 4,
                Date = DateTime.Now,
                Application = "This application",
                Method = "The method",
                LogLevel = (byte)LoggingLevel.Error,
                Level = Enum.GetName(LoggingLevel.Error.GetType(), LoggingLevel.Info),
                UserAccount = NSG_Helpers.User_Name,
                Message = "Error Message",
                Exception = ""
            }
        };
        //
        public static ApplicationDbContext GetMemoryDbContext(ServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            // string _name = "NetIncident4_" + Guid.NewGuid().ToString();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: "NetIncident4")
            );
            ApplicationDbContext db_context = services.BuildServiceProvider().GetService<ApplicationDbContext>();
            db_context.Database.EnsureCreated();
            // db_context.Database.OpenConnection();
            return db_context;
        }
        //
        /// <summary>
        /// Sqlite in-memory application db context.
        /// </summary>
        /// <returns>SqliteConnection</returns>
        public static SqliteConnection GetSqliteMemoryConnection( )
        {
            return new SqliteConnection("DataSource=:memory:");
        }
        //
        /// <summary>
        /// Get a relational Sqlite in-memory db instance
        /// </summary>
        /// <param name="sqliteConnection"></param>
        /// <param name="services"></param>
        /// <returns>ApplicationDbContext</returns>
        public static ApplicationDbContext GetSqliteMemoryDbContext(SqliteConnection sqliteConnection, IServiceCollection services)
        {
            // Add ASP.NET Core Identity database in memory.
            sqliteConnection = new SqliteConnection("DataSource=:memory:");
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlite(sqliteConnection)
            );
            // Add Identity using in memory database to create UserManager and RoleManager.
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
                // Object reference not set to an instance of an object. IdentityBuilderUIExtensions.GetApplicationAssembly(IdentityBuilder builder)
                //.AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddRoleManager<RoleManager<ApplicationRole>>()
                .AddDefaultTokenProviders();
            //
            ApplicationDbContext db_context = services.BuildServiceProvider()
                .GetService<ApplicationDbContext>();
            db_context.Database.OpenConnection();
            db_context.Database.EnsureCreated();
            //
            return db_context;
        }
        //
        /// <summary>
        /// Get UserManager from the ServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static UserManager<ApplicationUser> GetUserManager(ServiceCollection services)
        {
            if( services == null )
            {
                throw new ArgumentNullException(nameof(services));
            }
            return services.BuildServiceProvider().GetService<UserManager<ApplicationUser>>();
        }
        //
        /// <summary>
        /// Get RoleManager from the ServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static RoleManager<ApplicationRole> GetRoleManager(ServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            return services.BuildServiceProvider().GetService<RoleManager<ApplicationRole>>();
        }
        //
        static public async Task<bool> SeedMemoryDbContext(ApplicationDbContext db_context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            Console.WriteLine("Database defined ...");
            if (db_context == null)
            {
                throw new ArgumentNullException(nameof(db_context));
            }
            DatabaseSeeder _seeder = new DatabaseSeeder(db_context, userManager, roleManager);
            bool ret = await _seeder.Seed();
            Console.WriteLine("Database seeded ...");
            return ret;
        }
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        static public void AddLoggingService(this IServiceCollection services)
        {
            services.AddLogging(builder => builder
                //.AddConfiguration(Configuration.GetSection("Logging"))
                .AddConsole()
                .AddDebug()
            );
        }
        //
        public static ClaimsPrincipal TestPrincipal(string userName, string role)
        {
            ClaimsPrincipal _user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Email, $"{userName}@any.net"),
                new Claim(ClaimTypes.Role, role)
            }, "basic"));
            //
            return _user;
        }
        //
    }
}
