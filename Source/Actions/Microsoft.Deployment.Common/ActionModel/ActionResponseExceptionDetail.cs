namespace Microsoft.Deployment.Common.ActionModel
{
    using ErrorResources;
    using Helpers;
    using System;

    public class ActionResponseExceptionDetail
    {
        public Exception ExceptionCaught = null;
        public string LogLocation = string.Empty;
        public string FriendlyMessageCode;

        public string FriendlyErrorMessage
        {
            get
            {
                return this.FriendlyMessageCode == null ? EnglishErrorCodes.DefaultErrorCode : ErrorUtility.GetErrorCode(this.FriendlyMessageCode);
            }
        }

        public string AdditionalDetailsErrorMessage { get; set; } = string.Empty;

        public ActionResponseExceptionDetail(string code = "", string additionalCode = "")
        {
            this.FriendlyMessageCode = code;
            if (!string.IsNullOrEmpty(additionalCode)) this.AdditionalDetailsErrorMessage = ErrorUtility.GetAdditionalDetailsMessage(additionalCode);
        }
    }
}