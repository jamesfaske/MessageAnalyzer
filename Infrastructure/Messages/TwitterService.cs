using Application.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;

namespace Infrastructure.Messages
{
    public class TwitterService : ITwitterService
    {
        public TwitterClient _twitterClient { get; set; }

        public TwitterService(IOptionsMonitor<TwitterConfig> twitterConfig)
        {
            var config = twitterConfig.CurrentValue;
            _twitterClient = new TwitterClient(config.ConsumerKey, config.ConsumerSecret, config.AccessToken, config.AccessTokenSecret);
        }

        public async Task StartStreamAsync()
        {
            var sampleStream = _twitterClient.Streams.CreateSampleStream();
            sampleStream.TweetReceived += (sender, eventArgs) =>
            {
                Console.WriteLine(eventArgs.Tweet);
            };

            await sampleStream.StartAsync();
        }
    }
}
