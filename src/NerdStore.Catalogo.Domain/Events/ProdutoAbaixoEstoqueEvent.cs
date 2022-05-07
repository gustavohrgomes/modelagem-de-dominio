using NerdStore.Core.Messages.CommonMessages.DomainEvents;

namespace NerdStore.Catalogo.Domain.Events
{
    public class ProdutoAbaixoEstoqueEvent : DomainEvent
    {
        public ProdutoAbaixoEstoqueEvent(Guid aggragateId, int quantidadeRestante) : base(aggragateId)
        {
            QuantidadeRestante = quantidadeRestante;
        }

        public int QuantidadeRestante { get; private set; }
    }
}
