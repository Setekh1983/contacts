using System;

namespace Alex.DddBasics
{
  public interface IDomainEvent
  {
    Guid EventId { get; }
  }
}