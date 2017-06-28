using System.Diagnostics;

namespace Microsoft.Bpst.App.Msi
{
    public class Logger
    {
        private const string Source = "Microsoft Solution Template";
        private const string SourceLog = "Application";

        public Logger()
        {
            if (!EventLog.SourceExists(Source))
            {
                EventLog.CreateEventSource(Source, SourceLog);
            }
        }

        //public void WriteException(Exception e)
        //{
        //    string exceptionMessages = "";
        //    Exception exc = e;
        //    while (exc != null)
        //    {
        //        exceptionMessages += " " + exc.Message;
        //        exc = exc.InnerException;
        //    }

            
        //    EventLog.WriteEntry(Source, exceptionMessages, EventLogEntryType.Error);
        //}

        //public void WriteMessage(string message)
        //{
        //    EventLog.WriteEntry(Source, message);
        //}
    }
}