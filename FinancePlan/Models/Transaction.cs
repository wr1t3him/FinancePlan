using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancePlan.Models
{
    public class Transaction
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string description { get; set; }
        public decimal Cost { get; set; }
        public bool verify { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string UserID { get; set; }
        //public int HouseholdID { get; set; }
        public int AccountID { get; set; }

        //public virtual Household Household { get; set; }
        public virtual Account Account { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}