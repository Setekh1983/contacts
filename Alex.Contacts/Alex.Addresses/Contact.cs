using Alex.DddBasics;

using System;

namespace Alex.Addresses
{
  public class Contact : Entity
  {
    public Contact(Name name)
    {
      _ = name ?? throw new ArgumentNullException(nameof(name));

      this.Name = name;
    }

    public Address Address { get; private set; }
    public Name Name { get; private set; }

    public void SetAddress(Address address)
    {
      _ = address ?? throw new ArgumentNullException(nameof(address));

      this.Address = address;
    }

    public void CorrectName(Name correctedName)
    {
      _ = correctedName ?? throw new ArgumentNullException(nameof(correctedName));

      this.Name = correctedName;
    }
  }
}