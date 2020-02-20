using Opeqe.Identity.Infrastructure.Entities;
using System.Collections.Generic;

namespace Opeqe.Identity.Infrastructure.ViewModels
{
    public class TodayBirthDaysViewModel
    {
        public List<User> Users { set; get; }

        public AgeStatViewModel AgeStat { set; get; }
    }
}