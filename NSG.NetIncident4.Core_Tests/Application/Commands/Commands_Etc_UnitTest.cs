using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
//
using NSG.Integration.Helpers;
//
namespace NSG.NetIncident4.Core_Tests.Application.Commands
{
    [TestFixture]
    public class Commands_Etc_UnitTest
    {
        [SetUp]
        public void Setup()
        {
        }
        //
        [Test]
        public void ReplaceCRToN_Test()
        {
            // given
            string str = $"A{Environment.NewLine}b<br />c";
            // when
            string ret = str.ReplaceCRToN();
            // then
            Assert.That(ret, Is.EqualTo(@"A\nb\nc"));
        }
        //
        [Test]
        public void ReplaceToBR_Test()
        {
            // given
            string str = @$"A{Environment.NewLine}b\nc";
            // when
            string ret = str.ReplaceToBR();
            // then
            Assert.That(ret, Is.EqualTo(@"A<br />b<br />c"));
        }
        //
        //[Test]
        //public async Task Test1()
        //{
        //    Console.WriteLine("Setup");
        //    //
        //    Assert.Pass();
        //}
        //
    }
}