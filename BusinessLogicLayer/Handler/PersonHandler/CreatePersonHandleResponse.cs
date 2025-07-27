using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Handler.PersonHandler
{
    public class CreatePersonHandleResponse
    {
       public string Message { get; set; }
       public bool Error { get; set; }
    }
}
