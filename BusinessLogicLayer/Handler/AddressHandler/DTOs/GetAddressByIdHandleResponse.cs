using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Handler.AddressHandler.DTOs
{
    public class GetAddressByIdHandleResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public long Id { get; set; }
        public string Text { get; set; }
        public AddressType AddressType { get; set; }
        public long PersonId { get; set; }
        public Status Status { get; set; }
    }
}
