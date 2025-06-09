using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.Exceptions
{
    public class ValueMultiplierException : Exception
    {
        public ValueMultiplierException(string message) : base(message)
        {

        }
    }
}
