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
                var timeSpan = DateTime.UtcNow - startTime;
                response.RatePerSecond = _analyzer.MessageRatePerSecond(timeSpan.Seconds, tweets.Count);

                //Top emojis
                foreach (var tweet in tweets)
                {
                    var emojiList = _emojiService.TotalMessagesWithEmojis(tweet);
                }
                

                //Percent of tweets that contain emojis

                
                //Top hastags


                //Percent of tweets that contain a url


                //Percent of tweets that contain a photo url


                //Top domains of urls in tweets


                return response;
            }
        }
    }
}
