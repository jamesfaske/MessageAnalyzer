using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application;
using Application.Features;
using Application.Interfaces;
using NUnit.Framework;

namespace Tests.Unit
{
    [TestFixture]
    [Parallelizable]
    public class MessageAnalyzerTests
    {
        private MessageAnalyzer _analyzer;

        [OneTimeSetUp]
        public void OneTime()
        {
            _analyzer = new MessageAnalyzer();
        }

        public void InvalidTimespan_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _analyzer.MessageRatePerSecond(0, 10));
        }

        public void InvalidMessageCount_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _analyzer.MessageRatePerSecond(10, 0));
        }

        [Test]
        [TestCase(1, 1, ExpectedResult = 1)]
        [TestCase(60, 60, ExpectedResult = 1)]
        [TestCase(500, 60, ExpectedResult = 0.12)]
        public double GoodParams_ReturnsCorrectRate(int timeSpan, int messageCount)
        {
            return _analyzer.MessageRatePerSecond(timeSpan, messageCount);
        }
    }
}
