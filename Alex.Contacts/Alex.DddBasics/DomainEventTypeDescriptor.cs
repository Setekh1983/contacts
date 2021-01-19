using System;

namespace Alex.DddBasics
{
  class DomainEventTypeDescriptor
  {
    
    public DomainEventTypeDescriptor(Type eventType, Type handlerType)
    {
      this.EventType = eventType;
      this.HandlerType = handlerType;
    }

    public Type EventType { get; }
    public Type HandlerType { get; }
  }
}