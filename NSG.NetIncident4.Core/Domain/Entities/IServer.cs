﻿// ---------------------------------------------------------------------------
using System;
//
namespace NSG.NetIncident4.Core.Domain.Entities
{
    //
    /// <summary>
    /// Minimum implementation of the server class
    /// </summary>
    public interface IServer
    {
        int ServerId { get; set; }       // The type of the key.
        string ServerShortName { get; set; } // The unique name of the server.
    }
}
// ---------------------------------------------------------------------------
