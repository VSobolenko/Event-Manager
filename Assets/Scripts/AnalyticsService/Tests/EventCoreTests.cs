using System.Collections.Generic;
using System.Threading.Tasks;
using AnalyticsService.SaveSystemProvider;
using AnalyticsService.ServerProviders;
using FluentAssertions;
using NUnit.Framework;

namespace AnalyticsService.Tests
{
    public class EventCoreTests
    {
        [Test]
        public void InitializeExistsEvents_ReadExisting_ShouldLoadOldAndClearInStorage()
        {
            var server = new ConsoleProvider();
            var save = new CollectionsProvider<EventData>
            {
                Data = GetTestEvents().events,
            };

            GetTestEvents().events.Should().BeEquivalentTo(save.Data);
            var eventCore = new EventCore(server, save, 5);
            Assert.IsEmpty(save.Data);
        }

        [Test]
        public async Task Dispose_SaveUnsentEvents_ShouldSendToServerAndClearEvents()
        {
            var server = new ConsoleProvider {ServerAvailability = false};
            var save = new CollectionsProvider<EventData>();
            var cooldownBeforeSend = 100;

            var eventCore = new EventCore(server, save, cooldownBeforeSend);
            eventCore.TrackEvent("type1", "data1");
            eventCore.TrackEvent("type2", "data2");
            await Task.Delay(ConsoleProvider.ServerDelay + cooldownBeforeSend);
            eventCore.Dispose();

            GetTestEvents().events.Should().BeEquivalentTo(save.Data);
            eventCore = new EventCore(server, save, cooldownBeforeSend);
            Assert.IsEmpty(save.Data);
        }

        [Test]
        public async Task InvokeFirstInitialize_SendExistingEvents_ShouldSendToServerAndClearEvents()
        {
            var server = new ConsoleProvider();
            var save = new CollectionsProvider<EventData>
            {
                Data = GetTestEvents().events,
            };

            Assert.IsNull(server.LastSendObject);

            var eventCore = new EventCore(server, save, 1);
            await Task.Delay(ConsoleProvider.ServerDelay);

            GetTestEvents().Should().BeEquivalentTo(server.LastSendObject);
        }

        [Test]
        public async Task TrackEvent_WithInternetCollectEventsBeforeCoolDown_ShouldSendToServerAndClearEvents()
        {
            var server = new ConsoleProvider();
            var save = new CollectionsProvider<EventData>();
            var cooldownBeforeSend = 100;

            var eventCore = new EventCore(server, save, cooldownBeforeSend);
            eventCore.TrackEvent("type1", "data1");
            eventCore.TrackEvent("type2", "data2");

            Assert.IsNull(server.LastSendObject);
            await Task.Delay(ConsoleProvider.ServerDelay + cooldownBeforeSend);
            GetTestEvents().Should().BeEquivalentTo(server.LastSendObject);
        }

        [Test]
        public async Task TrackEvent_WithInternetCollectEventsAfterCoolDown_ShouldSendToServerAndClearEvents()
        {
            var server = new ConsoleProvider();
            var save = new CollectionsProvider<EventData>();
            var cooldownBeforeSend = 100;

            var eventCore = new EventCore(server, save, cooldownBeforeSend);
            eventCore.TrackEvent("type1", "data1");

            await Task.Delay(ConsoleProvider.ServerDelay + cooldownBeforeSend);
            GetTestEventsPart1().Should().BeEquivalentTo(server.LastSendObject);

            eventCore.TrackEvent("type2", "data2");
            await Task.Delay(ConsoleProvider.ServerDelay + cooldownBeforeSend);

            GetTestEventsPart2().Should().BeEquivalentTo(server.LastSendObject);
        }

        [Test]
        public async Task TrackEvent_AfterConnectToInternet_ShouldSendToServerAndClearEvents()
        {
            var server = new ConsoleProvider {ServerAvailability = false};
            var save = new CollectionsProvider<EventData>();
            var cooldownBeforeSend = 100;

            var eventCore = new EventCore(server, save, cooldownBeforeSend);
            eventCore.TrackEvent("type1", "data1");

            await Task.Delay(ConsoleProvider.ServerDelay + cooldownBeforeSend);
            Assert.IsNull(server.LastSendObject);

            server.ServerAvailability = true;
            eventCore.TrackEvent("type2", "data2");
            await Task.Delay(ConsoleProvider.ServerDelay + cooldownBeforeSend);

            GetTestEvents().Should().BeEquivalentTo(server.LastSendObject);
        }

        private static Events GetTestEvents()
        {
            return new Events
            {
                events = new List<EventData>
                {
                    new EventData {data = "data1", type = "type1"},
                    new EventData {data = "data2", type = "type2"},
                }
            };
        }

        private static Events GetTestEventsPart1()
        {
            return new Events
            {
                events = new List<EventData>
                {
                    new EventData {data = "data1", type = "type1"},
                }
            };
        }

        private static Events GetTestEventsPart2()
        {
            return new Events
            {
                events = new List<EventData>
                {
                    new EventData {data = "data2", type = "type2"},
                }
            };
        }
    }
}