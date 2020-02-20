using cloudscribe.Web.Pagination;
using Opeqe.Identity.Infrastructure.Entities;
using System.Collections.Generic;

namespace Opeqe.Identity.Infrastructure.ViewModels
{
    public class PagedUsersListViewModel
    {
        public PagedUsersListViewModel()
        {
            Paging = new PaginationSettings();
        }

        public List<User> Users { get; set; }

        public List<Role> Roles { get; set; }

        public PaginationSettings Paging { get; set; }
    }
}
