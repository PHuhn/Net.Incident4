using NUnit.Framework;
using System;
using System.IO;
using System.Security.Principal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Moq;
//
using NSG.Integration.Helpers;
using NSG.NetIncident4.Core.Infrastructure.Common;
using NSG.NetIncident4.Core.Infrastructure.Authentication;
//
namespace NSG.NetIncident4.Core_Tests.Infrastructure
{
    [TestFixture]
    public class appSettings_Tests : UnitTestFixture
    {
        //
        [SetUp]
        public void Setup()
        {
        }
        //
        [Test]
        public void IdentitySettings_Test()
        {
            // when
            IdentitySettings identitySettings = configuration.GetSection(
                "IdentitySettings").Get<IdentitySettings>();
            // then
            Assert.IsNotNull(identitySettings);
            Assert.GreaterOrEqual(identitySettings.PasswordMinLength, 6);
        }
        //
        [Test]
        public void AuthSettings_Test()
        {
            // when
            AuthSettings authSettings = configuration.GetSection(
                "AuthSettings").Get<AuthSettings>();
            // then
            Assert.IsNotNull(authSettings);
            Assert.GreaterOrEqual(authSettings.CookieExpirationHours, 1);
        }
        //
        [Test]
        public void AuthenticationGoogleSettings_Test()
        {
            // when
            IConfigurationSection googleAuthSection = configuration.GetSection("Authentication:Google");
            // then
            Assert.IsNotNull(googleAuthSection);
            string actual = googleAuthSection["ClientId"];
            Assert.IsNotNull(actual);
        }
        //
    }
}
//
