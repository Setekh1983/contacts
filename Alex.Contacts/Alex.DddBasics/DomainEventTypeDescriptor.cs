﻿using System;

namespace Alex.DddBasics
{
  class DomainEventTypeDescriptor
  {
    
    public DomainEventTypeDescriptor(Type concreteType, Type interfaceType)
    {
      this.ConcreteType = concreteType;
      this.InterfaceType = interfaceType;
    }

    public Type ConcreteType { get; }
    public Type InterfaceType { get; }
  }
}