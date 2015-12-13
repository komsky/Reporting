using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ImgTec.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Case> Cases { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class ApplicationDbContextInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            string userName = "Admin";
            var rolesNames = new List<string> {"Admins", "Agents", "Customers"};
            string password = "Pa$$w0rd";

            //seed roles
            foreach (var roleName in rolesNames)
            {
                if (!roleManager.RoleExists(roleName))
                {
                    var roleresult = roleManager.Create(new IdentityRole(roleName));
                }
            }
            

            //seed user
            var user = new ApplicationUser();
            user.UserName = userName;
            var adminResult = userManager.Create(user, password);
            if (adminResult.Succeeded)
            {
                var result = userManager.AddToRole(user.Id, rolesNames[0]);
            }
            base.Seed(context);
        }

    }
}