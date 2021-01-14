using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alex.DddBasics
{
  public interface IDomainEventDispatcher
  {
    Task Dispatch<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : IDomainEvent;
    Task Dispatch<TDomainEvent>(IEnumerable<TDomainEvent> domainEvents) where TDomainEvent : IDomainEvent;
  }
}
