using System;
using System.Collections.Generic;

namespace Microsoft.Deployment.Actions.Salesforce.Helpers
{
    static class SalesforceConcurrencyMapper
    {
        public static List<Tuple<string, int>> Limits = new List<Tuple<string, int>>()
        {
            new Tuple<string,int>("Team Edition", 5),
            new Tuple<string,int>("Professional Edition", 25),
            new Tuple<string,int>("Enterprise Edition", 25),
            new Tuple<string,int>("Developer Edition", 5),
            new Tuple<string,int>("Personal Edition", 5),
            new Tuple<string,int>("Unlimited Edition", 25),
            new Tuple<string,int>("Contact Manager Edition",5),
            new Tuple<string,int>("Base Edition", 5)
        };
    }
}