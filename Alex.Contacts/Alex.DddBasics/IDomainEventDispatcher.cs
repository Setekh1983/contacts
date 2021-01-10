using System.Threading.Tasks;

namespace Alex.DddBasics
{
  public interface IDomainEventDispatcher
  {
    Task Dispatch(IDomainEvent domainEvent);
  }
}
