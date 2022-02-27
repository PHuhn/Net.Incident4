// ===========================================================================
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using NSG.NetIncident4.Core.Domain.Entities;
//
namespace NSG.NetIncident4.Core.Domain.Entities.Authentication
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
            var keys = primaryKey.Properties.ToDictionary(x => x.Name, x => x.PropertyInfo.GetValue(entity));
            return keys;
        }
        //
    }
}
// ===========================================================================
