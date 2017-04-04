using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEnvang.Models
{
    public class BaseService
    {
        public String ConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }
        }

        public static String StaticConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }
        }
    }
}