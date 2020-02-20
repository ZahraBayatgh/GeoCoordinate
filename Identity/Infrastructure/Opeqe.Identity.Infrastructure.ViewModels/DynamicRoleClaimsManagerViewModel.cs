using DNTCommon.Web.Core;
using Opeqe.Identity.Infrastructure.Entities;
using System.Collections.Generic;

namespace Opeqe.Identity.Infrastructure.ViewModels
{
    public class DynamicRoleClaimsManagerViewModel
    {
        public string[] ActionIds { set; get; }

        public int RoleId { set; get; }

        public Role RoleIncludeRoleClaims { set; get; }

        public ICollection<MvcControllerViewModel> SecuredControllerActions { set; get; }
    }
}