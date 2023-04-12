//
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
//
using NSG.NetIncident4.Core.Application.Commands.CompanyEmailTemplates;
using NSG.NetIncident4.Core.Application.Commands.IncidentTypes;
//
namespace NSG.NetIncident4.Core.UI.ViewModels
{
    public class CompanyEmailTemplatesCreateViewModel
    {
        public List<SelectListItem> CompanySelect { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> IncidentTypeSelect { get; set; } = new List<SelectListItem>();
        public List<IncidentTypeListQuery> IncidentTypes { get; set; } = new List<IncidentTypeListQuery>();
    }
}