using System;
using System.Threading.Tasks;
using ImgTec.Data.DataAccess.Repositories;

namespace ImgTec.Data.DataAccess.UnitOfWork
{
    public class DataFacade : IDataFacade
    {
        #region Fields
        private bool _disposed;
        private ImgTecDbContext _dbContext;
        private ICaseRepository _cases;
        private UserRepository _users;
        private RoleRepository _roles;
        #endregion

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Constructors

        public DataFacade(ImgTecDbContext context)
        {
            CreateDbContext(context);
        }

        public DataFacade()
        {
            CreateDbContext(null);
        }

        private void CreateDbContext(ImgTecDbContext context)
        {
            _dbContext = context ?? new ImgTecDbContext();

            _dbContext.Configuration.ProxyCreationEnabled = false;
            _dbContext.Configuration.LazyLoadingEnabled = false;
            _dbContext.Configuration.ValidateOnSaveEnabled = false; 
        }

        #endregion

        #region Commits
        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        } 
        #endregion

        #region Repositories
        public ICaseRepository Cases
        {
            get { return _cases ?? (_cases = new CaseRepository(_dbContext)); }
        }

        public UserRepository Users
        {
            get { return _users ?? (_users = new UserRepository(_dbContext)); }
        }

        public RoleRepository Roles
        {
            get { return _roles ?? (_roles = new RoleRepository(_dbContext)); }

        }
        #endregion
    }
}
