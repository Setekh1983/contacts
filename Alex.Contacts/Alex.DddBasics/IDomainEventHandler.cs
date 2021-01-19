using System.Threading.Tasks;

namespace Alex.DddBasics
{
  public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : IDomainEvent
  {
    Task Handle(TDomainEvent domainEvent);
  }
}
