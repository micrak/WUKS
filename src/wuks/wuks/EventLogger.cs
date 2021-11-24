using System;
using System.Diagnostics;

namespace wuks
{
    internal static class EventLogger
    {
        public static void Initialize()
        {
            Trace.Listeners.Add(new EventLogTraceListener(Constants.ServiceName));
        }

        public static void Info(string message)
        {
            Trace.TraceInformation(message);
        }

        public static void Error(string message)
        {
            Trace.TraceError(message);
        }

        public static void Error(string message, Exception exception)
        {
            Error(message + Environment.NewLine + exception);
        }
    }
}