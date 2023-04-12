using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NSG.NetIncident4.Core.Persistence;
//
namespace NSG.NetIncident4.Core.Domain.Entities
{
    [Table("ApplicationUserApplicationServer")]
    public class ApplicationUserServer
    {
        [Required]
        public string Id { get; set; } = String.Empty;
        public virtual ApplicationUser User { get; set; }
        //
        [Required]
        public int ServerId { get; set; } = 0;
        public virtual Server Server { get; set; }
    }
}
//