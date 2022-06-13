using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class BookModel
    {//copied from Books.cs class
        public int Id { get; set; }

        [Required(ErrorMessage = "plz add the title property")]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
