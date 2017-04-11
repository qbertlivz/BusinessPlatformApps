using Microsoft.WindowsAzure.Storage.Table;

namespace Microsoft.Deployment.Actions.Custom.CustomerData
{
    public class CustomerInfoSimplement : TableEntity
    {
        public CustomerInfoSimplement(string emailAddress) : this()
        {
            RowKey = emailAddress;
        }

        private CustomerInfoSimplement()
        {
            PartitionKey = "Simplement.SolutionTemplate.AR";
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentName { get; set; }
    }
}