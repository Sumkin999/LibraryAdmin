using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAdmin.Business.Models
{
    public class Book
    {
        public long? Id { get; set; }
        public int? Year { get; set; }
        public string? Title { get; set; }
        public long? AuthorId { get; set; }
        public int? BooksAmount { get; set; }
        //public DateTimeOffset CreatedDate { get; set; }
    }
}
