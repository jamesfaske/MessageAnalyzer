using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;

namespace Application
{
    public class MessageAnalyzer : IMessageAnalyzer
    {
        public double MessageRatePerSecond(int timeSpanSeconds, int messageCount)
        {
            if (timeSpanSeconds < 1)
            {
                throw new ArgumentException("timeSpanSeconds must be greater than 0", nameof(timeSpanSeconds));
            }

            if (messageCount == 0)
            {
                throw new ArgumentException("messageCount must be greater than 0", nameof(messageCount));
            }

            return (double) messageCount / timeSpanSeconds;
        }
    }
}
