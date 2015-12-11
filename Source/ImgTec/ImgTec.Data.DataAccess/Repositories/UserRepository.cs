using System.Linq;
using ImgTec.Data.Entities;

namespace ImgTec.Data.DataAccess.Repositories
{
    public class UserRepository : WebGenericRepository<User>, IUserRepository
    {
        public UserRepository(ImgTecDbContext dbContext) : base(dbContext)
        {
        }

        public User GetByEmail(string email)
        {
            return GetAll().SingleOrDefault(x => x.Email.ToLower() == email.ToLower());
        }

        public User GetByUserName(string userName)
        {
            return GetAll().SingleOrDefault(x => x.UserName.ToLower() == userName.ToLower());
        }
    }
}
