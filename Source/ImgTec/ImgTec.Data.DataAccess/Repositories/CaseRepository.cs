using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImgTec.Data.Entities;
using ImgTec.Data.Entities.Enums;

namespace ImgTec.Data.DataAccess.Repositories
{
    public class CaseRepository: WebGenericRepository<Case>, ICaseRepository
    {
        public CaseRepository(ImgTecDbContext dbContext) : base(dbContext)
        {
        }

        public List<Case> GetCasesByState(CaseState caseState)
        {
            List<Case> cases = GetAll()
                .Include(x => x.Customer)
                .Include(x => x.Agent)
                .Where(x=>x.CaseState == caseState)
                .ToList();

            return cases;
        }

        public List<Case> GetCasesByCustomer(User customer)
        {
            List<Case> cases = GetAll()
                .Include(x => x.Customer)
                .Include(x => x.Agent)
                .Where(x => x.Customer == customer)
                .ToList();

            return cases;
        }

        public List<Case> GetCasesByCustomerId(string customerId)
        {
            List<Case> cases = GetAll()
                .Include(x => x.Customer)
                .Include(x => x.Agent)
                .Where(x => x.Customer.Id == customerId)
                .ToList();

            return cases;
        }

        public List<Case> GetCasesByAgent(User agent)
        {
            List<Case> cases = GetAll()
                .Include(x => x.Customer)
                .Include(x => x.Agent)
                .Where(x => x.Agent == agent)
                .ToList();

            return cases;
        }

        public List<Case> GetCasesByAgentId(string agentId)
        {
            List<Case> cases = GetAll()
                .Include(x => x.Customer)
                .Include(x => x.Agent)
                .Where(x => x.Agent.Id == agentId)
                .ToList();

            return cases;
        }
    }
}
