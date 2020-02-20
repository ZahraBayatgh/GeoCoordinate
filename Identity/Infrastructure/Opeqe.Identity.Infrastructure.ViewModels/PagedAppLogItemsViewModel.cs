using cloudscribe.Web.Pagination;
using Opeqe.Identity.Infrastructure.Entities;
using System.Collections.Generic;

namespace Opeqe.Identity.Infrastructure.ViewModels
{
    public class PagedAppLogItemsViewModel
    {
        public PagedAppLogItemsViewModel()
        {
            Paging = new PaginationSettings();
        }

        public string LogLevel { get; set; } = string.Empty;

        public List<AppLogItem> AppLogItems { get; set; }

        public PaginationSettings Paging { get; set; }
    }
}