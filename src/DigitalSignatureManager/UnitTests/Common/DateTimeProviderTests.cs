using System;
using Common.DateTime;
using NUnit.Framework;

namespace UnitTests.Common
{
    [TestFixture]
    public class DateTimeProviderTests
    {
        private const int ErrorMarginInMs = 100;
        private DateTimeProvider _dateTimeProvider;

        [SetUp]
        public void SetUp()
        {
            _dateTimeProvider = new DateTimeProvider();
        }

        [Test]
        public void UtcNow_ShouldBeCloseToSystemUtcNow()
        {
            var systemUtcNow = DateTime.UtcNow;
            var providerUtcNow = _dateTimeProvider.UtcNow;

            var diff = providerUtcNow - systemUtcNow;

            Assert.LessOrEqual(diff.TotalMilliseconds, ErrorMarginInMs,
                $"Error margin between system clock and actual clock should be less than {ErrorMarginInMs}");
        }
    }
}