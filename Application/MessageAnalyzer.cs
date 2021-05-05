using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Application.Interfaces;

namespace Application
{
    public class MessageAnalyzer : IMessageAnalyzer
    {
        private readonly Regex _hashtagRegex = new Regex(@"#\w+");
        private readonly Regex _urlRegex = new Regex(@"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$");
        private readonly Regex _twitterPhotoUrlRegex = new Regex(@"^(ht|f)tp(s?)\:\/\/pic.twitter.com*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$");

        public double MessageRatePerSecond(int timeSpanSeconds, int messageCount)
        {
            if (timeSpanSeconds < 1)
            {
                throw new ArgumentException("value must be greater than 0", nameof(timeSpanSeconds));
            }

            if (messageCount == 0)
            {
                throw new ArgumentException("value must be greater than 0", nameof(messageCount));
            }

            return (double) messageCount / timeSpanSeconds;
        }

        public List<string> GetHashtagsFromMessage(string message)
        {
            var hashtagList = new List<string>();

            foreach (var match in _hashtagRegex.Matches(message))
            {
                hashtagList.Add(match.ToString());
            }

            return hashtagList;
        }

        public bool DoesContainsUrl(string message)
        {
            return _urlRegex.IsMatch(message);
        }

        public bool DoesContainsPhotoUrl(string message)
        {
            return _twitterPhotoUrlRegex.IsMatch(message);
        }

        public List<string> GetDomainsFromMessage(string message)
        {
            var domainList = new List<string>();

            foreach (var match in _urlRegex.Matches(message))
            {
                //parse the domain (host) from url
                var url = new Uri(match.ToString());
                domainList.Add(url.Host);
            }

            return domainList;
        }

    }
}
