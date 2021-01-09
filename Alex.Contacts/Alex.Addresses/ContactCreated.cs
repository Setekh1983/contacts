using Alex.DddBasics;

using System;

namespace Alex.Addresses
{
  public class ContactCreated : IDomainEvent
  {
    public string Forename { get; }
    public string LastName { get; }
    public Guid ContactId { get; }

    public ContactCreated(Guid contactId, string forename, string lastName)
    {
      this.Forename = forename;
      this.LastName = lastName;
      this.ContactId = contactId;
    }
  }
}