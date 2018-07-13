using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancePlan.Models
{
    public class Invitation
    {
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
        public string code { get; set; }
        public int Lifespan { get; set; }
        public int HouseholdID { get; set; }

        public virtual Household Household { get; set; }
    }
}