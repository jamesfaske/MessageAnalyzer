using Application.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Infrastructure.Data;
using Tweetinvi;

namespace Infrastructure.Messages
{
    public class TwitterService : ITwitterService
    {
        private TwitterClient TwitterClient { get; set; }
        private readonly IRepository _store;

        public TwitterService(IOptionsMonitor<TwitterConfig> twitterConfig, IRepository store)
        {
            var config = twitterConfig.CurrentValue;
            TwitterClient = new TwitterClient(config.ConsumerKey, config.ConsumerSecret, config.AccessToken, config.AccessTokenSecret);
            _store = store;
        }

        public async Task StartStreamAsync()
        {
            var sampleStream = TwitterClient.Streams.CreateSampleStream();
            sampleStream.TweetReceived += (sender, eventArgs) =>
            {
                Console.WriteLine(eventArgs.Tweet);
                _store.AddLine(eventArgs.Tweet.ToString());
            };

            await sampleStream.StartAsync();
        }
    }
}
