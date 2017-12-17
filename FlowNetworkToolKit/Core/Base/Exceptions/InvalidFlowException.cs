using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowNetworkToolKit.Core.Base.Exceptions
{
    class InvalidFlowException : Exception
    {
        public InvalidFlowException(string message) : base(message)
        {
        }

        public InvalidFlowException() : base()
        {

        }
    }
}
