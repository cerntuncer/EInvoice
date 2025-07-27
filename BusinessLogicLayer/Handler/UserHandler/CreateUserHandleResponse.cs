using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Handler.UserHandler
{
    internal class CreateUserHandleResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
    }
}
