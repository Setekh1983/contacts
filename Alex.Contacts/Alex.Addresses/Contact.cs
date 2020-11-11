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

    private Name Name { get; }
  }
}