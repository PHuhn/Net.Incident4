// ===========================================================================
using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using NSG.NetIncident4.Core.Domain.Entities;
//
namespace NSG.NetIncident4.Core.Persistence
{
    public partial class ApplicationDbContext : IdentityDbContext<
            ApplicationUser, // TUser
            ApplicationRole, // TRole
            string, // TKey
            IdentityUserClaim<string>, // TUserClaim
            ApplicationUserRole, // TUserRole,
            IdentityUserLogin<string>, // TUserLogin
            IdentityRoleClaim<string>, // TRoleClaim
            IdentityUserToken<string> // TUserToken
        >
    {
        //
        /// <summary>
        /// Roll-back transaction changes.
        /// </summary>
        public void RollBackChanges()
        {
            var _changedEntries = this.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();
            //
            foreach (var _entry in _changedEntries)
            {
                switch (_entry.State)
                {
                    case EntityState.Modified:
                        _entry.CurrentValues.SetValues(_entry.OriginalValues);
                        _entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        _entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        _entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }
        //
        /// <summary>
        /// This should be called before entity frameworks SaveChanges.
        /// Loop through all dirty data and write them to audit.  
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="PageName"></param>
        public void TrackChanges(string userName, string pageName)
        {
            //
            string _usrName = userName;
            if (string.IsNullOrEmpty(userName))
            {
                _usrName = "-unknown-";
            }
            //
            string _prgName = pageName;
            if (string.IsNullOrEmpty( pageName ))
            {
                _prgName = "-unknown-";
            }
            //
            string _tblName = "";
            string _keyName = "";
            string _keyValue = "";
            string _key = "";
            string _updType = "";
            string _dataBefore = "";
            string _dataAfter = "";
            int _count = 0;
            var _changedEntries = this.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();
            foreach (var _item in _changedEntries)
            {
                var _entity = _item.Entity;
                _tblName = _item.Metadata.DisplayName();
                _keyName = "";
                _keyValue = "";
                _key = "";
                _dataBefore = "";
                _dataAfter = "";
                //
                try
                {
                    IReadOnlyList<IProperty> _keyProps = _item.Metadata.FindPrimaryKey().Properties;
                    if (_keyProps != null)
                    {
                        int _keyCount = _keyProps.Count();
                        for (int i = 0; i < _keyCount; i++)
                        {
                            _keyName = _keyProps[i].Name.ToString();
                            _keyValue = _keyProps[i].GetGetter().GetClrValue(_entity).ToString();
                            _key += _keyName + ":" + _keyValue + (i == _keyCount - 1 ? "" : "|");
                        }
                    }
                }
                catch (Exception _ex)
                {
                    System.Diagnostics.Debug.WriteLine(_ex.ToString());
                }
                //
                if (_item.State == EntityState.Added)
                {
                    _updType = "I";
                    _count = _item.CurrentValues.Properties.Count;
                    for (int _i = 0; _i < _count; _i++)
                    {
                        var _propName = _item.CurrentValues.Properties.ElementAt(_i).Name;
                        var _currentValue = _item.CurrentValues[_propName];
                        _dataAfter += _propName + ":" + _currentValue.ToString().Trim() + (_count == _i ? "" : "|");
                    }
                }
                if (_item.State == EntityState.Modified)
                {
                    _updType = "U";
                    _count = _item.CurrentValues.Properties.Count;
                    for (int _i = 0; _i < _count; _i++)
                    {
                        var _propName = _item.CurrentValues.Properties.ElementAt(_i).Name;
                        var _originalValue = _item.OriginalValues[_propName];
                        var _currentValue = _item.CurrentValues[_propName];
                        _dataBefore += _propName + ":" + _originalValue.ToString().Trim() + (_count == _i ? "" : "|");
                        if (!_currentValue.Equals(_originalValue))
                        {
                            _dataAfter += _propName + ":" + _currentValue.ToString().Trim() + (_count == _i ? "" : "|");
                        }
                    }
                }
                if (_item.State == EntityState.Deleted)
                {
                    _updType = "D";
                    _count = _item.OriginalValues.Properties.Count;
                    for (int _i = 0; _i < _count; _i++)
                    {
                        var _propName = _item.CurrentValues.Properties.ElementAt(_i).Name;
                        var _originalValue = _item.OriginalValues[_propName];
                        _dataBefore += _propName + ":" + _originalValue.ToString().Trim() + (_count == _i ? "" : "|");
                    }
                }
                Audit _audit = new Audit();
                _audit.ChangeDate = System.DateTime.Now;
                _audit.UserName = userName;
                _audit.Program = _prgName.Substring(0, Math.Min(_prgName.Length, 256));
                _audit.TableName = _tblName.Substring(0, Math.Min(_tblName.Length, 256));
                _audit.UpdateType = _updType;
                _audit.Keys = _key.Substring(0, Math.Min(_keyName.Length, 512));
                _audit.Before = _dataBefore.Substring(0, Math.Min(_dataBefore.Length, 4000));
                _audit.After = _dataAfter.Substring(0, Math.Min(_dataAfter.Length, 4000));
                // this.Audit.Add(_audit);
            }
            //
        }
        //
        public IDictionary<string, object> GetKeys(ApplicationDbContext ctx, EntityEntry entity)
        {
            if (ctx == null)
            {
                throw new ArgumentNullException(nameof(ctx));
            }
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            var primaryKey = entity.Metadata.FindPrimaryKey();
            if( primaryKey != null )
            {
                var keys = primaryKey.Properties.ToDictionary(x => x.Name, x => x.PropertyInfo.GetValue(entity));
                return keys;
            }
            else
            {
                return new Dictionary<string, object>();
            }
        }
        //
        // ---------------------------------------------------------------------------
        // Handle better description for DbUpdateException 
        //
        public Exception HandleDbUpdateException(DbUpdateException upExc)
        {
            string message = HandleDbUpdateExceptionString(upExc);
            return new Exception(message, upExc);
        }
        public Exception HandleDbUpdateException(DbUpdateException upExc, string newExceptionMessage)
        {
            return new Exception(newExceptionMessage, upExc);
        }
        //
        // Handle better description for DbUpdateException output as a string
        //
        public string HandleDbUpdateExceptionString(DbUpdateException upExc)
        {
            //
            var builder = new StringBuilder("DbUpdateException: ");
            try
            {
                foreach (var entityEntry in upExc.Entries)
                {
                    string _entityName = entityEntry.Entity.GetType().Name;
                    builder.AppendFormat("Problem updating entity: {0}. ", _entityName);
                    bool _reported = false;
                    switch (_entityName)
                    {
                        case "ApplicationUser":
                            if (entityEntry.State == EntityState.Added)
                            {
                                if (Users.Any(a => a.UserName == ((ApplicationUser)entityEntry.Entity).UserName))
                                {
                                    builder.AppendFormat($"Duplicate Username: {((ApplicationUser)entityEntry.Entity).UserName}, ");
                                    _reported = true;
                                }
                                if (Users.Any(a => a.Email == ((ApplicationUser)entityEntry.Entity).Email))
                                {
                                    builder.AppendFormat($"Duplicate Email: {((ApplicationUser)entityEntry.Entity).Email}, ");
                                    _reported = true;
                                }
                                if (Users.Any(a => a.FullName == ((ApplicationUser)entityEntry.Entity).FullName))
                                {
                                    builder.AppendFormat($"Duplicate Full Name: {((ApplicationUser)entityEntry.Entity).FullName}, ");
                                    _reported = true;
                                }
                            }
                            if (entityEntry.State == EntityState.Modified)
                            {
                                if (Users.Any(a =>
                                    (a.UserName == ((ApplicationUser)entityEntry.Entity).UserName
                                    || a.Email == ((ApplicationUser)entityEntry.Entity).Email
                                    || a.FullName == ((ApplicationUser)entityEntry.Entity).FullName)
                                    && a.Id != ((ApplicationUser)entityEntry.Entity).Id
                                    ))
                                {
                                    builder.AppendFormat("Duplicate Username: {0}, and/or Email: {1}, and/or Name: {2}, ",
                                        ((ApplicationUser)entityEntry.Entity).UserName,
                                        ((ApplicationUser)entityEntry.Entity).Email,
                                        ((ApplicationUser)entityEntry.Entity).FullName
                                    );
                                    _reported = true;
                                }
                            }
                            if (!_reported)
                            {
                                builder.AppendFormat("{0}. ", upExc.GetBaseException().Message);
                            }
                            break;
                        case "Company":
                            if (entityEntry.State == EntityState.Added || entityEntry.State == EntityState.Modified)
                            {
                                if (Companies.Any(a =>
                                    a.CompanyShortName == ((Company)entityEntry.Entity).CompanyShortName
                                    || a.CompanyName == ((Company)entityEntry.Entity).CompanyName
                                    ))
                                {
                                    builder.AppendFormat("Duplicate Short: {0} and/or Discription: {1}. ",
                                        ((Company)entityEntry.Entity).CompanyShortName,
                                        ((Company)entityEntry.Entity).CompanyName
                                    );
                                    _reported = true;
                                }
                            }
                            if (!_reported)
                            {
                                builder.AppendFormat("{0}. ", upExc.GetBaseException().Message);
                            }
                            break;
                        case "Server":
                            if (entityEntry.State == EntityState.Added || entityEntry.State == EntityState.Modified)
                            {
                                if (Servers.Any(a =>
                                    a.ServerShortName == ((Server)entityEntry.Entity).ServerShortName
                                    || a.ServerName == ((Server)entityEntry.Entity).ServerName
                                    ))
                                {
                                    builder.AppendFormat("Duplicate Short: {0} and/or Name: {1}. ",
                                        ((Server)entityEntry.Entity).ServerShortName,
                                        ((Server)entityEntry.Entity).ServerName
                                    );
                                    _reported = true;
                                }
                            }
                            if (!_reported)
                            {
                                builder.AppendFormat("{0}. ", upExc.GetBaseException().Message);
                            }
                            break;
                        case "IncidentType":
                            if (entityEntry.State == EntityState.Added || entityEntry.State == EntityState.Modified)
                            {
                                if (IncidentTypes.Any(a =>
                                    a.IncidentTypeShortDesc == ((IncidentType)entityEntry.Entity).IncidentTypeShortDesc
                                    || a.IncidentTypeDesc == ((IncidentType)entityEntry.Entity).IncidentTypeDesc
                                    ))
                                {
                                    builder.AppendFormat("Duplicate Short: {0} and/or Discription: {1}. ",
                                        ((IncidentType)entityEntry.Entity).IncidentTypeShortDesc,
                                        ((IncidentType)entityEntry.Entity).IncidentTypeDesc
                                    );
                                    _reported = true;
                                }
                            }
                            if (!_reported)
                            {
                                builder.AppendFormat("{0}. ", upExc.GetBaseException().Message);
                            }
                            break;
                        case "EmailTemplate":
                            // Company specific IncidentType
                            if (entityEntry.State == EntityState.Added || entityEntry.State == EntityState.Modified)
                            {
                                if (EmailTemplates.Any(a =>
                                    a.CompanyId == ((EmailTemplate)entityEntry.Entity).CompanyId
                                    && a.IncidentTypeId == ((EmailTemplate)entityEntry.Entity).IncidentTypeId
                                    ))
                                {
                                    builder.AppendFormat("Duplicate Company: {0}-{1} and IncidentType: {2}-{3}. ",
                                        ((EmailTemplate)entityEntry.Entity).Company.CompanyShortName,
                                        ((EmailTemplate)entityEntry.Entity).CompanyId,
                                        ((EmailTemplate)entityEntry.Entity).IncidentType.IncidentTypeShortDesc,
                                        ((EmailTemplate)entityEntry.Entity).IncidentTypeId
                                    );
                                    _reported = true;
                                }
                            }
                            if (!_reported)
                            {
                                builder.AppendFormat("{0}. ", upExc.GetBaseException().Message);
                            }
                            break;
                        case "NIC":
                            if (entityEntry.State == EntityState.Added || entityEntry.State == EntityState.Modified)
                            {
                                if (NICs.Any(a =>
                                        a.NIC_Id == ((NIC)entityEntry.Entity).NIC_Id
                                        || a.NICDescription == ((NIC)entityEntry.Entity).NICDescription
                                    ))
                                {
                                    builder.AppendFormat("Duplicate Id: {0} and/or Discription: {1}. ",
                                        ((NIC)entityEntry.Entity).NIC_Id,
                                        ((NIC)entityEntry.Entity).NICDescription);
                                    _reported = true;
                                }
                            }
                            if (!_reported)
                            {
                                builder.AppendFormat("{0}. ", upExc.GetBaseException().Message);
                            }
                            break;
                        case "NoteType":
                            if (entityEntry.State == EntityState.Added || entityEntry.State == EntityState.Modified)
                            {
                                if (NoteTypes.Any(a =>
                                    a.NoteTypeShortDesc == ((NoteType)entityEntry.Entity).NoteTypeShortDesc
                                    || a.NoteTypeDesc == ((NoteType)entityEntry.Entity).NoteTypeDesc
                                    ))
                                {
                                    builder.AppendFormat("Duplicate Short: {0} and/or Discription: {1}. ",
                                        ((NoteType)entityEntry.Entity).NoteTypeShortDesc,
                                        ((NoteType)entityEntry.Entity).NoteTypeDesc
                                    );
                                    _reported = true;
                                }
                            }
                            if (!_reported)
                            {
                                builder.AppendFormat("{0}. ", upExc.GetBaseException().Message);
                            }
                            break;
                        default:
                            builder.AppendFormat("{0}. ", upExc.GetBaseException().Message);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                builder.Append("Error parsing DbUpdateException: " + e.ToString());
            }
            //
            return builder.ToString();
        }
    }
}
// ===========================================================================
