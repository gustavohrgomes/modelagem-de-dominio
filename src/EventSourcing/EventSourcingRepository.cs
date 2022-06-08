using EventStore.Client;
using NerdStore.Core.Data.EventSourcing;
using NerdStore.Core.Messages;
using System.Text;
using System.Text.Json;

namespace EventSourcing
{
    public class EventSourcingRepository : IEventSourcingRepository
    {
        private readonly IEventStoreService _eventStoreService;

        public EventSourcingRepository(IEventStoreService eventStoreService)
        {
            _eventStoreService = eventStoreService;
        }

        public async Task SalvarEvento<TEvent>(TEvent evento) where TEvent : Event
        {
            await _eventStoreService.Client.AppendToStreamAsync(
                streamName: evento.AggregateId.ToString(),
                expectedState: StreamState.Any,
                eventData: FormatarEvento(evento),
                cancellationToken: CancellationToken.None);
        }

        public async Task<IEnumerable<StoredEvent>> ObterEventos(Guid aggregateId)
        {
            var eventos = _eventStoreService.Client.ReadStreamAsync(
                direction: Direction.Forwards,
                streamName: aggregateId.ToString(),
                revision: StreamPosition.Start,
                maxCount: 500,
                resolveLinkTos: false);

            var listaEventos = new List<StoredEvent>();

            await foreach (var @event in eventos)
            {
                var dataEncoded = Encoding.UTF8.GetString(@event.Event.Data.ToArray());
                var jsonData = JsonSerializer.Deserialize<BaseEvent>(dataEncoded);

                var evento = new StoredEvent(
                    @event.Event.EventId.ToGuid(),
                    @event.Event.EventType,
                    jsonData.Timestamp,
                    dataEncoded);

                listaEventos.Add(evento);
            }

            return listaEventos.OrderBy(e => e.DataOcorrencia);
        }

        private static IEnumerable<EventData> FormatarEvento<TEvent>(TEvent evento) where TEvent : Event
        {
            yield return new EventData(
                Uuid.NewUuid(),
                evento.MessageType,
                JsonSerializer.SerializeToUtf8Bytes(evento));
        }
    }

    class BaseEvent
    {
        public DateTime Timestamp { get; set; }
    }
}
