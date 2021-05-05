using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;

namespace Application.Features
{
    public class AnalyzeTweets
    {
        public class Command : IRequest<Response>
        {
        }

        public class Response : ResponseBase
        {
            public int Total { get; set; }
            public double RatePerHour { get; set; }
            public double RatePerMinute { get; set; }
            public double RatePerSecond { get; set; }
            public string TopEmoji { get; set; }
            public double PercentageWithUrl { get; set; }
            public double PercentageWithPhotoUrl { get; set; }
            public string TopHashtag { get; set; }
            public string TopDomain { get; set; }
            public double PercentageWithEmojis { get; set; }
        }

        public class CommandHandler : RequestHandler<Command, Response>
        {
            private readonly IRepository _store;
            private readonly IMessageAnalyzer _analyzer;
            private readonly IEmojiService _emojiService;

            public CommandHandler(IRepository store, IMessageAnalyzer analyzer, IEmojiService emojiService)
            {
                _store = store;
                _analyzer = analyzer;
                _emojiService = emojiService;
            }

            protected override Response Handle(Command cmd)
            {
                var response = new Response();

                List<string> tweets;
                DateTime startTime;

                try
                {
                    tweets = _store.ReadAll();
                    startTime = _store.GetStartTime();
                }
                catch (Exception ex)
                {
                    response.Errors.Add($"An error occurred trying to read messages from the store: {ex.Message}");
                    return response;
                }
                
                //Total number of tweets recieved
                response.Total = tweets.Count;

                if (tweets.Count == 0)
                {
                    return response;
                }

                //Tweeting rate
                response.RatePerSecond = _analyzer.MessageRatePerSecond(startTime, tweets.Count);
                if (response.RatePerSecond > 0)
                {
                    response.RatePerMinute = response.RatePerSecond * 60;
                    response.RatePerHour = response.RatePerSecond * 60 * 60;
                }
                
                //Top emojis
                var tweetsWithEmojis = 0;
                var tweetsWithUrl = 0;
                var tweetsWithPhotoUrl = 0;

                var allEmojis = new List<string>();
                var allHashtags = new List<string>();
                var allDomains = new List<string>();

                foreach (var tweet in tweets)
                {
                    var emojiList = _emojiService.GetEmojisFromMessage(tweet);

                    if (emojiList.Count == 0)
                    {
                        tweetsWithEmojis++;
                    }
                    else
                    {
                        allEmojis.AddRange(emojiList);
                    }

                    var hashtagList = _analyzer.GetHashtagsFromMessage(tweet);

                    if (hashtagList.Count > 0)
                    {
                        allHashtags.AddRange(emojiList);
                    }

                    var domainList = _analyzer.GetDomainsFromMessage(tweet);

                    if (domainList.Count == 0)
                    {
                        tweetsWithEmojis++;
                    }
                    else
                    {
                        allDomains.AddRange(emojiList);
                    }

                    var hasUrl = _analyzer.DoesContainUrl(tweet);
                    if (hasUrl)
                    {
                        tweetsWithUrl++;
                    }

                    var hasPhotoUrl = _analyzer.DoesContainPhotoUrl(tweet);
                    if (hasPhotoUrl)
                    {
                        tweetsWithPhotoUrl++;
                    }
                }

                response.TopEmoji = allEmojis
                    .GroupBy(e => e)
                    .OrderByDescending(e => e.Count())
                    .Select(e => e.Key)
                    .FirstOrDefault();

                //Percent of tweets that contain emojis
                response.PercentageWithEmojis = (double) tweetsWithEmojis / tweets.Count;

                //Top hastags
                response.TopHashtag = allHashtags
                    .GroupBy(e => e)
                    .OrderByDescending(e => e.Count())
                    .Select(e => e.Key)
                    .FirstOrDefault();

                //Percent of tweets that contain a url
                response.PercentageWithUrl = (double) tweetsWithUrl / tweets.Count;

                //Percent of tweets that contain a photo url
                response.PercentageWithPhotoUrl = (double) tweetsWithPhotoUrl / tweets.Count;

                //Top domains of urls in tweets
                response.TopDomain = allDomains
                    .GroupBy(e => e)
                    .OrderByDescending(e => e.Count())
                    .Select(e => e.Key)
                    .FirstOrDefault();

                return response;
            }
        }
    }
}
