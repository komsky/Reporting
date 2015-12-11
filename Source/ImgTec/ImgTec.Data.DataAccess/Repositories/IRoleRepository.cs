using ImgTec.Data.Entities;

namespace ImgTec.Data.DataAccess.Repositories
{
    public interface IRoleRepository :IRepository<Role>
    {
        Role GetByName(string name);
    }
}
