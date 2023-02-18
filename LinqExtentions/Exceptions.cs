using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExtensions
{
    public static class Exceptions
    {
        public static void ThrowIfNull(this object source, string nameOfProp)
        {
            if (source == null)
                throw new ArgumentNullException(nameOfProp + " is null");
        }
    }
}
