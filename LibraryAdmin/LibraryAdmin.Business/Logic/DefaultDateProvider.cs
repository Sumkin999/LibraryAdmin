using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAdmin.Business
{
    public static class DefaultDateProvider
    {
        public static readonly DateOnly AuthorBirthDateMin = new DateOnly(1900, 1, 1);
        public static readonly int BookYearMin = 1900;

        public static DateTimeOffset Now()
        {
            return DateTimeOffset.Now;
        }
    }
}
