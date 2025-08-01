﻿using DatabaseAccessLayer.Enumerations;
using DatabaseAccessLayer.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DatabaseAccessLayer.Entities
{
    [Table("Addresses")]
    public class Address : BaseEntity
    {
        public AddressType AddressType { get; set; }
        [Required]
        [MaxLength(200)]
        public string Text { get; set; }
        public long PersonId { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }
    }
}
