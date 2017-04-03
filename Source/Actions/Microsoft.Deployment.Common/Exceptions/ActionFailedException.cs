using System;

namespace Microsoft.Deployment.Common.Exceptions
{
    public class ActionFailedException :Exception
    {
        public ActionFailedException(string message) : base(message)
        {
        }
    }
}