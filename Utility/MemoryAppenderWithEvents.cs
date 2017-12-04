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
    public class MemoryAppenderWithEvents : MemoryAppender
    {
        public event EventHandler Updated;

        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            // Append the event as usual
            base.Append(loggingEvent);

            // Then alert the Updated event that an event has occurred
            Updated?.Invoke(this, new EventArgs());
        }
    }
}
