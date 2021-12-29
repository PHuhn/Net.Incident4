using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
//
namespace NSG.NetIncident4.Core.Domain.Entities
{
    [Table("ApplicationUserApplicationServer")]
    public class ApplicationUserServer
    {
        [Required]
        public string Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        //
        [Required]
        public int ServerId { get; set; }
        public virtual Server Server { get; set; }
    }
}
//