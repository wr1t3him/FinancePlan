using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancePlan.Models
{
    public class Bank
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string phone { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }

        public Bank()
        {
            this.Accounts = new HashSet<Account>();
        }
    }
}