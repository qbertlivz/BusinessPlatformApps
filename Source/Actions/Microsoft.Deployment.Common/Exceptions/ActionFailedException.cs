using System;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Helpers;

namespace Microsoft.Deployment.Common.Exceptions
{
    public class ActionFailedException :Exception
    {
        public ActionFailedException(string message) : base(message)
        {
        }
    }
}
