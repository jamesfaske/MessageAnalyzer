using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Application.Interfaces;
using EmojiData;

namespace Infrastructure
{
    public class EmojiService : IEmojiService
    {
        public List<string> TotalMessagesWithEmojis(string message)
        {
            var emojiList = new List<string>();

            foreach (Match match in Emoji.EmojiRegex.Matches(message))
            {
                var emoji = Emoji.GetSingleEmoji(match.Value);
                emojiList.Add(emoji.Name);
            }

            return emojiList;
        }

        private void Callback(Match match, SingleEmoji emoji)
        {
            Console.WriteLine("Found it!" + emoji);
        }
    }
}
