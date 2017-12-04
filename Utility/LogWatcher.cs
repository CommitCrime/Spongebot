﻿using log4net;
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
        private string logContent;

        private MemoryAppenderWithEvents memoryAppender;

        public event EventHandler Updated;

        public string LogContent
        {
            get { return logContent; }
        }

        public LogWatcher()
        {
            // Get the memory appender
            memoryAppender = (MemoryAppenderWithEvents)Array.Find(LogManager.GetRepository().GetAppenders(), GetMemoryAppender);

            // Read in the log content
            this.logContent = GetEvents(memoryAppender);

            // Add an event handler to handle updates from the MemoryAppender
            memoryAppender.Updated += HandleUpdate;
        }

        public void HandleUpdate(object sender, EventArgs e)
        {
            this.logContent += GetEvents(memoryAppender);

            // Then alert the Updated event that the LogWatcher has been updated
            var handler = Updated;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        private static bool GetMemoryAppender(IAppender appender)
        {
            // Returns the IAppender named MemoryAppender in the Log4Net.config file
            if (appender.Name.Equals("MemoryAppender"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetEvents(MemoryAppenderWithEvents memoryAppender)
        {
            StringBuilder output = new StringBuilder();

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
                    // Construct the line we want to log
                    string line = ev.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss,fff") + " [" + ev.ThreadName + "] " + ev.Level + " " + ev.LoggerName + ": " + ev.RenderedMessage + "\r\n";

                    // Append to the StringBuilder
                    output.Append(line);
                }
            }

            // Return the constructed output
            return output.ToString();
        }
    }
}
