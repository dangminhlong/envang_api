using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebEnvang.Models
{
    [Table("PageRole")]
    public class PageRole
    {
        [Key, Column(Order = 0)]
        public int PageId { get; set; }
        [Key, Column(Order = 1)]
        public string RoleId { get; set; }        
    }
}