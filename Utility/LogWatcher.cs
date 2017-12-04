using log4net;
using log4net.Appender;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeBot.Utility
{
    public class LogWatcher
    {
        private MemoryAppenderWithEvents memoryAppender;

        public event EventHandler<LogEventArgs> Updated;

        public LogWatcher()
        {
            // Get the memory appender
            memoryAppender = (MemoryAppenderWithEvents)Array.Find(LogManager.GetRepository().GetAppenders(), GetMemoryAppender);

            // Add an event handler to handle updates from the MemoryAppender
            memoryAppender.Updated += FireEvents;
        }

        private static bool GetMemoryAppender(IAppender appender)
        {
            // Returns the IAppender named MemoryAppender in the Log4Net.config file
            return appender.Name.Equals("MemoryAppender");
        }

        public void FireEvents(object sender, EventArgs e)
    {
            // Get any events that may have occurred
            LoggingEvent[] events = memoryAppender.GetEvents();

            // Check that there are events to return
            if (events != null && events.Length > 0)
            {
                // If there are events, we clear them from the logger, since we're done with them  
                memoryAppender.Clear();

                // Iterate through each event
                foreach (LoggingEvent ev in events)
                {
                    // Then alert the Updated event that the LogWatcher has been updated
                    Updated?.Invoke(this, new LogEventArgs(ev));
                }
            }
        }
    }

    public class LogEventArgs : EventArgs
    {
        private LoggingEvent logEvent;

        public LogEventArgs(LoggingEvent logEvent)
        {
            this.logEvent = logEvent;
        }

        public LoggingEvent LogEvent { get => logEvent; }
    }
}
