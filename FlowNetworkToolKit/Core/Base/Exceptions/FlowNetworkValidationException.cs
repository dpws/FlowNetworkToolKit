using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowNetworkToolKit.Core.Base.Exceptions
{
    class FLowNetworkValidationException: Exception
    {
        public FLowNetworkValidationException(string message) : base(message)
        {
        }

        public FLowNetworkValidationException() : base()
        {
        }

        public FLowNetworkValidationException(List<string> messages) : base(String.Join("\r\n", messages))
        {
        }
    }
}
