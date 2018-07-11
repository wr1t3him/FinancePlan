using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancePlan.Models
{
    public class Household
    {
        public int ID { get; set; }
        public string Name { get; set; }
        


        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }
      
        
        public Household()
        {
            this.Users = new HashSet<ApplicationUser>();
            this.Transactions = new HashSet<Transaction>();
            this.Accounts = new HashSet<Account>();
            this.Budgets = new HashSet<Budget>();
            this.Invitations = new HashSet<Invitation>();
        }
    }
}