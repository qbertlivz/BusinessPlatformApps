using System;
using System.Collections.Generic;

namespace Microsoft.Deployment.Common.ActionModel
{
    public class DictionaryIgnoreCase<TV> : Dictionary<string, TV>
    {
        public DictionaryIgnoreCase() : base(StringComparer.OrdinalIgnoreCase)
        {
        }
    }
}