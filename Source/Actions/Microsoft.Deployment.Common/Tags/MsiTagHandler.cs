using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using Newtonsoft.Json.Linq;

using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.AppLoad;
using Microsoft.Deployment.Common.Model;

namespace Microsoft.Deployment.Common.Tags
{
    [Export(typeof(ITagHandler))]
    public class MsiTagHandler : ITagHandler
    {
        public bool Recurse { get; } = false;

        public string Tag { get; } = "MSI";

        public object ProcessTag(JToken innerJson, JToken entireJson, Dictionary<string, UIPage> allPages, Dictionary<string, IAction> allActions, App app, List<TagReturn> childObjects)
        {
            var val = innerJson["Guid"];

            app.MsiGuid = Guid.Parse(val.ToString());

            return null;
        }
    }
}