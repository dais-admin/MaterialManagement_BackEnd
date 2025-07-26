using DAIS.DataAccess.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DAIS.DataAccess.Entities
{
    public class User  : IdentityUser
    {     
        public UserTypes UserType { get; set; }
        public string? EmployeeName {  get; set; }
        public bool IsInitialLogin { get; set; }=false;
        public Guid? ReportingTo {  get; set; }
        public string UserPhoto {  get; set; }
        public Guid? RegionId { get; set; }
        public virtual Region Region { get; set; }
        public Guid? LocationId { get; set; }
        public virtual LocationOperation Location { get; set; }
        public Guid? SubDivisionId { get; set; }
        public virtual SubDivision SubDivision { get; set; }
        public Guid? ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }    
}
