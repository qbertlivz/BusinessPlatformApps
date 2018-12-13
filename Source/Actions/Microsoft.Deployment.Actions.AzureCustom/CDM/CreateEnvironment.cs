﻿//-----------------------------------------------------------------------
// <copyright file="GetCMDEntities.cs" company="Microsoft Corp.">
//     Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Threading.Tasks;

namespace Microsoft.Deployment.Actions.AzureCustom.CDM
{
    using System.ComponentModel.Composition;
    using System.Net.Http;
    using Microsoft.Deployment.Common.ActionModel;
    using Microsoft.Deployment.Common.Actions;
    using Microsoft.Deployment.Common.Helpers;

    [Export(typeof(IAction))]
    public class CreateEnvironment : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            var azureToken = request.DataStore.GetJson("AzureToken", "access_token");
            var location = request.DataStore.GetValue("Location").ToString();
            var environmentName = request.DataStore.GetJson("EnvironmentName").ToString();

            AzureHttpClient client = new AzureHttpClient(azureToken);

            var response = await client.ExecuteGenericRequestWithHeaderAsync(HttpMethod.Post, "https://management.azure.com/providers/Microsoft.BusinessAppPlatform/environments?api-version=2016-11-01&id=/providers/Microsoft.BusinessAppPlatform/scopes/admin/environments", $"{{\"location\":\"{location}\",\"properties\":{{\"displayName\":\"{environmentName}\"}}}}");
            var responseString = await response.Content.ReadAsStringAsync();
            var responseParsed = JsonUtility.GetJsonObjectFromJsonString(responseString);
            var environId = responseParsed["name"].ToString();

            request.DataStore.AddToDataStore("EnvironmentID", environId, DataStoreType.Public);

            return new ActionResponse(ActionStatus.Success);
        }
    }
}