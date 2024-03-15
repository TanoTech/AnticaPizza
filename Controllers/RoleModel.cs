using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnticaPizza.Controllers
{
    public class RoleModel
    {
        public const string Admin = "admin";
        public const string Customer = "customer";

        public static string GetRoleFromSession()
        {
            int? adminId = HttpContext.Current.Session?["AdminID"] as int?;


            if (adminId.HasValue)
            {

                return Admin;
            }
            else
            {

                return Customer;
            }
        }
    }

}