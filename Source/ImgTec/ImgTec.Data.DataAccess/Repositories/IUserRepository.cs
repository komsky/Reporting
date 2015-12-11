using ImgTec.Data.Entities;

namespace ImgTec.Data.DataAccess.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByEmail(string email);
        User GetByUserName(string userName);
    }
}
