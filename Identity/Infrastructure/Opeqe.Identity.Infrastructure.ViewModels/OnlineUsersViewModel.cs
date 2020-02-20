using Opeqe.Identity.Infrastructure.Entities;
using System.Collections.Generic;

namespace Opeqe.Identity.Infrastructure.ViewModels
{
    public class OnlineUsersViewModel
    {
        public List<User> Users { set; get; }
        public int NumbersToTake { set; get; }
        public int MinutesToTake { set; get; }
        public bool ShowMoreItemsLink { set; get; }
    }
}