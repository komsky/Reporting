using ImgTec.Data.DataAccess.UnitOfWork;

namespace ImgTec.Domain.Handlers
{
    public class BaseHandler :IBaseHandler
    {
        protected readonly IDataFacade DataFacade;

        protected BaseHandler()
        {
            DataFacade = new DataFacade();
        }

        protected BaseHandler(IDataFacade dataFacade)
        {
            DataFacade = dataFacade;
        }

        public void Dispose()
        {
            DataFacade.Dispose();
        }
    }
}
