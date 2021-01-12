using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alex.DddBasics.Test.Domain
{
  class DomainEventHandlerStub
  {
    public static IList<IDomainEvent> HandledEvents = new List<IDomainEvent>();

    public class CitizenMarriedDomainEventHandler1Stub
    {

    }
    public class CitizenMarriedDomainEventHandler2Stub
    {

    }

    public class CitizenMovedDomainEventHandler1Stub
    {
    }
  }
}
