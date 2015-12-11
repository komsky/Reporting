using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImgTec.Data.Entities;

namespace ImgTec.Data.DataAccess
{
    public class ImgTecDbContext : DbContext
    {
        #region Properties

        public DbSet<Case> Cases { get; set; }
        #endregion
    }
}
