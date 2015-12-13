using System;
using System.ComponentModel.DataAnnotations;
using ImgTec.Web.Models.Enums;

namespace ImgTec.Web.Models
{
    public class Case
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String Title { get; set; }
        [Display(Name = "State")]
        public CaseState CaseState { get; set; }

        [Required]
        public String Description { get; set; }
        [Display(Name = "Priority")]
        public CasePriority CasePriority { get; set; }
        [Display(Name = "Agent's Reply")]
        public String AgentReply { get; set; }
        [Display(Name = "Case Owner")]
        public virtual ApplicationUser Customer { get; set; }

        public virtual ApplicationUser Agent { get; set; }
    }
}