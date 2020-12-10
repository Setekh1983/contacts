using System.Threading.Tasks;

namespace Alex.DddBasics
{
  public interface IRepository<TAggregate> where TAggregate : AggregateRoot
  {
    Task SaveAsync(TAggregate aggregate);
  }
}