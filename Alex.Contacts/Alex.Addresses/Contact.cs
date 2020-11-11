using System;

namespace Alex.Addresses
{
  public class Contact
  {
    public Contact(Name name)
    {
      _ = name ?? throw new ArgumentNullException(nameof(name));

      this.Name = name;
    }

    public Address Address { get; private set; }
    public Name Name { get; }

    public void AddAddress(Address address)
    {
      _ = address ?? throw new ArgumentNullException(nameof(address));

      this.Address = address;
    }
  }
}