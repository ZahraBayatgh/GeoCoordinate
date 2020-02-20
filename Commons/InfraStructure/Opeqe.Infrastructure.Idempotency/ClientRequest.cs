using System;

namespace Opeqe.Domain.Entities
{
    public class ClientRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
    }

}
