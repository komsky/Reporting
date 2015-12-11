using System;
using System.ComponentModel.DataAnnotations;
using ImgTec.Data.Entities.Enums;

namespace ImgTec.Data.Entities
{
    public class Case
    {
        [Key]
        public int Id { get; set; }
        public String Title { get; set; }
        public CaseState CaseState { get; set; }
        public String Description { get; set; }
        public CasePriority CasePriority { get; set; }
        public String AgentReply { get; set; }
        public User Customer { get; set; }
        public User AssignedAgent { get; set; }
    }
}
