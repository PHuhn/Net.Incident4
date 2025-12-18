using NUnit.Framework;
using System;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Moq;
using MediatR;
//
using NSG.NetIncident4.Core.UI.Api;
using NSG.NetIncident4.Core.Infrastructure.Services;
using NSG.Integration.Helpers;
using Castle.Components.DictionaryAdapter;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualBasic;
using NSG.NetIncident4.Core.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.WebRequestMethods;
using System.IO.Pipelines;
using System.Runtime.Intrinsics.X86;
using System.Linq;
//
namespace NSG.NetIncident4.Core_Tests.UI.Api
{
    [TestFixture]
    public class ApiServices_UnitTests : UnitTestFixture
    {
        //
        public IConfiguration Configuration { get; set; }
        ServicesController sut;
        ServicesControllerProtected sutProtected;
        IOptions<ServicesSettings> _servicesSettings = null;
        //
        public ApiServices_UnitTests()
        {
            //
        }
        //
        [SetUp]
        public void MySetup()
        {
            Mock<IMediator> _mediator =
                new Mock<IMediator>();
            Mock<ILogger<ServicesController>> _mockLogger =
                new Mock<ILogger<ServicesController>>();
            SetupConfiguration("appsettings.json");
            _servicesSettings = GetTestConfiguration<ServicesSettings>("ServicesSettings");
            if (_servicesSettings == null)
                throw new Exception("Null services settings");
            sut = new ServicesController(_mediator.Object, _mockLogger.Object, _servicesSettings);
            sutProtected = new ServicesControllerProtected(_mediator.Object, _mockLogger.Object, _servicesSettings);
        }
        //
        [Test]
        public void ApiServicesController_PingAddress194_88_104_Test()
        {
            // RIPE: Pinging 194-88-104-81.hosted-by-worldstream.net [194.88.104.81] with 32 bytes of data:
            string _expected = @"
Pinging 194-88-104-81.hosted-by-worldstream.net [194.88.104.81] with 32 bytes of data:";
            string _actual = ApiServicesController_PingCommand("194.88.104.81");
            Assert.That(_actual.Substring(0, _expected.Length), Is.EqualTo(_expected));
        }
        //
        [Test]
        public void ApiServicesController_PingAddress64_183_Test()
        {
            // ARIN: Pinging ec2-54-183-209-144.us-west-1.compute.amazonaws.com [54.183.209.144] with 32 bytes of data:
            string _expected = @"
Pinging ec2-54-183-209-144.us-west-1.compute.amazonaws.com [54.183.209.144] with 32 bytes of data:";
            string _actual = ApiServicesController_PingCommand("54.183.209.144");
            Assert.That(_actual.Substring(0, _expected.Length), Is.EqualTo(_expected));
        }
        //
        [Test]
        public void ApiServicesController_PingAddress01_009_Test()
        {
            // APNIC: Pinging 1.9.149.170 with 32 bytes of data:
            string _expected = @"
Pinging 1.9.149.170 with 32 bytes of data:";
            string _actual = ApiServicesController_PingCommand("1.9.149.170");
            Assert.That(_actual.Substring(0, _expected.Length), Is.EqualTo(_expected));
        }
        //
        private string ApiServicesController_PingCommand(string ip)
        {
            ActionResult<string> _ping = sut.Ping(ip);
            Console.WriteLine(_ping.Result);
            Console.WriteLine(_ping.Value);
            return _ping.Value;
        }
        /*
        [Querying whois.arin.net]
        [Redirected to whois.ripe.net]
        [Querying whois.ripe.net]
        [whois.ripe.net]
        % This is the RIPE Database query service.
        % The objects are in RPSL format.
        %
        % The RIPE Database is subject to Terms and Conditions.
        % See https://apps.db.ripe.net/docs/HTML-Terms-And-Conditions

        % Note: this output has been filtered.
        %       To receive output for a database update, use the "-B" flag.

        % Information related to '46.161.62.0 - 46.161.62.255'

        % Abuse contact for '46.161.62.0 - 46.161.62.255' is 'abusemail@depo40.ru'

        inetnum:        46.161.62.0 - 46.161.62.255
        netname:        FineTransitUA
        country:        UA
        admin-c:        NA5995-RIPE
        tech-c:         NA5995-RIPE
        status:         ASSIGNED PA
        mnt-by:         FTN-MNT
        mnt-routes:     MNT-PINSUPPORT
        mnt-by:         MNT-PINSUPPORT
        created:        2022-02-11T08:22:26Z
        last-modified:  2023-02-14T05:43:13Z
        source:         RIPE

        role:           QualityNetwork_NOC
        address:        Estonia, Tallinn, Harju county, pst 5/309B
        phone:          +3726682671
        nic-hdl:        NA5995-RIPE
        mnt-by:         FTN-MNT
        created:        2018-07-04T17:00:53Z
                last-modified:  2020-12-22T14:07:12Z
        source:         RIPE # Filtered

        % Information related to '46.161.62.0/24AS26548'

        route:          46.161.62.0/24
        origin:         AS26548
        mnt-by:         MNT-PINSUPPORT
        created:        2023-01-26T04:43:10Z
        last-modified:  2023-01-26T04:43:10Z
        source:         RIPE

        % This query was served by the RIPE Database Query Service version 1.108 (BUSA)
        */
        [Test]
        public void ApiServicesController_WhoIs46_161_Test()
        {
            // RIPE: Pinging pinspb.ru [46.161.62.245] with 32 bytes of data:
            string ip = "46.161.62.245";
            ActionResult<string> _whois = sut.WhoIs(ip);
            string _actual = _whois.Value;
            Console.WriteLine($"Query ip: {ip}");
            Console.WriteLine(_actual);
            Assert.That(_actual.Contains(
                "https://rdap.arin.net/registry/ip/46.0.0.0" ), Is.True);
            //  "Abuse contact for '46.161.62.0 - 46.161.62.255' is 'abuse@finegroupservers.com'"), Is.True);
        }
        /*
        [Querying whois.arin.net]
        [whois.arin.net]

        #
        # ARIN WHOIS data and services are subject to the Terms of Use
        # available at: https://www.arin.net/resources/registry/whois/tou/
        #
        # If you see inaccuracies in the results, please report at
        # https://www.arin.net/resources/registry/whois/inaccuracy_reporting/
        #
        # Copyright 1997-2023, American Registry for Internet Numbers, Ltd.
        #

        #
        # Query terms are ambiguous.  The query is assumed to be:
        #     "n 174.79.60.55"
        #
        # Use "?" to get help.
        #

        Cox Communications Inc. CXA (NET-174-64-0-0-1) 174.64.0.0 - 174.79.255.255
        Cox Communications NETBLK-PH-CBS-174-79-32-0 (NET-174-79-32-0-1) 174.79.32.0 - 174.79.63.255

        #
        # ARIN WHOIS data and services are subject to the Terms of Use
        # available at: https://www.arin.net/resources/registry/whois/tou/
        #
        # If you see inaccuracies in the results, please report at
        # https://www.arin.net/resources/registry/whois/inaccuracy_reporting/
        #
        # Copyright 1997-2023, American Registry for Internet Numbers, Ltd.
        #
        */
        // Then link query returns the following:
        /*
        [Querying whois.arin.net]
        [whois.arin.net]
    
        #
        # ARIN WHOIS data and services are subject to the Terms of Use
        # available at: https://www.arin.net/resources/registry/whois/tou/
        #
        # If you see inaccuracies in the results, please report at
        # https://www.arin.net/resources/registry/whois/inaccuracy_reporting/
        #
        # Copyright 1997-2023, American Registry for Internet Numbers, Ltd.
        #
    
    
        #
        # Query terms are ambiguous.  The query is assumed to be:
        #     "n ! NET-174-64-0-0-1"
        #
        # Use "?" to get help.
        #
    
        NetRange:       174.64.0.0 - 174.79.255.255
        CIDR:           174.64.0.0/12
        NetName:        CXA
        NetHandle:      NET-174-64-0-0-1
        Parent:         NET174 (NET-174-0-0-0-0)
        NetType:        Direct Allocation
        OriginAS:       
        Organization:   Cox Communications Inc. (CXA)
        RegDate:        2009-02-19
        Updated:        2012-03-02
        Ref:            https://rdap.arin.net/registry/ip/174.64.0.0
    
    
    
        OrgName:        Cox Communications Inc.
        OrgId:          CXA
        Address:        1400 Lake Hearn Dr.
        City:           Atlanta
        StateProv:      GA
        PostalCode:     30319
        Country:        US
        RegDate:        
        Updated:        2023-02-14
        Comment:        For legal requests/assistance please use the
    
        Comment:        following contact information:
    
        Comment:        Cox Subpoena Info: https://www.cox.com/aboutus/policies/law-enforcement-and-subpoenas-information.html
        Ref:            https://rdap.arin.net/registry/entity/CXA
    
    
        OrgTechHandle: RUIZC31-ARIN
        OrgTechName:   Ruiz, Carlos 
        OrgTechPhone:  +1-480-318-5430 
        OrgTechEmail:  carlos.ruiz1@cox.com
        OrgTechRef:    https://rdap.arin.net/registry/entity/RUIZC31-ARIN
    
        OrgRoutingHandle: GARCI1592-ARIN
        OrgRoutingName:   Garcia, Jacob 
        OrgRoutingPhone:  +1-404-269-4416 
        OrgRoutingEmail:  jacob.garcia@cox.com
        OrgRoutingRef:    https://rdap.arin.net/registry/entity/GARCI1592-ARIN
    
        OrgTechHandle: MEROL3-ARIN
        OrgTechName:   Merola, Cari 
        OrgTechPhone:  +1-404-269-4416 
        OrgTechEmail:  cari.merola@cox.com
        OrgTechRef:    https://rdap.arin.net/registry/entity/MEROL3-ARIN
    
        OrgTechHandle: BERUB3-ARIN
        OrgTechName:   Berube, Tori 
        OrgTechPhone:  +1-404-269-4416 
        OrgTechEmail:  tori.berube@cox.com
        OrgTechRef:    https://rdap.arin.net/registry/entity/BERUB3-ARIN
    
        OrgTechHandle: GARCI1592-ARIN
        OrgTechName:   Garcia, Jacob 
        OrgTechPhone:  +1-404-269-4416 
        OrgTechEmail:  jacob.garcia@cox.com
        OrgTechRef:    https://rdap.arin.net/registry/entity/GARCI1592-ARIN
    
        OrgTechHandle: GOODW243-ARIN
        OrgTechName:   Goodwin, Mark 
        OrgTechPhone:  +1-404-269-8267 
        OrgTechEmail:  mark.goodwin@cox.com
        OrgTechRef:    https://rdap.arin.net/registry/entity/GOODW243-ARIN
    
        OrgAbuseHandle: IC146-ARIN
        OrgAbuseName:   Cox Communications Inc
        OrgAbusePhone:  +1-866-272-5111 
        OrgAbuseEmail:  abuse@cox.com
        OrgAbuseRef:    https://rdap.arin.net/registry/entity/IC146-ARIN
    
        #
        # ARIN WHOIS data and services are subject to the Terms of Use
        # available at: https://www.arin.net/resources/registry/whois/tou/
        #
        # If you see inaccuracies in the results, please report at
        # https://www.arin.net/resources/registry/whois/inaccuracy_reporting/
        #
        # Copyright 1997-2023, American Registry for Internet Numbers, Ltd.
        #
        */
        [Test]
        public void ApiServicesController_WhoIs174_79_Test()
        {
            // RIPE: Pinging pinspb.ru [46.161.62.245] with 32 bytes of data:
            string ip = "174.79.60.55";
            ActionResult<string> _whois = sut.WhoIs(ip);
            Console.WriteLine(_whois.Result);
            string _actual = _whois.Value;
            Console.WriteLine($"Query ip: {ip}");
            Console.WriteLine(_actual);
            Assert.That(_actual.Contains(ip.Substring(0, 6)), Is.True);
            Assert.That(_actual.Contains(
                "OrgAbuseEmail:  abuse@cox.com"), Is.True);
        }
        //
        // WhoIs Tests
        //
        [Test]
        public void ApiServicesController_WhoIsLink_Link_Test()
        {
            string _data = @"[Querying whois.arin.net]
[whois.arin.net]

#
# ARIN WHOIS data and services are subject to the Terms of Use
# available at: https://www.arin.net/whois_tou.html
#
# If you see inaccuracies in the results, please report at
# https://www.arin.net/public/whoisinaccuracy/index.xhtml
#
#
# The following results may also be obtained via:
# https://whois.arin.net/rest/nets;q=174.79.60.55?showDetails=true&showARIN=false&showNonArinTopLevelNet=false&ext=netref2
#

Cox Communications NETBLK-PH-CBS-174-79-32-0 (NET-174-79-32-0-1) 174.79.32.0 - 174.79.63.255
Cox Communications Inc. CXA (NET-174-64-0-0-1) 174.64.0.0 - 174.79.255.255

#
# ARIN WHOIS data and services are subject to the Terms of Use
# available at: https://www.arin.net/whois_tou.html
#
# If you see inaccuracies in the results, please report at
# https://www.arin.net/public/whoisinaccuracy/index.xhtml
#
";
            string _link = sutProtected.WhoIsLinkProtected(_data);
            System.Diagnostics.Debug.WriteLine(_link);
            Assert.That(_link, Is.Not.Empty);
        }
        //
        [Test]
        public void ApiServicesController_WhoIsLink_NoLink1_Test()
        {
            string _data = @"[Querying whois.arin.net]
[whois.arin.net]

#
# ARIN WHOIS data and services are subject to the Terms of Use
# available at: https://www.arin.net/whois_tou.html
#
# If you see inaccuracies in the results, please report at
# https://www.arin.net/public/whoisinaccuracy/index.xhtml
#

NetRange:       174.79.32.0 - 174.79.63.255
CIDR:           174.79.32.0/19
NetName:        NETBLK-PH-CBS-174-79-32-0
NetHandle:      NET-174-79-32-0-1
Parent:         CXA (NET-174-64-0-0-1)
NetType:        Reassigned
OriginAS:       
Customer:       Cox Communications (C02309424)
RegDate:        2009-09-04
Updated:        2011-03-24
Ref:            https://whois.arin.net/rest/net/NET-174-79-32-0-1


CustName:       Cox Communications
Address:        1400 Lake Hearn Drive
City:           Atlanta
StateProv:      GA
PostalCode:     30319
Country:        US
RegDate:        2009-09-04
Updated:        2011-03-24
Ref:            https://whois.arin.net/rest/customer/C02309424

OrgAbuseHandle: IC146-ARIN
OrgAbuseName:   Cox Communications Inc
OrgAbusePhone:  +1-404-269-7626 
OrgAbuseEmail:  abuse@cox.net
OrgAbuseRef:    https://whois.arin.net/rest/poc/IC146-ARIN

OrgTechHandle: BAABO-ARIN
OrgTechName:   BA, Aboubakr 
OrgTechPhone:  +1-404-269-4416 
OrgTechEmail:  abuse@cox.net
OrgTechRef:    https://whois.arin.net/rest/poc/BAABO-ARIN

OrgTechHandle: ADA131-ARIN
OrgTechName:   Anderson, Alvin Demond
OrgTechPhone:  +1-404-269-4416 
OrgTechEmail:  alvin.anderson@cox.com
OrgTechRef:    https://whois.arin.net/rest/poc/ADA131-ARIN

OrgTechHandle: BERUB3-ARIN
OrgTechName:   Berube, Tori 
OrgTechPhone:  +1-404-269-4416 
OrgTechEmail:  tori.berube@cox.com
OrgTechRef:    https://whois.arin.net/rest/poc/BERUB3-ARIN

OrgTechHandle: NIA16-ARIN
OrgTechName:   National IP Administrator
OrgTechPhone:  +1-404-269-4416 
OrgTechEmail:  tiffany.coleman@cox.com
OrgTechRef:    https://whois.arin.net/rest/poc/NIA16-ARIN

OrgTechHandle: RWA196-ARIN
OrgTechName:   Waldron, Roderick 
OrgTechPhone:  +1-404-269-7626 
OrgTechEmail:  abuse@cox.net
OrgTechRef:    https://whois.arin.net/rest/poc/RWA196-ARIN

OrgTechHandle: MEROL3-ARIN
OrgTechName:   Merola, Cari 
OrgTechPhone:  +1-404-269-4416 
OrgTechEmail:  cari.merola@cox.com
OrgTechRef:    https://whois.arin.net/rest/poc/MEROL3-ARIN


#
# ARIN WHOIS data and services are subject to the Terms of Use
# available at: https://www.arin.net/whois_tou.html
#
# If you see inaccuracies in the results, please report at
# https://www.arin.net/public/whoisinaccuracy/index.xhtml
#
";
            string _link = sutProtected.WhoIsLinkProtected(_data);
            System.Diagnostics.Debug.WriteLine(_link);
            Assert.That(_link, Is.EqualTo(""));
        }
        //
        [Test]
        public void ApiServicesController_WhoIsLink_NoLink2_Test()
        {
            string _data = @"[Querying whois.arin.net]
[Redirected to rwhois.singlehop.net:4321]
[Querying rwhois.singlehop.net]
[rwhois.singlehop.net]
%rwhois V-1.5:003eff:00 rwhois.singlehop.com (by Network Solutions, Inc. V-1.5.9.5)
network:Class-Name:network
network:ID:ORG-SINGL-8.184-154-139-0/26
network:Auth-Area:184.154.0.0/16
network:IP-Network:184.154.139.0/26
network:Organization:Innovative Business Services (dba SiteLock)
network:Street-Address:8125 N 86th Place
network:City:Scottsdale
network:State:AZ
network:Postal-Code:85258
network:Country-Code:US
network:Tech-Contact;I:NETWO1546-ARIN
network:Admin-Contact;I:NETWO1546-ARIN
network:Abuse-Contact;I:ABUSE2492-ARIN
network:Created:20150331
network:Updated:20150331

%referral rwhois://root.rwhois.net:4321/auth-area=.
%ok

";
            string _link = sutProtected.WhoIsLinkProtected(_data);
            System.Diagnostics.Debug.WriteLine(_link);
            Assert.That(_link, Is.EqualTo(""));
        }
        // 159.192.106.207
        //
        [Test]
        public void ApiServicesController_WhoIs_159_192_106_207_Test()
        {
            string ip = "159.192.106.207";
            ActionResult<string> _whois = sut.WhoIs(ip);
            Console.WriteLine(_whois.Result);
            string _actual = _whois.Value;
            Console.WriteLine($"Query ip: {ip}");
            Console.WriteLine(_actual);
            Assert.That(_actual.Contains(ip.Substring(0, 6)), Is.True);
            Assert.That(_actual.Contains(
                "Abuse contact for '159.192.0.0 - 159.192.127.255' is 'pitoon.p@ntplc.co.th'"), Is.True);
        }
        //
    }
    public class ServicesControllerProtected : ServicesController
    {
        public ServicesControllerProtected(IMediator mediator, ILogger<ServicesController> logger, IOptions<ServicesSettings> servicesSettings) : base(mediator, logger, servicesSettings)
        {
        }
        public string WhoIsLinkProtected(string whoisData)
        {
            return base.WhoIsLink( whoisData );
        }
    }
}
