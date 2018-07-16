using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancePlan.ViewModels
{
    public class AcceptInviteViewModel
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }

    public class ViewUsers
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? HouseholdID { get; set; }
    }
}