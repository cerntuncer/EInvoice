using DatabaseAccessLayer.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessLayer.Models
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }

       
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public Status Status { get; set; }
    }
}
