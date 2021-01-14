using System.Threading.Tasks;

namespace Alex.DddBasics
{
  public interface IDomainEventHandler<TDomainEvent> where TDomainEvent : IDomainEvent
  {
    Task Handle(TDomainEvent domainEvent);
  }
}
