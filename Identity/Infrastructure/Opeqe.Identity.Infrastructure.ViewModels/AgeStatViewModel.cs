using Opeqe.Identity.Infrastructure.Entities;


namespace Opeqe.Identity.Infrastructure.ViewModels
{
    public class AgeStatViewModel
    {
        private const char RleChar = (char)0x202B;

        public int UsersCount { set; get; }
        public int AverageAge { set; get; }
        public User MaxAgeUser { set; get; }
        public User MinAgeUser { set; get; }

    }
}