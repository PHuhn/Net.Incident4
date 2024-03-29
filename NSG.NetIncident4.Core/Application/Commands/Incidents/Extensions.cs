﻿// ---------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
//
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Persistence;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
//
namespace NSG.NetIncident4.Core.Application.Commands.Incidents
{
    //
    /// <summary>
    /// Extension method.
    /// </summary>
    public static partial class Extensions
    {
        //
        /// <summary>
        /// Extension method that translates from Incident to etworkIncidentData.
        /// </summary>
        /// <param name="entity">The Incident entity class.</param>
        /// <returns>'NetworkIncidentData' or Incident list query.</returns>
        public static NetworkIncidentData ToNetworkIncidentData(this Incident entity)
        {
            return new NetworkIncidentData
            {
                IncidentId = entity.IncidentId,
                ServerId = entity.ServerId,
                IPAddress = entity.IPAddress,
                NIC = entity.NIC_Id,
                NetworkName = entity.NetworkName,
                AbuseEmailAddress = entity.AbuseEmailAddress,
                ISPTicketNumber = entity.ISPTicketNumber,
                Mailed = entity.Mailed,
                Closed = entity.Closed,
                Special = entity.Special,
                Notes = entity.Notes,
                CreatedDate = entity.CreatedDate,
                IsChanged = false
            };
        }
        //
        /// <summary>
        /// Extension method that translates from Incident to IncidentListQuery.
        /// </summary>
        /// <param name="entity">The Incident entity class.</param>
        /// <returns>'IncidentListQuery' or Incident list query.</returns>
        public static IncidentListQuery ToIncidentListQuery(this Incident entity)
        {
            return new IncidentListQuery
            {
                IncidentId = entity.IncidentId,
                ServerId = entity.ServerId,
                IPAddress = entity.IPAddress,
                NIC = entity.NIC_Id,
                NetworkName = entity.NetworkName,
                AbuseEmailAddress = entity.AbuseEmailAddress,
                ISPTicketNumber = entity.ISPTicketNumber,
                Mailed = entity.Mailed,
                Closed = entity.Closed,
                Special = entity.Special,
                Notes = entity.Notes,
                CreatedDate = entity.CreatedDate,
                IsChanged = false
            };
        }
        //
        /// <summary>
        /// Create a 'to string'.
        /// </summary>
        public static string IncidentToString(this Incident entity)
        {
            //
            StringBuilder _return = new StringBuilder("record:[");
            _return.AppendFormat("IncidentId: {0}, ", entity.IncidentId.ToString());
            _return.AppendFormat("ServerId: {0}, ", entity.ServerId.ToString());
            _return.AppendFormat("IPAddress: {0}, ", entity.IPAddress);
            _return.AppendFormat("NIC_Id: {0}, ", entity.NIC_Id);
            _return.AppendFormat("NetworkName: {0}, ", entity.NetworkName);
            _return.AppendFormat("AbuseEmailAddress: {0}, ", entity.AbuseEmailAddress);
            _return.AppendFormat("ISPTicketNumber: {0}, ", entity.ISPTicketNumber);
            _return.AppendFormat("Mailed: {0}, ", entity.Mailed.ToString());
            _return.AppendFormat("Closed: {0}, ", entity.Closed.ToString());
            _return.AppendFormat("Special: {0}, ", entity.Special.ToString());
            _return.AppendFormat("Notes: {0}, ", entity.Notes);
            _return.AppendFormat("CreatedDate: {0}]", entity.CreatedDate.ToString());
            return _return.ToString();
        }
        //
        public static IncidentNoteData ToIncidentNoteData(this IncidentNote entity)
        {
            return new IncidentNoteData()
            {
                IncidentNoteId = entity.IncidentNoteId,
                NoteTypeId = entity.NoteTypeId,
                NoteTypeShortDesc = entity.NoteType.NoteTypeShortDesc,
                Note = entity.Note,
                CreatedDate = entity.CreatedDate,
                IsChanged = false
            };
        }
        //
        /// <summary>
        /// Extension method that translates from NetworkLog to NetworkLogListQuery.
        /// </summary>
        /// <param name="entity">The NetworkLog entity class.</param>
        /// <returns>'NetworkLogListQuery' or NetworkLog list query.</returns>
        public static NetworkLogData ToNetworkLogData(this NetworkLog entity, long incidentId)
        {
            return new NetworkLogData
            {
                NetworkLogId = entity.NetworkLogId,
                ServerId = entity.ServerId,
                IncidentId = entity.IncidentId,
                IPAddress = entity.IPAddress,
                NetworkLogDate = entity.NetworkLogDate,
                Log = entity.Log,
                IncidentTypeId = entity.IncidentTypeId,
                IsChanged = false,
                Selected = (incidentId == entity.IncidentId ? true : false)
            };
        }
        //
        //  GetNoteTypes
        //  GetNICs
        //  GetIncidentTypes
        //
        #region "Get SelectItems"
        //
        /// <summary>
        /// Create the dropdown data
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<SelectItemExtra> GetNoteTypes(ApplicationDbContext context)
        {
            return
                context.NoteTypes
                    .Select(_nt => new SelectItemExtra(
                        _nt.NoteTypeId.ToString(),
                        _nt.NoteTypeShortDesc,
                        _nt.NoteTypeClientScript
                    )).ToList();
        }
        //
        /// <summary>
        /// Create the dropdown data
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<SelectItem> GetNICs(ApplicationDbContext context)
        {
            return
                context.NICs
                .Select(_n => new SelectItem
                {
                    value = _n.NIC_Id,
                    label = _n.NIC_Id
                }).ToList();
        }
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<SelectItem> GetIncidentTypes(ApplicationDbContext context)
        {
            return
                context.IncidentTypes
                .Where(_it => _it.IncidentTypeId > 0)
                .Select(_it => new SelectItem
                {
                    value = _it.IncidentTypeId.ToString(),
                    label = _it.IncidentTypeShortDesc
                }).ToList();
        }
        //
        #endregion
        //
        /// <summary>
        /// Get an entity record via the primary key.
        /// </summary>
        /// <param name="incidentId">long key</param>
        /// <returns>Returns a Incident entity record.</returns>
        public static Task<Server> GetServerByKey(ApplicationDbContext context, int serverId)
        {
            return context.Servers.SingleOrDefaultAsync(r => r.ServerId == serverId);
        }
        //
        /// <summary>
        /// Return a list with all rows of default IncidentType and
        /// overridden specific EmailTemplate for a company.
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public static List<IncidentTypeData> GetCompanyIncidentType(ApplicationDbContext context, int companyId)
        {
            string _companyNumber = " (" + companyId.ToString() + ")";
            return
                (from _i in context.IncidentTypes
                 join _r in context.EmailTemplates
                 on new { _i.IncidentTypeId, CompanyId = companyId } equals new { _r.IncidentTypeId, _r.CompanyId } into g
                 from _et in g.DefaultIfEmpty()
                 select new IncidentTypeData()
                 {
                     IncidentTypeId = _i.IncidentTypeId,
                     IncidentTypeShortDesc = _et == null ? _i.IncidentTypeShortDesc : _i.IncidentTypeShortDesc + _companyNumber,
                     IncidentTypeDesc = _et == null ? _i.IncidentTypeDesc : _i.IncidentTypeDesc + _companyNumber,
                     IncidentTypeFromServer = _et == null ? _i.IncidentTypeFromServer : _et.FromServer,
                     IncidentTypeSubjectLine = _et == null ? _i.IncidentTypeSubjectLine : _et.SubjectLine,
                     IncidentTypeEmailTemplate = _et == null ? _i.IncidentTypeEmailTemplate : _et.EmailBody,
                     IncidentTypeTimeTemplate = _et == null ? _i.IncidentTypeTimeTemplate : _et.TimeTemplate,
                     IncidentTypeThanksTemplate = _et == null ? _i.IncidentTypeThanksTemplate : _et.ThanksTemplate,
                     IncidentTypeLogTemplate = _et == null ? _i.IncidentTypeLogTemplate : _et.LogTemplate,
                     IncidentTypeTemplate = _et == null ? _i.IncidentTypeTemplate : _et.Template
                 }).ToList();
        }
        //
        /// <summary>
        /// Return an IQueryable of NetworkLog
        /// </summary>
        /// <returns></returns>
        private static IQueryable<NetworkLogData> NetworkLogDataQueryable(ApplicationDbContext context)
        {
            return
                from _r in context.NetworkLogs
                select new NetworkLogData()
                {
                    NetworkLogId = _r.NetworkLogId,
                    ServerId = _r.ServerId,
                    IPAddress = _r.IPAddress,
                    NetworkLogDate = _r.NetworkLogDate,
                    Log = _r.Log,
                    IncidentTypeId = _r.IncidentTypeId,
                    IncidentTypeShortDesc = _r.IncidentType.IncidentTypeShortDesc,
                    IncidentId = (_r.IncidentId.HasValue ? _r.IncidentId.Value : 0),
                    Selected = (_r.IncidentId.HasValue ? (_r.IncidentId.Value > 0 ? true : false) : false),
                    IsChanged = false
                };
        }
        //
        /// <summary>
        /// Return a list with select rows of NetworkLog.
        /// * only one server
        /// * if not mailed all unassigned network logs
        /// </summary>
        /// <param name="context"></param>
        /// <param name="incidentId"></param>
        /// <param name="serverId"></param>
        /// <param name="mailed"></param>
        /// <returns></returns>
        public static async Task<List<NetworkLogData>> GetNetworkLogData( ApplicationDbContext context, long incidentId, int serverId, bool mailed )
        {
            long?[] incidents;
            if (mailed)
                incidents = new long?[] { incidentId };
            else
                incidents = new long?[] { incidentId, (long)0, null };
            //
            return await NetworkLogDataQueryable(context)
                .OrderByDescending(_r => _r.Selected).ThenBy(_r2 => _r2.NetworkLogDate)
                .Where(_r => _r.ServerId == serverId
                   && incidents.Contains(_r.IncidentId)).ToListAsync();
        }
        //
    }
    //
}
// ---------------------------------------------------------------------------
