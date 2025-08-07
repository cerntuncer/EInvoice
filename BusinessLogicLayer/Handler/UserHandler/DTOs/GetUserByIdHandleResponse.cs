using DatabaseAccessLayer.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Handler.UserHandler.DTOs
{
    public class GetUserByIdHandleResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public long Id { get; set; }
        public UserType Type { get; set; }
        public long PersonId { get; set; }
        public Status Status { get; set; }
    }
}
