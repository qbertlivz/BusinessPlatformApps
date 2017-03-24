using System.Collections.Generic;

namespace Microsoft.Deployment.Common.Model.Scribe
{
    public class ScribeReplicationSettings
    {
        public List<string> Entities;
        public string SelectionType;

        private ScribeReplicationSettings()
        {
            Entities = new List<string>();
        }

        public ScribeReplicationSettings(string[] entities)
        {
            Entities = entities == null
                ? new List<string>()
                : new List<string>(entities);
            SelectionType = "Selected";
        }

        public static ScribeReplicationSettings CRMCreateForSelected()
        {
            var result = new ScribeReplicationSettings { SelectionType = "Selected" };
            result.Entities.AddRange(new[] { "opportunityproduct", "territory", "lead", "opportunity", "account", "systemusermanagermap", "businessunit", "systemuser", "product" });
            return result;
        }
        public static ScribeReplicationSettings SalesforceCreateForSelected()
        {
            var result = new ScribeReplicationSettings { SelectionType = "Selected" };
            result.Entities.AddRange(new[] { "Account", "Lead", "Opportunity", "OpportunityLineItem", "OpportunityStage", "UserRole", "User", "Product2" });
            return result;
        }

        public static ScribeReplicationSettings CreateForRecommended()
        {
            return new ScribeReplicationSettings { SelectionType = "Recommended" };
        }

        public static ScribeReplicationSettings CreateForAll()
        {
            return new ScribeReplicationSettings { SelectionType = "All" };
        }
    }
}