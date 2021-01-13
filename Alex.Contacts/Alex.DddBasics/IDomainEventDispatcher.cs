using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alex.DddBasics
{
  public interface IDomainEventDispatcher
  {
    Task Dispatch(IDomainEvent domainEvent);
    Task Dispatch(IEnumerable<IDomainEvent> domainEvents);
  }
}
