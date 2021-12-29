using System;
using System.Configuration;
//
using NSG.Library.Helpers;
//
namespace NSG.NetIncident4.Core.Infrastructure.Common
{
    public static class Constants
    {
        //
        // Base roles are as follows (see Aplication.cs):
        //  Public          has access to basic grid of incidents for given company/server
        //  User            has create and edit of incidents for given company/server
        //  CompanyAdmin    has create, edit and delete of incidents for given company/server
        //  Admin           administrator
        public static readonly string NotAuthenticated = "- Not Authenticated -";
        public static readonly string adminRole = "admin";
        public static readonly string companyadminRole = "companyadmin";
        public static readonly string userRole = "user";
        //
    }
}