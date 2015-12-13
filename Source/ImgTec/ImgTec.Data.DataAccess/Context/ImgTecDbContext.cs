using System.Data.Entity;
using ImgTec.Data.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ImgTec.Data.DataAccess
{
    public class ImgTecDbContext : IdentityDbContext<User>
    {
        #region Properties
        public DbSet<Case> Cases { get; set; }
        public DbSet<User> Users { get; set; }
        #endregion

        #region Constructor

        public ImgTecDbContext()
        {
        }

        #endregion

        #region Context Overrides
        #endregion
    }
}
