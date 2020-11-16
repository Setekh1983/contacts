using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alex.DddBasics.Test.Domain
{
  class Citizen2 : Entity
  {
    public Citizen2() { }
    public Citizen2(Guid guid)
      : base(guid)
    {
    }
  }
}
