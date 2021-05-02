using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;

namespace Infrastructure.Data
{
    public class InMemoryStore : IRepository
    {
        private ConcurrentDictionary<int, string> Messages = new ConcurrentDictionary<int, string>();
        private int Index { get; set; }

        public void AddLine(string line)
        {
            Messages.TryAdd(Index, line);
            Index++;
        }

        public List<string> ReadAll()
        {
            var messages = new List<string>();

            for (int i = 0; i < Index; i++)
            {
                Messages.TryGetValue(i, out string message);
                messages.Add(message);
            }

            return messages;
        }
    }
}
