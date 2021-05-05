using System;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IMessageAnalyzer
    {
        double MessageRatePerSecond(DateTime startTimeUtc, int messageCount);
        List<string> GetHashtagsFromMessage(string message);
        public bool DoesContainUrl(string message);
        public bool DoesContainPhotoUrl(string message);
        public List<string> GetDomainsFromMessage(string message);
    }
}
