using EventStore.Client;
using Microsoft.Extensions.Configuration;

namespace EventSourcing
{
    public class EventStoreService : IEventStoreService
    {
        private readonly EventStoreClient _eventStoreClient;

        public EventStoreService(EventStoreClient eventStoreClient)
        {
            _eventStoreClient = eventStoreClient;
        }

        public EventStoreClient Client => _eventStoreClient;
    }
}
