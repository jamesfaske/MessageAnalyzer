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
            public List<string> Tweets { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Response>
        {
            private readonly IRepository _store;

            public CommandHandler(IRepository store)
            {
                _store = store;
            }

            public async Task<Response> Handle(Command cmd, CancellationToken cancellationToken)
            {
                var response = new Response();

                var tweets = _store.ReadAll();
                
                //calculate here
                response.Tweets = tweets;

                return response;
            }
        }
    }
}
