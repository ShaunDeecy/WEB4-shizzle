using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppLes.Models
{
    public class CountryException : Exception
    {
        public CountryException(string message) : base(message)
        {

        }

    }
}
