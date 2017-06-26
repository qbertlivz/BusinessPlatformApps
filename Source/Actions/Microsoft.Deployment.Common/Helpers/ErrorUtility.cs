namespace Microsoft.Deployment.Common.Helpers
{
    using ErrorResources;

    public static class ErrorUtility
    {
        public static string GetErrorCode(string code)
        {
            string result = EnglishErrorCodes.ResourceManager.GetString(code);

            return result == null ? EnglishErrorCodes.DefaultErrorCode : result;
        }

        public static string GetAdditionalDetailsMessage(string code)
        {
            string result = EnglishErrorCodes.ResourceManager.GetString(code);

            return result == null ? code : result;
        }
    }
}