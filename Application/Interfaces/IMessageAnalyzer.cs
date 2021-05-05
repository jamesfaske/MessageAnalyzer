using System;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IMessageAnalyzer
    {
        double MessageRatePerSecond(int timeSpanSeconds, int messageCount);
        List<string> GetHashtagsFromMessage(string message);
        public bool DoesContainsUrl(string message);
        public bool DoesContainsPhotoUrl(string message);
        public List<string> GetDomainsFromMessage(string message);
    }
}
