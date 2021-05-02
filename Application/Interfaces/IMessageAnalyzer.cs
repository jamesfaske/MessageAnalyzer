using System;

namespace Application.Interfaces
{
    public interface IMessageAnalyzer
    {
        double MessageRatePerSecond(int timeSpanSeconds, int messageCount);
    }
}
