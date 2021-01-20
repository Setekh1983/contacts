using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alex.DddBasics.Test.Domain
{
  public sealed class CitizenDiedEvent : DomainEvent
  {
    public CitizenDiedEvent(Guid citizenId) => this.CitizenId = citizenId;

    public Guid CitizenId { get; }
  }
}
