using CefSharp;

namespace Installer
{
    public class DownloadHandler : IDownloadHandler
    {
        public void OnBeforeDownload(IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    callback.Continue("SolutionTemplate.pbix", showDialog: true);
                }
            }
        }

        public void OnDownloadUpdated(IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
        }
    }
}