using FinancePlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancePlan.ViewModels
{
    public class AcceptInviteViewModel
    {
        public int HouseholdID { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
    }

    public class ViewUsers
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? HouseholdID { get; set; }
    }

    public class HouseView
    {
        public List<Household> households { get; set; }
        public List<Bank> banks { get; set; }
        public List<Account> accounts { get; set; }
        public List<Budget> budgets { get; set; }
        public List<Transaction> transactions { get; set; }

        public HouseView()
        {
            this.households = new List<Household>();
            this.banks = new List<Bank>();
            this.accounts = new List<Account>();
            this.budgets = new List<Budget>();
            this.transactions = new List<Transaction>();
        }

    }
}