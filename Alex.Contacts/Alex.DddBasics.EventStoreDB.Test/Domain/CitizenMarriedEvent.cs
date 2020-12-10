﻿using System;

namespace Alex.DddBasics.EventStoreDB.Test.Domain
{
  public sealed class CitizenMarriedEvent : IDomainEvent
  {

    public CitizenMarriedEvent(Guid citizen, Guid marriedToCitizen)
    {
      this.Citizen = citizen;
      this.MarriedToCitizen = marriedToCitizen;
    }

    public Guid Citizen { get; }
    public Guid MarriedToCitizen { get; }
  }
}