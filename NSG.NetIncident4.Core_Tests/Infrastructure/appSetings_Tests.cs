// ===========================================================================
// File: appSettings_Tests.cs
using NUnit.Framework;
using System;
using Microsoft.Extensions.Configuration;
//
using Moq;
//
using NSG.Integration.Helpers;
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
            Assert.That(identitySettings, Is.Not.Null);
            Assert.That(identitySettings.PasswordMinLength, Is.GreaterThanOrEqualTo(6));
        }
        //
        [Test]
        public void AuthSettings_Test()
        {
            // when
            AuthSettings authSettings = configuration.GetSection(
                "AuthSettings").Get<AuthSettings>();
            // then
            Assert.That(authSettings, Is.Not.Null);
            Assert.That(authSettings.CookieExpirationHours, Is.GreaterThanOrEqualTo(1));
        }
        //
        [Test]
        public void AuthenticationGoogleSettings_Test()
        {
            // when
            IConfigurationSection googleAuthSection = configuration.GetSection("Authentication:Google");
            // then
            Assert.That(googleAuthSection, Is.Not.Null);
            string actual = googleAuthSection["ClientId"];
            Assert.That(actual, Is.Not.Null);
        }
        //
    }
}
// ===========================================================================
