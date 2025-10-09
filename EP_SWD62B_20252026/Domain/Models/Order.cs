using Humanizer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Order
    {
        [Key] //To make a Guid property auto-generated, either set it manually using Guid.NewGuid() or configure in the DbContext class.
        public Guid Id { get; set; }

        public string Username { get; set; }

        public DateTime DatePlaced { get; set; }
    }
}