using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancePlan.Models
{
    public class Account
    {
        public int ID { get; set; }
        public decimal Balance { get; set; }
        public string Type { get; set; }
        public string UserID { get; set; }
        public int HouseholdID { get; set; }
        public int BankID { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public Account()
        {
            this.Transactions = new HashSet<Transaction>();
        }

        public virtual Household Household { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Bank Bank { get; set; }
    }
}