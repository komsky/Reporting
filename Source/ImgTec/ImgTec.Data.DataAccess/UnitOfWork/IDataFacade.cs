using System;
using System.Threading.Tasks;
using ImgTec.Data.DataAccess.Repositories;

namespace ImgTec.Data.DataAccess.UnitOfWork
{
    public interface IDataFacade : IDisposable
    {
        void Commit();
        Task CommitAsync();
        ICaseRepository Cases { get; }
        UserRepository Users { get; }
        RoleRepository Roles { get; }
    }
}
