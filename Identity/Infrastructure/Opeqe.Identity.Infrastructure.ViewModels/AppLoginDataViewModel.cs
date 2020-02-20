using System;

namespace Opeqe.Identity.Infrastructure.ViewModels
{
    public class AppLoginDataViewModel
    {
        public string RegId { get; set; }
        public string PhoneUniqueId { get; set; }
        public string OS { get; set; }
        public string UserToken { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public string PhoneNumber { get; set; }
    }
}
