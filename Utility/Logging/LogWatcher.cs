using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Appender;
using System.ComponentModel;
using System.IO;
using System.Globalization;
using log4net;
using log4net.Core;

namespace SpongeBot.Utility
{
    public class LogWatcher : MemoryAppender
    {
        public event EventHandler<LogEventArgs> LogEvent;

        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            // Append the event as usual
            base.Append(loggingEvent);

            // Then alert the Updated event that an event has occurred
            LogEvent?.Invoke(this, new LogEventArgs(loggingEvent));
        }

        internal static LogWatcher get()
        {
            return (LogWatcher)Array.Find(LogManager.GetRepository().GetAppenders(), LogWatcherFilter);
        }

        private static bool LogWatcherFilter(IAppender appender)
        {
            // Returns the IAppender named MemoryAppender in the Log4Net.config file
            return appender.Name.Equals("LogWatcher");
        }
    }

public class LogEventArgs : EventArgs
    {
        public LogEventArgs(LoggingEvent logEvent)
        {
            this.LogEvent = logEvent;
        }

        public LoggingEvent LogEvent { get; }
    }
}
