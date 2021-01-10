using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alex.Contacts.Service
{
  public class AddAddressCommand
  {
    public string City { get; set; }
    public string CityCode { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public Guid ContactId { get; set; }
  }
}
