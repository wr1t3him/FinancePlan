using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancePlan.Models
{
    public class Budget
    {
        public int ID { get; set; }
        public decimal value { get; set; }
        public decimal balance { get; set; }
        public int HouseHoldID { get; set; }
        public string BudgetType { get; set; }
        public string BudgetItem { get; set; }

        public virtual Household Household { get; set; }

    }
}