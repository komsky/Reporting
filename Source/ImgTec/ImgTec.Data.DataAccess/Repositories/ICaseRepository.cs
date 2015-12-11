using System;
using System.Collections.Generic;
using ImgTec.Data.Entities;
using ImgTec.Data.Entities.Enums;

namespace ImgTec.Data.DataAccess.Repositories
{
    public interface ICaseRepository : IRepository<Case>
    {
        List<Case> GetCasesByState(CaseState caseState);
        List<Case> GetCasesByCustomer(User customer);
        List<Case> GetCasesByCustomerId(String customerId);
        List<Case> GetCasesByAgent(User agent);
        List<Case> GetCasesByAgentId(String agentId);
    }
}
