using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAdmin.Business.Models
{
    public class Author
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Genre { get; set; }
        //public required DateTimeOffset CreatedDate { get; set; }
    }
}
