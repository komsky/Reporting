using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImgTec.Data.Entities;

namespace ImgTec.Domain.Handlers
{
    /// <summary>
    /// We want to not use IdentityUser as there is a dependency on Entity Framework
    /// but due to lack of time, I'll brake this rule on purpose.
    /// I would remove dependency on project's second phase/release
    /// </summary>
    public interface IUserHandler :IBaseHandler
    {
        User GetById(string id);

    }
}
