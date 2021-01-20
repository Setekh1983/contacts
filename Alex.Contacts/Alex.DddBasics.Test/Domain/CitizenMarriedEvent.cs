using System;

namespace Alex.DddBasics.Test.Domain
{
  public sealed class CitizenMarriedEvent : DomainEvent
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