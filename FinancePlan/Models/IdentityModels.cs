using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FinancePlan.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string displayname { get; set; }
        //public string Role { get; set; }
        //public decimal Income { get; set; }
        public int? HouseholdID { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }

        public ApplicationUser()
        {
            this.Accounts = new HashSet<Account>();
            this.Transactions = new HashSet<Transaction>();
        }

        public virtual Household Household { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<FinancePlan.Models.Budget> Budgets { get; set; }

        public System.Data.Entity.DbSet<FinancePlan.Models.Household> Households { get; set; }

        public System.Data.Entity.DbSet<FinancePlan.Models.Bank> Banks { get; set; }

        public System.Data.Entity.DbSet<FinancePlan.Models.Account> Accounts { get; set; }

        public System.Data.Entity.DbSet<FinancePlan.Models.Invitation> Invitations { get; set; }

        public System.Data.Entity.DbSet<FinancePlan.Models.Transaction> Transactions { get; set; }

        public System.Data.Entity.DbSet<FinancePlan.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}