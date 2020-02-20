using System;

namespace Opeqe.Identity.Infrastructure.Entities
{
    public partial class AppLoginData
    {
        public int Id { get; set; }
        public string RegId { get; set; }
        public string PhoneUniqueId { get; set; }
        public int ContactId { get; set; }
        public bool IsDeleted { get; set; }
        public string Os { get; set; }
        public string UserToken { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }

}