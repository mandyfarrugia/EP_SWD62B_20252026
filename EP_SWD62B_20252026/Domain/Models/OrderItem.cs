using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class OrderItem
    {
        [Key(), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /* Navigational Properties:
         * 1) They allow you to navigate through the property to get the data about the book.
         * 2) They save you the time to make another request to the database. 
         * 3) They save you the time to forge another inner join request. */
        [ForeignKey("BookFK")]
        public virtual Book Book { get; set; }

        public int BookFK { get; set; }

        public int Qty { get; set; }

        [ForeignKey("OrderFK")]
        public virtual Order Order { get; set; }

        public Guid OrderFK { get; set; }
    }
}