using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImgTec.Data.Entities;

namespace ImgTec.Data.DataAccess.Repositories
{
    public class RoleRepository : WebGenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(ImgTecDbContext dbContext)
            : base(dbContext)
        {

        }
        public Role GetByName(string name)
        {
            return GetAll().SingleOrDefault(x => x.Name.ToLower() == name.ToLower());
        }
    }
}
