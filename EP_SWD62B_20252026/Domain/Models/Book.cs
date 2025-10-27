using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Book
    {
        [Key(), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }

        public double WholesalePrice { get; set; }

        public int PublishedYear { get; set; }

        [ForeignKey("CategoryFK")]
        public virtual Category Category { get; set; } //This is called a navigational property.

        public int CategoryFK { get; set; } //This is a foreign key.

        public int Stock { get; set; }

        public string? Path { get; set; } //Since we already have books in the database, we do not want to create issues with existing records. Therefore, ? allows for nullable values. You can have a string or a null value.
    }
}