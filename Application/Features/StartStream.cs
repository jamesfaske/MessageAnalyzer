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
    public class StartStream
    {
        public class Command : IRequest<Response>
        {
        }

        public class Response : ResponseBase
        {
        }

        public class CommandHandler : IRequestHandler<Command, Response>
        {
            private readonly ITwitterService _twitterService;

            public CommandHandler(ITwitterService twitterService)
            {
                _twitterService = twitterService;
            }

            public async Task<Response> Handle(Command cmd, CancellationToken cancellationToken)
            {
                var response = new Response();

                try
                {
                    await _twitterService.StartStreamAsync();
                }
                catch (Exception ex)
                {
                    response.Errors.Add($"An error occurred reading the stream: {ex.Message}");
                    
                }

                return response;
            }
        }
    }
}
