using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.Roles
{
    public class RoleDTO
    {
        public string RoleId { get; set; }
        public List<int> PageIdList { get; set; }
    }
}